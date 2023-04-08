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
    public class PersonCarRentalsController : ControllerBase
    {
        private readonly PersonCarRentalContext _context;

        public PersonCarRentalsController(PersonCarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonCarRentalDTO>>> GetPersonCarRentals()
        {
            return await _context.PersonCarRentals
                .Select(x => PersonCarRentalToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonCarRentalDTO>> GetPersonCarRental(int id)
        {
            var personcarrental = await _context.PersonCarRentals.FindAsync(id);
            if (personcarrental == null)
            {
                return NotFound();
            }

            return PersonCarRentalToDTO(personcarrental);
        }

        [HttpPost]
        [Route("UpdatePersonCarRental")]
        public async Task<ActionResult<PersonCarRentalDTO>> UpdatePersonCarRental(PersonCarRentalDTO personcarrentalDTO)
        {
            var personcarrental = await _context.PersonCarRentals.FindAsync(personcarrentalDTO.Id);
            if (personcarrental == null)
            {
                return NotFound();
            }
            personcarrental.PersonID = personcarrentalDTO.PersonID;
            personcarrental.CarID = personcarrentalDTO.CarID;
            personcarrental.RentalDate = personcarrentalDTO.RentalDate;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonCarRentalExists(personcarrentalDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdatePersonCarRental),
                new { id = personcarrental.Id },
                PersonCarRentalToDTO(personcarrental));
        }

        [HttpPost]
        [Route("CreatePersonCarRental")]
        public async Task<ActionResult<PersonCarRentalDTO>> CreatePersonCarRental(PersonCarRentalDTO personcarrentalDTO)
        {
            var personcarrental = new PersonCarRental
            {
                PersonID = personcarrentalDTO.PersonID,
                CarID = personcarrentalDTO.CarID,
                RentalDate = personcarrentalDTO.RentalDate
            };

            _context.PersonCarRentals.Add(personcarrental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPersonCarRental),
                new { id = personcarrental.Id },
                PersonCarRentalToDTO(personcarrental));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonCarRental>> DeletePersonCarRental(int id)
        {
            var personcarrental = await _context.PersonCarRentals.FindAsync(id);
            if (personcarrental == null)
            {
                return NotFound();
            }
            _context.PersonCarRentals.Remove(personcarrental);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool PersonCarRentalExists(int id) =>
            _context.PersonCarRentals.Any(e => e.Id == id);

        private static PersonCarRentalDTO PersonCarRentalToDTO(PersonCarRental personcarrental) =>
            new PersonCarRentalDTO
            {
                Id = personcarrental.Id,
                PersonID = personcarrental.PersonID,
                CarID = personcarrental.CarID,
                RentalDate = personcarrental.RentalDate
            };
    }
}
