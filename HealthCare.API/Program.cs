
using HealthCare.API.Extensions;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Data;
using HealthCare.Repository.Data.DataSeed;
using HealthCare.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace HealthCare.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Service Configurations

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<HealthCareContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            builder.Services.AddApplicationService();


            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion


            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var service = scope.ServiceProvider;

            var LoggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = service.GetRequiredService<HealthCareContext>();

                await DbContext.Database.MigrateAsync();

                var userManager = service.GetRequiredService<UserManager<AppUser>>();

                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

                await RolesSeed.RolesSeedAsync(roleManager);

                await DataStoreSeed.SeedAsync(DbContext, userManager);

                await IdentityStoreSeed.SeedUserAsync(userManager, DbContext);

                await DataStoreSeed.SeedAsync(DbContext, userManager);

            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error during update Database");
            }

            // Configure the HTTP request pipeline.
            #region Middle Wares

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run(); 
            #endregion
        }
    }
}
