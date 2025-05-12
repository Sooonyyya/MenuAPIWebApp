using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Dish
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Price { get; set; }

    public int Calories { get; set; }

    [ForeignKey("DishType")]
    public int DishTypeId { get; set; }
    public DishType DishType { get; set; } = null!;

    public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<FavoriteDish> FavoriteDishes { get; set; } = new List<FavoriteDish>();
}
