using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly MenuContext _context;

        public ReviewsController(MenuContext context)
        {
            _context = context;
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetByDish(int dishId)
        {
            return await _context.Reviews
                .Where(r => r.DishId == dishId)
                .Include(r => r.User)
                .ToListAsync();
        }

        // Приклад POST: api/reviews?username=Sofia
        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string username, [FromBody] Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
                return BadRequest("Рейтинг має бути від 1 до 5.");

            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Ім’я користувача обов'язкове.");

            // Шукаємо або створюємо юзера
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                user = new User
                {
                    UserName = username,
                    Email = $"{Guid.NewGuid()}@generated.local",
                    PasswordHash = Guid.NewGuid().ToString()
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            review.UserId = user.Id;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
