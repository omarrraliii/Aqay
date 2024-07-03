
using aqay_apis.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Interfaces;
using Org.BouncyCastle.Bcpg.Sig;
using System.Numerics;
using System;
using System.Reflection.Emit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System.Threading.Channels;

namespace aqay_apis.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //for Identity useres Change user table names into simpler ones
            builder.Entity<User>().ToTable("Users")
                .Ignore(u => u.PhoneNumberConfirmed);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<Consumer>()
            .HasOne(c => c.WishList)
            .WithOne(w => w.Consumer)
            .HasForeignKey<Consumer>(w => w.WishListId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Plan>().HasData(
            new Plan { Id = 1, Name = "Annual subscription", Describtion= "Annual Subscription plan,merchants gain full access to our e - commerce platform for a whole year.", Price = 3600 },
            new Plan { Id = 2, Name = "Quarterly subscription", Describtion = "Our Quarterly Subscription plan provides merchants with a balance between commitment and flexibility.", Price = 1500 },
            new Plan { Id = 3, Name = "Monthly subscription", Describtion = "Our Monthly Subscription plan offers the flexibility of paying on a monthly basis.", Price = 600 }
            );
            
            builder.Entity<Subscription>().HasData(
            new Subscription
            {
                Id = 1,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 1, 1),
                PlanId = 1
            }
            );
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<PendingMerchant> PendingMerchants { get; set; }
        ////////////////// FOR TESTING 
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
    }

}
