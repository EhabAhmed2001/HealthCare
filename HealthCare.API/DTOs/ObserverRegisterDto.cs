using System.ComponentModel.DataAnnotations;
using HealthCare.Core.Entities;

namespace HealthCare.PL.Controllers
{
    public class ObserverRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PatientObserverId { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string password { get; set; }

    }
}

