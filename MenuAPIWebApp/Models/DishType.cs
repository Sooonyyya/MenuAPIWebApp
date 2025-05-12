using System.ComponentModel.DataAnnotations;

public class DishType
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
