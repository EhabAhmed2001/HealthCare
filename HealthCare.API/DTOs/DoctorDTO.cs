using System.ComponentModel.DataAnnotations;

namespace HealthCare.PL.DTOs
{
    public class DoctorDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }

        public string Role { get; set; }
        public string Token { get; set; }


    }
}
