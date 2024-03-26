using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.Data
{
    public class Service : AppEntity
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Hotline { get; set; }
        public char Type { get; set; }
        public string Location { get; set; }

    }
}
