using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("Dish")]
    public int DishId { get; set; }
    public Dish Dish { get; set; } = null!;
}
