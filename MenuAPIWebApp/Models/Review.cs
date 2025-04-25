using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPIWebApp.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
