using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;

namespace InventMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Till> Till { get; set; }
        public DbSet<Transaction> Transaction { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
               .HasOne(d => d.Category)
               .WithMany(d=> d.Products)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Till>()
                .HasOne(d => d.User)
                .WithMany(d => d.Tills)
                .OnDelete(DeleteBehavior.Restrict);

            

            builder.Entity<Transaction>()
                .HasOne(d => d.User)
                .WithMany(d => d.Transactions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(d => d.Till)
                .WithMany(d => d.Transactions)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasMany(d => d.Products)
                .WithMany(d => d.Transactions);
            base.OnModelCreating(builder);
        }
    }
}
