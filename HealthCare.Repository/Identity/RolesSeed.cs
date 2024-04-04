using HealthCare.Core.Entities.Data;
using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository.Identity
{
    public class RolesSeed
    {
        public static async Task RolesSeedAsync(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Doctor"));
                await roleManager.CreateAsync(new IdentityRole("Patient"));
                await roleManager.CreateAsync(new IdentityRole("Observer"));
            }
        }

    }
}
