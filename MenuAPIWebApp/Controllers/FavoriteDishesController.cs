using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteDishesController : ControllerBase
    {
        private readonly MenuContext _context;

        public FavoriteDishesController(MenuContext context)
        {
            _context = context;
        }

        // Отримати улюблені страви користувача
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<FavoriteDish>>> GetByUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return NotFound("Користувача не знайдено");

            var favorites = await _context.FavoriteDishes
                .Where(fd => fd.UserId == user.Id)
                .Include(fd => fd.Dish)
                .ToListAsync();

            return Ok(favorites);
        }

        // Додати улюблену страву
        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string username, [FromBody] FavoriteDish favoriteDish)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Ім’я користувача обов’язкове.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                user = new User
                {
                    UserName = username,
                    Email = $"{Guid.NewGuid()}@generated.user",
                    PasswordHash = Guid.NewGuid().ToString()
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // ❗ Перевірка: чи вже існує ця улюблена страва для користувача
            bool alreadyExists = await _context.FavoriteDishes
                .AnyAsync(fd => fd.UserId == user.Id && fd.DishId == favoriteDish.DishId);

            if (alreadyExists)
                return Conflict("Ця страва вже додана до улюблених.");

            favoriteDish.UserId = user.Id;
            _context.FavoriteDishes.Add(favoriteDish);
            await _context.SaveChangesAsync();

            return Ok(favoriteDish);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var favorite = await _context.FavoriteDishes.FindAsync(id);
            if (favorite == null) return NotFound();

            _context.FavoriteDishes.Remove(favorite);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
