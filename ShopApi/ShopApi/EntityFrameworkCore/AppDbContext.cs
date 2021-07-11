using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        public AppDbContext(IServiceProvider services, DbContextOptions dbOptions)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
        }
    }
}
