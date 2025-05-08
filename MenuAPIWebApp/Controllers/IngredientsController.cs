using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly MenuContext _context;

        public IngredientsController(MenuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetAll()
        {
            return await _context.Ingredients.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return Ok(ingredient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Ingredients.FindAsync(id);
            if (item == null) return NotFound();
            _context.Ingredients.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
