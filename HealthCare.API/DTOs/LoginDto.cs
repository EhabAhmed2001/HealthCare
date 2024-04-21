using System.ComponentModel.DataAnnotations;

namespace HealthCare.PL.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string password { get; set; }
        
    }
}
