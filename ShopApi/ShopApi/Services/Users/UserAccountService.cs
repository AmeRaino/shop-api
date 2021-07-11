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
        Task<User> AuthenticatAccount(string username, string password);
        Task<UserAccount> Register(UserAccount userAccount, string password);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public UserAccountService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        // Login
        public async Task<User> AuthenticatAccount(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ApplicationException("Username is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ApplicationException("Password is required.");

            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var found = await appDb.UserAccounts
                    .Where(x => x.Username == username)
                    .Include(account => account.User)
                    .FirstOrDefaultAsync();

                if (found == null)
                    throw new ApplicationException("That username is invalid.");

                if (found.User == null)
                    throw new ApplicationException("User not found.");

                string hashed = PasswordHelper.HashPasswordWithSalt(password, found.PasswordSalt);

                if (hashed.Equals(found.PasswordHash))
                {
                    return found.User;
                }
                else
                {
                    throw new ApplicationException("Incorrect password.");
                }
            }
        }

        bool IsExistedUsername(string username)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (appDb.UserAccounts.Any(x => x.Username == username))
                    return true;

                return false;
            }
        }

        // Signup
        public async Task<UserAccount> Register(UserAccount userAccount, string password)
        {
            if (string.IsNullOrWhiteSpace(userAccount.Username))
                throw new ApplicationException("Username is required.");

            if (IsExistedUsername(userAccount.Username))
                throw new ApplicationException("That username is already taken. Please try another.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ApplicationException("Password is required.");

            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
