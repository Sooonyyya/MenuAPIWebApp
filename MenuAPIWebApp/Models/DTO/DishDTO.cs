namespace MenuAPIWebApp.Models.DTO
{
    public class DishDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Calories { get; set; }

        public int DishTypeId { get; set; }

        public List<int> IngredientIds { get; set; } = new();
    }
}
