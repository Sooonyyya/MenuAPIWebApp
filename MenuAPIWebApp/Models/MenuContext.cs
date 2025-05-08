using Microsoft.EntityFrameworkCore;

namespace MenuAPIWebApp.Models
{
    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteDish> FavoriteDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            modelBuilder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            modelBuilder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);

            modelBuilder.Entity<FavoriteDish>()
                .HasOne(fd => fd.User)
                .WithMany()
                .HasForeignKey(fd => fd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
