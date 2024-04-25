using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public class UserDto 
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public string FisrtName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        public string PhoneNamber { get; set; }

        public string PictureUrl { get; set; }
    }
}
