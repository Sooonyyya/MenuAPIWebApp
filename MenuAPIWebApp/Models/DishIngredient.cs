using System.ComponentModel.DataAnnotations.Schema;

public class DishIngredient
{
    [ForeignKey("Dish")]
    public int DishId { get; set; }
    public Dish Dish { get; set; } = null!;

    [ForeignKey("Ingredient")]
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
}
