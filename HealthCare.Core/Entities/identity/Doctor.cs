using HealthCare.Core.Entities.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities
{
    public class Doctor : AppUser
    {
        public string Specialist { get; set; } = "General";

        // Patient Relation
        public ICollection<Patient>? Patients { get; set; } = new HashSet<Patient>();

        // History Relation
        public ICollection<History>? History { get; set; } = new HashSet<History>();
    }
}
