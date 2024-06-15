using HealthCare.Core.AddRequest;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.identity
{
    [NotMapped]
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DeviceToken { get; set; }
        public string PictureUrl { get; set; } = "Images/defaultImg.png";
        public Address Address { get; set; }
    }
}
