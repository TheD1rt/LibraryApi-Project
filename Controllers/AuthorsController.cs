using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;
        public AuthorsController(LibraryContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors.Select(a => new {
                id = a.Id,
                first_name = a.FirstName,
                last_name = a.LastName
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(string id)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var a = await _context.Authors.FindAsync(intId);
            if (a == null) return NotFound();
            return Ok(new { id = a.Id, first_name = a.FirstName, last_name = a.LastName });
        }

        [HttpPost]
        public async Task<IActionResult> PostAuthor(CreateAuthorDto dto)
        {
            var author = new Author { FirstName = dto.FirstName, LastName = dto.LastName };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, new { id = author.Id, first_name = author.FirstName, last_name = author.LastName });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(string id, CreateAuthorDto dto)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var author = await _context.Authors.FindAsync(intId);
            if (author == null) return NotFound();
            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(string id)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var author = await _context.Authors.FindAsync(intId);
            if (author == null) return NotFound();
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}