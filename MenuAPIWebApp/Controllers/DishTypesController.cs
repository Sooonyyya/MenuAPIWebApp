using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DishTypesController : ControllerBase
{
    private readonly MenuContext _context;

    public DishTypesController(MenuContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishType>>> Get() =>
        await _context.DishTypes.ToListAsync();
}
