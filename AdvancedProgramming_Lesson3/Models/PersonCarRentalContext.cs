using Microsoft.EntityFrameworkCore;

namespace AdvancedProgramming_Lesson3.Models
{
    public class PersonCarRentalContext : DbContext
    {

        public PersonCarRentalContext(DbContextOptions<PersonCarRentalContext> options)
            : base(options)
        {
        }

        public DbSet<PersonCarRental> PersonCarRentals { get; set; }
    }
}
