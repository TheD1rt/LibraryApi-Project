using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BooksController(LibraryContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] int? authorId)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();
            if (authorId.HasValue) query = query.Where(b => b.AuthorId == authorId.Value);
            var books = await query.ToListAsync();
            return Ok(books.Select(b => MapToDto(b)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(string id)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var b = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(x => x.Id == intId);
            if (b == null) return NotFound();
            return Ok(MapToDto(b));
        }

        [HttpPost]
        public async Task<IActionResult> PostBook(CreateBookDto dto)
        {
            if (!_context.Authors.Any(a => a.Id == dto.AuthorId)) return BadRequest();
            var book = new Book { Title = dto.Title, Year = dto.Year, AuthorId = dto.AuthorId };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var b = await _context.Books.Include(x => x.Author).FirstAsync(x => x.Id == book.Id);
            return CreatedAtAction(nameof(GetBook), new { id = b.Id }, MapToDto(b));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(string id, CreateBookDto dto)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var book = await _context.Books.FindAsync(intId);
            if (book == null) return NotFound();
            if (!_context.Authors.Any(a => a.Id == dto.AuthorId)) return BadRequest();
            book.Title = dto.Title;
            book.Year = dto.Year;
            book.AuthorId = dto.AuthorId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (!int.TryParse(id, out int intId)) return NotFound();
            var book = await _context.Books.FindAsync(intId);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private object MapToDto(Book b) => new
        {
            id = b.Id,
            title = b.Title,
            year = b.Year,
            author = b.Author == null ? null : new
            {
                id = b.Author.Id,
                first_name = b.Author.FirstName,
                last_name = b.Author.LastName
            }
        };
    }
}