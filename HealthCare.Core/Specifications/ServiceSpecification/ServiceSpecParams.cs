using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Specifications.ServiceSpecification
{
    public class ServiceSpecParams
    {
        public string? sort { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }
        public string? street { get; set; }
        public string? region { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }

        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }

        public int index { get; set; } = 1;
    }
}
