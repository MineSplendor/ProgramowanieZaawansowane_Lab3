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
    public class CarsController : ControllerBase
    {
        private readonly CarContext _context;

        public CarsController(CarContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCars()
        {
            return await _context.Cars
                .Select(x => CarToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTO>> GetCar(long id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return CarToDTO(car);
        }

        [HttpPost]
        [Route("UpdateCar")]
        public async Task<ActionResult<CarDTO>> UpdateCar(CarDTO carDTO)
        {
            var car = await _context.Cars.FindAsync(carDTO.Id);
            if (car == null)
            {
                return NotFound();
            }
            car.Make = carDTO.Make;
            car.Model = carDTO.Model;
            car.Year = carDTO.Year;
            car.Color = carDTO.Color;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CarExists(carDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateCar),
                new { id = car.Id },
                CarToDTO(car));
        }

        [HttpPost]
        [Route("CreateCar")]
        public async Task<ActionResult<CarDTO>> CreateCar(CarDTO carDTO)
        {
            var car = new Car
            {
                Make = carDTO.Make,
                Model = carDTO.Model,
                Year = carDTO.Year,
                Color = carDTO.Color
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCar),
                new { id = car.Id },
                CarToDTO(car));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(long id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool CarExists(long id) =>
            _context.Cars.Any(e => e.Id == id);

        private static CarDTO CarToDTO(Car car) =>
            new CarDTO
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                Color = car.Color
            };
    }
}
