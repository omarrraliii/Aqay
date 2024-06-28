
using aqay_apis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
                .HasOne(c => c.Review)
                .WithOne(r => r.Consumer)
                .HasForeignKey<Review>(r => r.ConsumerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Consumer>()
                .HasOne(s=>s.ShoppingCart)
                .WithOne(s => s.Consumer)
                .HasForeignKey<ShoppingCart>(s=>s.ConsumerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Consumer>()
                .HasOne(c => c.WishList)
                .WithOne(w => w.Consumer)
                .HasForeignKey<Consumer>(w => w.WishListId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Plan>().HasData(
               new Plan { Id = 1, Name = "Monthly", Describtion = "Monthly subscription", Price = 9.99 },
               new Plan { Id = 2, Name = "Quarterly", Describtion = "Quarterly subscription", Price = 27.99 },
               new Plan { Id = 3, Name = "Yearly", Describtion = "Yearly subscription", Price = 99.99 }
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
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<About> Abouts { get; set; }
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
        ////////////////// FOR TESTING 
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
    }

}
