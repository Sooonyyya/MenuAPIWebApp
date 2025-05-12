using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class FavoriteDishesController : ControllerBase
{
    private readonly MenuContext _context;

    public FavoriteDishesController(MenuContext context) => _context = context;

    [HttpGet("{sessionId}")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetFavorites(string sessionId)
    {
        var favorites = await _context.FavoriteDishes
            .Where(f => f.UserSessionId == sessionId)
            .Select(f => f.DishId)
            .ToListAsync();

        var dishes = await _context.Dishes
            .Where(d => favorites.Contains(d.Id))
            .Include(d => d.DishType)
            .Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
            .Include(d => d.Reviews)
            .ToListAsync();

        return dishes;
    }


    [HttpPost]
    public async Task<IActionResult> AddFavorite(FavoriteDishDTO fav)
    {
        var exists = await _context.FavoriteDishes
            .AnyAsync(f => f.UserSessionId == fav.UserSessionId && f.DishId == fav.DishId);

        if (!exists)
        {
            _context.FavoriteDishes.Add(new FavoriteDish
            {
                DishId = fav.DishId,
                UserSessionId = fav.UserSessionId
            });
            await _context.SaveChangesAsync();
        }

        return Ok();
    }


    [HttpDelete("{sessionId}/{dishId}")]
    public async Task<IActionResult> RemoveFavorite(string sessionId, int dishId)
    {
        var fav = await _context.FavoriteDishes
            .FirstOrDefaultAsync(f => f.UserSessionId == sessionId && f.DishId == dishId);

        if (fav == null) return NotFound();

        _context.FavoriteDishes.Remove(fav);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
