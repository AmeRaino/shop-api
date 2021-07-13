using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Dtos.Authentications;
using ShopApi.Dtos.Registers;
using ShopApi.Dtos.Users;
using ShopApi.Entity;
using ShopApi.Extensions;
using ShopApi.Helpers;
using ShopApi.Models.Users;
using ShopApi.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string Id);
        void Update(string id, UpdateUserModel model);
        Task<User> RegisterAsync(UserRegisterRequest model);
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        Task<ThirdPartyAuthenticateResponse> Authenticate3rdPartyAsync(ThirdPartyAuthenticateRequest model);
    }
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly AppSettings _appSettings;


        public UserService(IServiceScopeFactory scopeFactory, IOptions<AppSettings> appSettings)
        {
            this.scopeFactory = scopeFactory;
            _appSettings = appSettings.Value;
        }

        #region Get User
        public async Task<User> GetByIdAsync(string Id)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await appDb.Users.SingleOrDefaultAsync(x => x.Id == Id);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await appDb.Users.ToListAsync();
            }
        }

        #endregion

        #region Update User
        public void Update(string id, UpdateUserModel model)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var user = appDb.Users.Where(x => x.Id == id).FirstOrDefault();
                if (user == null)
                    throw new ApplicationException("User not found.");

                if (!string.IsNullOrEmpty(model.Firstname))
                    user.Firstname = model.Firstname;

                if (!string.IsNullOrEmpty(model.Lastname))
                    user.Lastname = model.Lastname;

                if (!string.IsNullOrEmpty(model.Phone))
                    user.Phone = model.Phone;

                if (!string.IsNullOrEmpty(model.Email))
                    user.Email = model.Email;

                if (!string.IsNullOrEmpty(model.ImageUrl))
                    user.ImageUrl = model.ImageUrl;

                if (model.Birthday != null)
                    user.Birthday = model.Birthday;

                appDb.SaveChanges();
            }
        }
        #endregion

        #region Register
        public async Task<User> RegisterAsync(UserRegisterRequest model)
        {
            checkValidModel(model);

            var user = new User();
            user.CopyPropertiesFrom(model);
            user.Id = IdentityUtil.GenerateId();
            user.DateCreated = DateTimeOffset.Now;

            var account = new UserAccount();
            account.CopyPropertiesFrom(model);
            var hashedResult = PasswordUtil.HashPasswordWithRandomSalt(model.Password);
            account.Id = String.Format("AC{0}", user.Id);
            account.PasswordHash = hashedResult.hashed;
            account.PasswordSalt = hashedResult.salt;
            account.DateCreated = DateTimeOffset.Now;
            account.User = user;

            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                appDb.UserAccounts.Add(account);
                await appDb.SaveChangesAsync();
                return user;
            }
        }

        #endregion


        #region Authenticate
        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            checkValidModel(model);
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var found = await appDb.UserAccounts
                    .Where(x => x.Username == model.Username)
                    .Include(account => account.User)
                    .FirstOrDefaultAsync();

                if (found == null)
                    throw new ApplicationException("That username is invalid.");

                if (found.User == null)
                    throw new ApplicationException("User not found.");

                string hashed = PasswordUtil.HashPasswordWithSalt(model.Password, found.PasswordSalt);

                if (hashed.Equals(found.PasswordHash))
                {
                    var response = new AuthenticateResponse
                    {
                        Token = generateJwtToken(found.User)
                    };
                    response.CopyPropertiesFrom(found.User);
                    return response;
                }
                else
                {
                    throw new ApplicationException("Incorrect password.");
                }
            }
        }

        public async Task<ThirdPartyAuthenticateResponse> Authenticate3rdPartyAsync(ThirdPartyAuthenticateRequest model)
        {
            checkValidModel(model);
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var found = await appDb.AuthenticationProviders
                    .Where(x => x.Id == String.Format("{0}{1}", model.ProviderType.ToString(), model.KeyProvided))
                    .Include(x => x.User)
                    .FirstOrDefaultAsync();

                if (found == null)
                {
                    var newAuth = await insertAuthenticationProviderAsync(model);
                    var response = new ThirdPartyAuthenticateResponse
                    {
                        Token = generateJwtToken(newAuth.User)
                    };
                    response.CopyPropertiesFrom(newAuth.User);
                    return response;
                }
                else
                {
                    var response = new ThirdPartyAuthenticateResponse
                    {
                        Token = generateJwtToken(found.User)
                    };
                    response.CopyPropertiesFrom(found.User);
                    return response;
                }
            }
        }

        #endregion

        #region Helper Function

        private async Task<AuthenticationProvider> insertAuthenticationProviderAsync(ThirdPartyAuthenticateRequest model)
        {
            var authenticationProvider = new AuthenticationProvider();
            authenticationProvider.User = new User
            {
                Id = IdentityUtil.GenerateId(),
                DateCreated = DateTimeOffset.Now,
            };
            authenticationProvider.CopyPropertiesFrom(model);

            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                authenticationProvider.Id = String.Format("{0}{1}", authenticationProvider.ProviderTypeString, authenticationProvider.KeyProvided);
                appDb.AuthenticationProviders.Add(authenticationProvider);
                await appDb.SaveChangesAsync();
                return authenticationProvider;
            }
        }

        private void checkValidModel(ThirdPartyAuthenticateRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.KeyProvided))
                throw new ApplicationException("`KeyProvided` can not be null or white space.");

            if (model.ProviderType == ProviderType.Undefined || !Enum.IsDefined(typeof(ProviderType), model.ProviderType))
                throw new ApplicationException("`ProviderType` is invalid.");
        }

        private void checkValidModel(AuthenticateRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                throw new ApplicationException("Username is required.");

            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ApplicationException("Password is required.");

            if (model.ReturnUrl != null && !RegexUtilities.IsValidUrl(model.ReturnUrl))
                throw new ApplicationException("`Return url` is invalid.");
        }

        private void checkValidModel(UserRegisterRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Username))
                throw new ApplicationException("Username is required.");

            if (isExistedUsername(model.Username))
                throw new ApplicationException("That username is already taken. Please try another.");

            if (string.IsNullOrWhiteSpace(model.Password))
                throw new ApplicationException("Password is required.");

            if (model.Email != null)
            {
                if (!RegexUtilities.IsValidEmail(model.Email))
                    throw new ApplicationException("That email is invalid. Please try another.");

                if (isExistedEmail(model.Email))
                    throw new ApplicationException("This email is already used in another account. Please try another.");
            }
        }

        private bool isExistedEmail(string email)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (appDb.Users.Any(user => user.Email == email))
                    return true;

                return false;
            }
        }

        private bool isExistedUsername(string username)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (appDb.UserAccounts.Any(x => x.Username == username))
                    return true;

                return false;
            }
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

    }
}
