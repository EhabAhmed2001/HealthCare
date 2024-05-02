using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public class PatientToReturnDto :UserDto
    {
        public char Gender { get; set; }

        public DateOnly BOD { get; set; }

        public string? BloodType { get; set; }

        public string? DoctorId { get; set; }

    }
}
