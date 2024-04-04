using HealthCare.Core.Entities;

namespace HealthCare.PL.Controllers
{
	public class PatientDto
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HardwareId { get; set; }
        public char Gender { get; set; }
        public DateOnly BOD { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}

