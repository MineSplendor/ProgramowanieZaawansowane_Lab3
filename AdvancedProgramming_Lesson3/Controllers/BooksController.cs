using AdvancedProgramming_Lesson3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedProgramming_Lesson3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await _context.Books
                .Select(x => BookToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return BookToDTO(book);
        }

        [HttpPost]
        [Route("UpdateBook")]
        public async Task<ActionResult<BookDTO>> UpdateBook(BookDTO bookDTO)
        {
            var book = await _context.Books.FindAsync(bookDTO.Id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = bookDTO.Title;
            book.Author = bookDTO.Author;
            book.Genre = bookDTO.Genre;
            book.ISBN = bookDTO.ISBN;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookExists(bookDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateBook),
                new { id = book.Id },
                BookToDTO(book));
        }

        [HttpPost]
        [Route("CreateBook")]
        public async Task<ActionResult<BookDTO>> CreateBook(BookDTO bookDTO)
        {
            var book = new Book
            {
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                Genre = bookDTO.Genre,
                ISBN = bookDTO.ISBN
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { id = book.Id },
                BookToDTO(book));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool BookExists(long id) =>
            _context.Books.Any(e => e.Id == id);

        private static BookDTO BookToDTO(Book book) =>
            new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                ISBN = book.ISBN
            };
    }
}
