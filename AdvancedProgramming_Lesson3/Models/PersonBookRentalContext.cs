using Microsoft.EntityFrameworkCore;

namespace AdvancedProgramming_Lesson3.Models
{
    public class PersonBookRentalContext : DbContext
    {

        public PersonBookRentalContext(DbContextOptions<PersonBookRentalContext> options)
            : base(options)
        {
        }

        public DbSet<PersonBookRental> PersonBookRentals { get; set; }
    }
}
