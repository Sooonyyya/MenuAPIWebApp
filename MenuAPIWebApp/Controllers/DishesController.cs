// DishesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;
using MenuAPIWebApp.Models.DTO;

[Route("api/[controller]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly MenuContext _context;

    public DishesController(MenuContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> Get() =>
        await _context.Dishes
            .Include(d => d.DishType)
            .Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
            .Include(d => d.Reviews)
            .ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Dish>> Get(int id)
    {
        var dish = await _context.Dishes
            .Include(d => d.DishType)
            .Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
            .Include(d => d.Reviews)
            .FirstOrDefaultAsync(d => d.Id == id);

        return dish == null ? NotFound() : dish;
    }

    [HttpPost]
    public async Task<ActionResult<Dish>> Post(DishDTO dto)
    {
        var dish = new Dish
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Calories = dto.Calories,
            DishTypeId = dto.DishTypeId
        };

        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();

        foreach (var ingId in dto.IngredientIds)
        {
            _context.DishIngredients.Add(new DishIngredient
            {
                DishId = dish.Id,
                IngredientId = ingId
            });
        }

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dish.Id }, dish);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, DishDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var dish = await _context.Dishes
            .Include(d => d.DishIngredients)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dish == null)
            return NotFound();

        // Оновлюємо основні поля
        dish.Name = dto.Name;
        dish.Description = dto.Description;
        dish.Price = dto.Price;
        dish.Calories = dto.Calories;
        dish.DishTypeId = dto.DishTypeId;

        // Видаляємо попередні зв'язки з інгредієнтами
        _context.DishIngredients.RemoveRange(dish.DishIngredients);
        await _context.SaveChangesAsync();

        // Додаємо нові зв’язки
        foreach (var ingId in dto.IngredientIds)
        {
            var newIngredient = new DishIngredient
            {
                DishId = dish.Id,
                IngredientId = ingId
            };
            _context.Entry(newIngredient).State = EntityState.Added;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null) return NotFound();
        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        return NoContent();
    }



    // Пошук за частиною назви
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Dish>>> SearchByName([FromQuery] string query)
    {
        return await _context.Dishes
            .Where(d => d.Name.Contains(query))
            .Include(d => d.DishType)
            .ToListAsync();
    }

    // Сортування за ціною
    [HttpGet("sorted")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetSorted([FromQuery] bool desc = false)
    {
        var dishes = _context.Dishes.Include(d => d.DishType).AsQueryable();
        dishes = desc ? dishes.OrderByDescending(d => d.Price) : dishes.OrderBy(d => d.Price);
        return await dishes.ToListAsync();
    }

    // Порівняння за калорійністю
    [HttpPost("compare/calories")]
    public async Task<ActionResult<IEnumerable<Dish>>> CompareByCalories([FromBody] List<int> ids)
    {
        return await _context.Dishes
            .Where(d => ids.Contains(d.Id))
            .OrderByDescending(d => d.Calories)
            .ToListAsync();
    }

    // Порівняння за ціною
    [HttpPost("compare/price")]
    public async Task<ActionResult<IEnumerable<Dish>>> CompareByPrice([FromBody] List<int> ids)
    {
        return await _context.Dishes
            .Where(d => ids.Contains(d.Id))
            .OrderBy(d => d.Price)
            .ToListAsync();
    }

    // Порівняння за інгредієнтами
    [HttpPost("compare/ingredients")]
    public async Task<ActionResult<IEnumerable<object>>> CompareByIngredients([FromBody] List<int> ids)
    {
        var dishes = await _context.Dishes
            .Where(d => ids.Contains(d.Id))
            .Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
            .ToListAsync();

        return dishes.Select(d => new
        {
            d.Id,
            d.Name,
            IngredientCount = d.DishIngredients.Count,
            Ingredients = d.DishIngredients.Select(i => i.Ingredient.Name)
        }).ToList();
    }

    // Фільтрація по назві, типу, ціні
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Dish>>> Filter(
        [FromQuery] string? name,
        [FromQuery] int? typeId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var query = _context.Dishes
            .Include(d => d.DishType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(d => d.Name.Contains(name));

        if (typeId.HasValue)
            query = query.Where(d => d.DishTypeId == typeId);

        if (minPrice.HasValue)
            query = query.Where(d => d.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(d => d.Price <= maxPrice);

        return await query.ToListAsync();
    }
}