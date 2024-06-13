using AutoMapper;
using HealthCare.Core.Entities.identity;
using HealthCare.PL.DTOs;

namespace HealthCare.PL.Helper
{
    public class PictureResolver : IValueResolver<AppUser, UserSearchToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public PictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(AppUser source, UserSearchToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
    
}
