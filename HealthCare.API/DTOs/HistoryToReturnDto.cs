using HealthCare.Core.Entities.Data;

namespace HealthCare.PL.DTOs
{
    public class HistoryToReturnDto
    {
        public decimal HeartRate { get; set; }
        public decimal Temperature { get; set; }
        public decimal Oxygen { get; set; }
        public string ECG { get; set; }
        public DateTimeOffset MeasureDate { get; set; }
    }
}
