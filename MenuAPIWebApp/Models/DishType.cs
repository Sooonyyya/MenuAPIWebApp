using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MenuAPIWebApp.Models
{
    public class DishType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Dish> Dishes { get; set; }
    }
}
