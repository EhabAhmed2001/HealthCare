namespace HealthCare.PL.DTOs
{
    public class HistoryDto
    {

        public string HardwareId { get; set; }
        public decimal HeartRate { get; set; }
        public decimal Temperature { get; set; }
        public decimal Oxygen { get; set; }
        public string ECG { get; set; }
    }
}
