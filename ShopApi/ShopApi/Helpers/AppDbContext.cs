using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopApi.Entity;
using ShopApi.Entity.Models;
using System;

namespace ShopApi.Helpers
{
    public class AppDbContext : DbContext
    {
        // User
        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationProvider> AuthenticationProviders { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        // Product
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSku> ProductSkues { get; set; }
        public DbSet<ProductSkuValue> ProductSkuValues { get; set; }
        public DbSet<ProductOption> ProductVariants { get; set; }
        public DbSet<ProductOptionValue> ProductVariantOptions { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ProductSku
            modelBuilder
                .Entity<ProductSku>()
                .HasKey(p => new { p.ProductId, p.SkuId });

            modelBuilder
                .Entity<ProductSku>()
                .HasOne(p => p.Product)
                .WithMany(ps => ps.ProductSkus)
                .HasForeignKey(x => x.ProductId);

            modelBuilder
                .Entity<ProductSku>()
                .HasIndex(p => p.Sku);

            modelBuilder
                .Entity<ProductSku>()
                .Property(p => p.SkuId).ValueGeneratedOnAdd();

            // ProductSkuValue
            modelBuilder.Entity<ProductSkuValue>()
                .HasOne(p => p.ProductSku)
                .WithMany(p => p.ProductSkuValues)
                .HasForeignKey(x => new { x.ProductId, x.SkuId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ProductSkuValue>()
                .HasKey(p => new { p.ProductId, p.SkuId, p.OptionId });

            modelBuilder
                .Entity<ProductSkuValue>()
                .HasOne(p => p.ProductOptionValue)
                .WithMany(ps => ps.ProductSkuValues)
                .HasForeignKey(x => new { x.ProductId, x.OptionId, x.ValueId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ProductSkuValue>()
                .HasOne(p => p.ProductOption)
                .WithMany(ps => ps.ProductSkuValues)
                .HasForeignKey(x => new { x.ProductId, x.OptionId })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ProductOptionValue>()
                .HasKey(p => new { p.ProductId, p.OptionId, p.ValueId });

            modelBuilder
                .Entity<ProductOptionValue>()
                .HasOne(p => p.ProductOption)
                .WithMany(ps => ps.ProductOptionValues)
                .HasForeignKey(x => new { x.ProductId, x.OptionId });
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<ProductOptionValue>()
                .Property(p => p.ValueId).ValueGeneratedOnAdd();


            modelBuilder
                .Entity<ProductOption>()
                .HasKey(p => new { p.ProductId, p.OptionId });

            modelBuilder
                .Entity<ProductOption>()
                .HasOne(p => p.Product)
                .WithMany(po => po.ProductOptions)
                .HasForeignKey(x => new { x.ProductId })
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<ProductOption>()
                .Property(p => p.OptionId).ValueGeneratedOnAdd();
        }
    }
}
