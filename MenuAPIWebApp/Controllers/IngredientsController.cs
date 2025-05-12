using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class IngredientsController : ControllerBase
{
    private readonly MenuContext _context;

    public IngredientsController(MenuContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> Get() =>
        await _context.Ingredients.ToListAsync();
}
