using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.Data
{
    public class Services : AppEntity
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Hotline { get; set; }
        private string type;
        public string Type
        {
            get
            {
                return $"{type[0]}";
            }
            set
            {
                Type = $"{value.ToString().ToUpper()[0]}";
            }
        }
        public string Location { get; set; }

    }
}
