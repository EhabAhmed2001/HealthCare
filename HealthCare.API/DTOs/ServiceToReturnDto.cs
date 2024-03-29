using HealthCare.Core.Entities;
using System.Text.Json.Serialization;

namespace HealthCare.PL.DTOs
{
    public class ServiceToReturnDto
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Hotline { get; set; }
        public char Type { get; set; }
        public string Location { get; set; }
    }
}
