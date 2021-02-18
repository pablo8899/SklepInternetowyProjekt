using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepInternetowy.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SklepInternetowy.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<FavoriteProductEntity> FavoriteProducts { get; set; }
        public DbSet<CategoriesEntity> Categories { get; set; }
        public DbSet<ShoppingCartItemEntity> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCartEntity> ShoppingCart { get; set; }
        public DbSet<PromoCodeEntity> PromoCodes { get; set; }
        public DbSet<ShippingEntity> Shipping { get; set; }
        public DbSet<ShoppingHistoryEntity> ShoppingHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
