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
    public class PersonBookRentalsController : ControllerBase
    {
        private readonly PersonBookRentalContext _context;

        public PersonBookRentalsController(PersonBookRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonBookRentalDTO>>> GetPersonBookRentals()
        {
            return await _context.PersonBookRentals
                .Select(x => PersonBookRentalToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonBookRentalDTO>> GetPersonBookRental(int id)
        {
            var personbookrental = await _context.PersonBookRentals.FindAsync(id);
            if (personbookrental == null)
            {
                return NotFound();
            }

            return PersonBookRentalToDTO(personbookrental);
        }

        [HttpPost]
        [Route("UpdatePersonBookRental")]
        public async Task<ActionResult<PersonBookRentalDTO>> UpdatePersonBookRental(PersonBookRentalDTO personbookrentalDTO)
        {
            var personbookrental = await _context.PersonBookRentals.FindAsync(personbookrentalDTO.Id);
            if (personbookrental == null)
            {
                return NotFound();
            }
            personbookrental.PersonID = personbookrentalDTO.PersonID;
            personbookrental.BookID = personbookrentalDTO.BookID;
            personbookrental.RentalDate = personbookrentalDTO.RentalDate;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonBookRentalExists(personbookrentalDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdatePersonBookRental),
                new { id = personbookrental.Id },
                PersonBookRentalToDTO(personbookrental));
        }

        [HttpPost]
        [Route("CreatePersonBookRental")]
        public async Task<ActionResult<PersonBookRentalDTO>> CreatePersonBookRental(PersonBookRentalDTO personbookrentalDTO)
        {
            var personbookrental = new PersonBookRental
            {
                PersonID = personbookrentalDTO.PersonID,
                BookID = personbookrentalDTO.BookID,
                RentalDate = personbookrentalDTO.RentalDate
            };

            _context.PersonBookRentals.Add(personbookrental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPersonBookRental),
                new { id = personbookrental.Id },
                PersonBookRentalToDTO(personbookrental));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonBookRental>> DeletePersonBookRental(int id)
        {
            var personbookrental = await _context.PersonBookRentals.FindAsync(id);
            if (personbookrental == null)
            {
                return NotFound();
            }
            _context.PersonBookRentals.Remove(personbookrental);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool PersonBookRentalExists(int id) =>
            _context.PersonBookRentals.Any(e => e.Id == id);

        private static PersonBookRentalDTO PersonBookRentalToDTO(PersonBookRental personbookrental) =>
            new PersonBookRentalDTO
            {
                Id = personbookrental.Id,
                PersonID = personbookrental.PersonID,
                BookID = personbookrental.BookID,
                RentalDate = personbookrental.RentalDate
            };
    }
}
