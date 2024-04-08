using System.ComponentModel.DataAnnotations;
using HealthCare.Core.Entities;

namespace HealthCare.PL.Controllers
{
    public class PatientRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string HardwareId { get; set; }

        [Required]
        public char Gender { get; set; }

        [Required]
        public DateOnly BOD { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

