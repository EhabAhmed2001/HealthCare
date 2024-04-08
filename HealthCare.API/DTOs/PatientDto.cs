using HealthCare.Core.Entities;

namespace HealthCare.PL.Controllers
{
    public class PatientDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}