using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public abstract class UserDto : UserSearchToReturnDto
    {
        public string Email { get; set; }

        public Address Address { get; set; }

        public string PhoneNumber { get; set; }

    }
}
