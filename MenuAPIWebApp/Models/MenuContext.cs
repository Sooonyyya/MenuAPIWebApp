using Microsoft.EntityFrameworkCore;

public class MenuContext : DbContext
{
    public MenuContext(DbContextOptions<MenuContext> options) : base(options) { }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<DishType> DishTypes => Set<DishType>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<DishIngredient> DishIngredients => Set<DishIngredient>();
    public DbSet<FavoriteDish> FavoriteDishes => Set<FavoriteDish>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite primary key for DishIngredient
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
    }
}
