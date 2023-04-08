using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AdvancedProgramming_Lesson3.Models
{
    public class PersonCarRentalDTO
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public int CarID { get; set; }

        [DataType(DataType.Date)]
        public DateTime RentalDate { get; set; }
    }
}
