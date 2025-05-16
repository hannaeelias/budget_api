using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using budget_api.Models;
using budget_api.Data;

namespace budget_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET all items for a user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByUser(string userId)
        {
            return await _context.Items.Where(i => i.UserId == userId).ToListAsync();
        }

        // POST new item
        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemsByUser), new { userId = item.UserId }, item);
        }

        // PUT update an item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE an item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
