using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;
using MenuAPIWebApp.Models.DTO;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly MenuContext _context;

    public ReviewsController(MenuContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get() =>
        await _context.Reviews.Include(r => r.Dish).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<Review>> Post(ReviewDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var review = new Review
        {
            DishId = dto.DishId,
            UserName = dto.UserName,
            Text = dto.Text,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Post), new { id = review.Id }, review);
    }
}
