using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FavoriteDish
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserSessionId { get; set; } = null!;

    [ForeignKey("Dish")]
    public int DishId { get; set; }
    public Dish Dish { get; set; } = null!;
}
