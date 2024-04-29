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
        public string PictureUrl { get; set; } = "https://th.bing.com/th/id/OIP.TmFdrhMS6gzhI-ACF3977wHaF2?w=229&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7";
        public Address Address { get; set; }
    }
}
