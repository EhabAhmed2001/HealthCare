namespace HealthCare.Core.Entities
{
    public class Address
    {
        public string Street { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Country { get; set; } = "Egypt";
    }
}