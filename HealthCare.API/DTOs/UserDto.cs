using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public abstract class UserDto 
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        public string PhoneNumber { get; set; }

        public string PictureUrl { get; set; }
    }
}
