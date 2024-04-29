using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public class CurrentPatientDto :UserDto
    {
        public char Gender { get; set; }

        public DateOnly BOD { get; set; }

        public string? DoctorId { get; set; }
        public string? BloodType { get; set; }

    }
}
