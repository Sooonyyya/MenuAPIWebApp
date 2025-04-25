using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPIWebApp.Models
{
    public class FavoriteDish
    {
        [Key]
        public int Id { get; set; }

        public string UserIdentifier { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
