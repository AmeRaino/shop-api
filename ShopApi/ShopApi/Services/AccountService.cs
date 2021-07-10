using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopApi.EntityFrameworkCore;
using ShopApi.Models.User;
using ShopApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ShopApi.Services
{

    public interface IAccountService
    {
        Task<User> AuthenticateUserAccount(string username, string password);
        //Task<User> Register(UserRegister userRegister);
    }

    public class AccountService : IAccountService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public AccountService(IServiceScopeFactory scopeFactory)
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

                string hashed = IdentityHelper.HashPasswordWithSalt(password, userPasswordSalt);

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

        public async Task<UserAccount> Register(string username, string password, User user)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userAccount = new UserAccount();

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

                var result = IdentityHelper.HashPasswordWithRandomSalt(password);

                userAccount.Id = "";
                userAccount.Username = username;
                userAccount.PasswordHash = result.hashed;
                userAccount.PasswordSalt = result.salt;
                userAccount.DateCreated = DateTimeOffset.Now;
                userAccount.User = user;

                await appDb.UserAccounts.AddAsync(new UserAccount());
                appDb.SaveChanges();

                return userAccount;
            }
        }
    }
}
