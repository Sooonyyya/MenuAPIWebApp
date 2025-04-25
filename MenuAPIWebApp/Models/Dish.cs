using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace MenuAPIWebApp.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("DishType")]
        public int DishTypeId { get; set; }
        public DishType DishType { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }

        public int Calories { get; set; }
        public string Description { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<FavoriteDish> FavoriteDishes { get; set; }
        public ICollection<DishIngredient> DishIngredients { get; set; }
    }
}
