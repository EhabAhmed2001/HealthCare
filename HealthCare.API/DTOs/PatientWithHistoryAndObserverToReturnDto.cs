namespace HealthCare.PL.DTOs
{
    public class PatientWithHistoryAndObserverToReturnDto : UserDto
    {
        public char Gender { get; set; }
        public DateOnly BOD { get; set; }
        public string? BloodType { get; set; }
        public ObserverToReturnDto? Observer { get; set; }
        public ICollection<HistoryToReturnDto>? History { get; set; }
    }
}
