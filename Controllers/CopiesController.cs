using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("copies")]
    [ApiController]
    public class CopiesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public CopiesController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyDto>>> GetCopies()
        {
            var copies = await _context.Copies.ToListAsync();
            return Ok(copies.Select(c => new CopyDto { Id = c.Id, BookId = c.BookId }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CopyDto>> GetCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();
            return new CopyDto { Id = copy.Id, BookId = copy.BookId };
        }

        [HttpPost]
        public async Task<ActionResult<CopyDto>> PostCopy(CreateCopyDto dto)
        {
            if (!_context.Books.Any(b => b.Id == dto.BookId)) return BadRequest("Ksi¹¿ka nie istnieje");

            var copy = new Copy { BookId = dto.BookId };
            _context.Copies.Add(copy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCopy), new { id = copy.Id }, new CopyDto { Id = copy.Id, BookId = copy.BookId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();
            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}