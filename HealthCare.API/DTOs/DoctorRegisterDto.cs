using HealthCare.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.PL.DTOs
{
    public class DoctorRegisterDto
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }

   
    }


}

