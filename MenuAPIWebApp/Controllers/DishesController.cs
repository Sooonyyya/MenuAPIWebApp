using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly MenuContext _context;

        public DishesController(MenuContext context)
        {
            _context = context;
        }

        // Відкрити меню
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAll()
        {
            return await _context.Dishes
                .Include(d => d.DishType)
                .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                .ToListAsync();
        }

        // Переглянути детальну інформацію про страву
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> Get(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishType)
                .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null) return NotFound();
            return dish;
        }

        // Відфільтрувати меню
        [HttpGet("Filter")]
        public async Task<ActionResult<IEnumerable<Dish>>> Filter([FromQuery] string? name, [FromQuery] int? typeId, [FromQuery] decimal? price)
        {
            var query = _context.Dishes.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));
            if (typeId.HasValue)
                query = query.Where(d => d.DishTypeId == typeId);
            if (price.HasValue)
                query = query.Where(d => d.Price <= price);

            return await query
                .Include(d => d.DishType)
                .ToListAsync();
        }

        // Порівняння за ціною
        [HttpGet("CompareByPrice")]
        public async Task<ActionResult<IEnumerable<Dish>>> CompareByPrice()
        {
            return await _context.Dishes
                .OrderBy(d => d.Price)
                .ToListAsync();
        }

        // Порівняння за калорійністю
        [HttpGet("CompareByCalories")]
        public async Task<ActionResult<IEnumerable<Dish>>> CompareByCalories()
        {
            return await _context.Dishes
                .OrderBy(d => d.Calories)
                .ToListAsync();
        }

        // Порівняння за інгредієнтами (всі задані мають бути в страві)
        [HttpPost("CompareByIngredients")]
        public ActionResult<IEnumerable<Dish>> CompareByIngredients([FromBody] List<int> ingredientIds)
        {
            var dishes = _context.Dishes
                .Include(d => d.DishIngredients)
                .Where(d => ingredientIds.All(id => d.DishIngredients.Any(di => di.IngredientId == id)))
                .ToList();

            return dishes;
        }

        // Додати нову страву (адмін)
        [HttpPost]
        public async Task<ActionResult<Dish>> Post(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dish.Id }, dish);
        }

        // Змінити страву (адмін)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Dish dish)
        {
            if (id != dish.Id) return BadRequest();
            _context.Entry(dish).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Видалити страву (адмін)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Додати в обране
        [HttpPost("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDish favorite)
        {
            _context.FavoriteDishes.Add(favorite);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Залишити відгук
        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Отримати всі відгуки по страві
        [HttpGet("{dishId}/reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForDish(int dishId)
        {
            return await _context.Reviews
                .Where(r => r.DishId == dishId)
                .ToListAsync();
        }
    }
}
