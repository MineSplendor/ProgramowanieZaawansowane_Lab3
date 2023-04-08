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
    public class PeopleController : ControllerBase
    {
        private readonly PersonContext _context;

        public PeopleController(PersonContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople()
        {
            return await _context.People
                .Select(x => PersonToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(long id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            return PersonToDTO(person);
        }

        [HttpPost]
        [Route("UpdatePerson")]
        public async Task<ActionResult<PersonDTO>> UpdatePerson(PersonDTO personDTO)
        {
            var person = await _context.People.FindAsync(personDTO.Id);
            if (person == null)
            {
                return NotFound();
            }
            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;
            person.Age = personDTO.Age;
            person.Address = personDTO.Address;
            person.Email = personDTO.Email;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PersonExists(personDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdatePerson),
                new { id = person.Id },
                PersonToDTO(person));
        }

        [HttpPost]
        [Route("CreatePerson")]
        public async Task<ActionResult<PersonDTO>> CreatePerson(PersonDTO personDTO)
        {
            var person = new Person
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName,
                Age = personDTO.Age,
                Address = personDTO.Address,
                Email = personDTO.Email
            };

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPerson),
                new { id = person.Id },
                PersonToDTO(person));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(long id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool PersonExists(long id) =>
            _context.People.Any(e => e.Id == id);

        private static PersonDTO PersonToDTO(Person person) =>
            new PersonDTO
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                Address = person.Address,
                Email = person.Email
            };
    }
}
