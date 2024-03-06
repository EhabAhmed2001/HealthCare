using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Identity;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration _configuration)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<HelthCareIdentityContext>();

            return Services;
        }
    }
}
