using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPIWebApp.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public int Rating { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
