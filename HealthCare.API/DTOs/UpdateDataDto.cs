using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public class UpdateDataDto
    {
        public IFormFile? image { get; set; }
        public Address? address { get; set; }
    }
}
