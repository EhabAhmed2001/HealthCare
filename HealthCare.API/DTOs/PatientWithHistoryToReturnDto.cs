using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public class PatientWithHistoryToReturnDto : UserDto
    {
        public char Gender { get; set; }
        public DateOnly BOD { get; set; }
        public string? BloodType { get; set; }
        public DoctorToReturnDto? Doctor { get; set; }
        public ICollection<HistoryToReturnDto>? History { get; set; }
    }
}
