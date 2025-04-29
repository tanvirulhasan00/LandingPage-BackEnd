using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LandingPage.Data
{
    public class LandingPageDbContext(DbContextOptions<LandingPageDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<AllCost>? AllCosts { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductSize>? ProductSizes { get; set; }
        public DbSet<ProductDefination>? ProductDefinations { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Payment>? Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductSize>().HasData(
                new ProductSize
                {
                    Id = 1,
                    Size = "small"
                },
                new ProductSize
                {
                    Id = 2,
                    Size = "medium"
                },
                new ProductSize
                {
                    Id = 3,
                    Size = "large"
                },
                new ProductSize
                {
                    Id = 4,
                    Size = "xl"
                },
                new ProductSize
                {
                    Id = 5,
                    Size = "2xl"
                },
                new ProductSize
                {
                    Id = 6,
                    Size = "3xl"
                }
            );
        }

    }
}