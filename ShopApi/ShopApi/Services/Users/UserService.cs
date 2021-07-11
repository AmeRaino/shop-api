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
        Task<User> Create(User user);
    }
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory scopeFactory;
        public UserService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }
        public async Task<User> Create(User user)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                user.Id = IdentityHelper.GenerateId();
                user.DateCreated = DateTimeOffset.Now;

                //await appDb.Users.AddAsync(user);
                //await appDb.SaveChangesAsync();
                return user;
            }
        }
    }
}
