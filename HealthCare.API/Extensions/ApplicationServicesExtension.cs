using HealthCare.Core;
using HealthCare.Core.Entities.Data;
using HealthCare.PL.Helper;
using HealthCare.Repository;

namespace HealthCare.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddAutoMapper(typeof(Mapping));


            return services;
        }
    }
}
