using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.EntityFrameworkCore;
using ShopApi.Models.Users;
using ShopApi.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Services
{

    public interface IUserAccountService
    {
        Task<User> AuthenticateUserAccount(string username, string password);
        Task<UserAccount> Register(UserAccount userAccount, string password);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public UserAccountService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task<User> AuthenticateUserAccount(string username, string password)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (string.IsNullOrWhiteSpace(username))
                    throw new ApplicationException("Username is required!");

                if (string.IsNullOrWhiteSpace(password))
                    throw new ApplicationException("Password is required!");

                var found = await appDb.UserAccounts.Where(x => x.Username == username).FirstOrDefaultAsync();


                if (found == null)
                    throw new ApplicationException("Invalid username");

                var userPasswordHashed = found.PasswordHash;
                string userPasswordSalt = found.PasswordSalt;

                //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //    password: password,
                //    salt: Convert.FromBase64String(userPasswordSalt),
                //    prf: KeyDerivationPrf.HMACSHA1,
                //    iterationCount: 10000,
                //    numBytesRequested: 256 / 8));

                string hashed = PasswordHelper.HashPasswordWithSalt(password, userPasswordSalt);

                if (hashed.Equals(userPasswordHashed))
                {
                    return found.User;
                }
                else
                {
                    throw new ApplicationException("Invalid username");
                }
            }
        }

        public async Task<UserAccount> Register(UserAccount userAccount, string password)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                //byte[] salt = new byte[128 / 8];
                //using (var rng = RandomNumberGenerator.Create())
                //{
                //    rng.GetBytes(salt);
                //}

                //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //    password: password,
                //    salt: salt,
                //    prf: KeyDerivationPrf.HMACSHA1,
                //    iterationCount: 10000,
                //    numBytesRequested: 256 / 8));

                var result = PasswordHelper.HashPasswordWithRandomSalt(password);

                userAccount.Id = String.Format("AC{0}", userAccount.User.Id);
                userAccount.PasswordHash = result.hashed;
                userAccount.PasswordSalt = result.salt;
                userAccount.DateCreated = DateTimeOffset.Now;

                await appDb.UserAccounts.AddAsync(userAccount);
                appDb.SaveChanges();

                return userAccount;
            }
        }
    }
}
