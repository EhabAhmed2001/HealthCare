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
        public int HearRate { get; set; }
        public int Temperature { get; set; }
        public int Oxygen { get; set; }
        public string ECG { get; set; }

    }
}
