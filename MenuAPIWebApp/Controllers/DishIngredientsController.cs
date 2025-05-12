using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DishIngredientsController : ControllerBase
{
    private readonly MenuContext _context;

    public DishIngredientsController(MenuContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishIngredient>>> Get() =>
        await _context.DishIngredients
            .Include(di => di.Dish)
            .Include(di => di.Ingredient)
            .ToListAsync();
}
