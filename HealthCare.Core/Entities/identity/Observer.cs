using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.identity
{
    public class Observer : AppUser
    {
        public string PatientObserverId { get; set; }
        public Patient Patient { get; set; }
    }
}
