using System.ComponentModel.DataAnnotations;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();
}
