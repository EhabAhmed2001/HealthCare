using HealthCare.Core.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities
{
    public class History : AppEntity
    {
        public SensorsData UserData { get; set; }
        public string Actions { get; set; }
        public DateTimeOffset MeasureDate { get; set; } = DateTimeOffset.Now;


        public string HistoryPatientId { get; set; }
        public Patient Patient { get; set; }
        
        public string HistoryDoctorId { get; set; }
        public Doctor Doctor { get; set; }


    }
}
