using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MenuAPIWebApp.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<DishIngredient> DishIngredients { get; set; }
    }
}
