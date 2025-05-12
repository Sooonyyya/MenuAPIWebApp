namespace MenuAPIWebApp.Models.DTO
{
    public class ReviewDTO
    {
        public int DishId { get; set; }

        public string UserName { get; set; } = null!;

        public string Text { get; set; } = null!;
    }
}
