using HealthCare.Core;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.Repository;
using HealthCare.Repository.Identity;
using HealthCare.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HealthCare.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration _configuration)
        {

            Services.AddScoped<ITokenService, TokenService>();

            Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<HelthCareIdentityContext>();

            Services.AddAuthentication(Option =>
            {
                Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // Default schema for all end-points is bearer
                Option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;     // No end-points will called without bearer schema

            })
                .AddJwtBearer(option =>     // validation bearer schema to handle it (it's not handled)
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),

                    };
                });

            return Services;
        }
    }
}
