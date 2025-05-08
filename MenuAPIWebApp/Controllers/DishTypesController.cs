using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishTypesController : ControllerBase
    {
        private readonly MenuContext _context;

        public DishTypesController(MenuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishType>>> GetAll()
        {
            return await _context.DishTypes.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DishType type)
        {
            _context.DishTypes.Add(type);
            await _context.SaveChangesAsync();
            return Ok(type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DishType type)
        {
            if (id != type.Id) return BadRequest();
            _context.Entry(type).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.DishTypes.FindAsync(id);
            if (item == null) return NotFound();
            _context.DishTypes.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
