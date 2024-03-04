using HealthCare.Core;

namespace HealthCare.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(IUnitOfWork));

            return services;
        }
    }
}
