using HealthCare.Core.Entities.Data;
using HealthCare.Core.Entities.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities
{
    public class Patient : AppUser
    {
        public char Gender { get; set; }
        public DateOnly BOD { get; set; }
        public string? BloodType { get; set; }

        public string HardwareId { get; set; }
        public Hardware Hardware { get; set; }

        // Doctor Relation
        public string? PatientDoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        // Observer Relation
        public Observer? PatientObserver { get; set; }

        // History Relation
        public ICollection<History>? History { get; set; } = new HashSet<History>();
    }
}
