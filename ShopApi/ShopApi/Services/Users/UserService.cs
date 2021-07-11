using Microsoft.Extensions.DependencyInjection;
using ShopApi.EntityFrameworkCore;
using ShopApi.Models.Users;
using ShopApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public interface IUserService
    {
        User Create(User user);
    }
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public UserService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public User Create(User user)
        {
            if (!RegexUtilities.IsValidEmail(user.Email))
                throw new ApplicationException("That email is invalid. Please try another.");

            if (IsExistedEmail(user.Email))
                throw new ApplicationException("This email is already used in another account. Please try another.");

            user.Id = IdentityHelper.GenerateId();
            user.DateCreated = DateTimeOffset.Now;

            return user;
        }

        bool IsExistedEmail(string email)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (appDb.Users.Any(user => user.Email == email))
                    return true;

                return false;
            }
        }
    }
}
