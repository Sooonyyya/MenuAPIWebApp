using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPIWebApp.Models;

namespace MenuAPIWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly MenuContext _context;

        public UsersController(MenuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpGet("exists/{username}")]
        public async Task<ActionResult<bool>> Exists(string username)
        {
            bool exists = await _context.Users.AnyAsync(u => u.UserName == username);
            return Ok(exists);
        }
    }
}
