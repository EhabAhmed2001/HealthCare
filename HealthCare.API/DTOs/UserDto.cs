using HealthCare.Core.Entities;

namespace HealthCare.PL.DTOs
{
    public abstract class UserDto : UserSearchToReturnDto
    {
        public Address Address { get; set; }

        public string PhoneNumber { get; set; }

    }
}
