using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.Data
{
    public class SensorsData
    {
        public decimal HeartRate { get; set; }
        public decimal Temperature { get; set; }
        public decimal Oxygen { get; set; }
        public string ECG { get; set; }

    }
}
