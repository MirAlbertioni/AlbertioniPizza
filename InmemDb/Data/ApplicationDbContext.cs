using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InmemDb.Models;
using InmemDb.Models.ManageViewModels;

namespace InmemDb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>().HasKey(x => new { x.DishId, x.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(i => i.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(x => x.DishId);


            builder.Entity<DishIngredient>()
                .HasOne(i => i.Ingredient)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(x => x.IngredientId);



            builder.Entity<CartItemIngredient>()
                .HasOne(i => i.CartItem)
                .WithMany(d => d.CartItemIngredient)
                .HasForeignKey(x => x.CartItemId);


            builder.Entity<CartItemIngredient>()
                .HasOne(i => i.Ingredient)
                .WithMany(d => d.CartItemIngredient)
                .HasForeignKey(x => x.IngredientId);


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CartItemIngredient> CartItemIngredients { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<OrderConfirmation> Order { get; set; }
    }
}