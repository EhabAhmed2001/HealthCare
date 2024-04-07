using HealthCare.Core.Entities.identity;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.PL.Helper
{
    public class UserHelper
    {
        public async Task<string?> UserSearch(string Email, string Role, UserManager<AppUser> userManager)
        {
            
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return null;
            }

            // Check if the user has the given role
           else if (await userManager.IsInRoleAsync(user, Role))
            {
                return user.Id;
            }
            
          // If the user does not have the given role
            return null;
            
        }
    }
}
