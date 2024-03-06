
using HealthCare.API.Extensions;
using HealthCare.Repository.Data;
using HealthCare.Repository.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.API
{
    public class Program
    {
        public static void Main(string[] args)
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

            builder.Services.AddDbContext<HelthCareIdentityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppIdentityConnection"));
            });

            builder.Services.AddApplicationService();


            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion


            var app = builder.Build();

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
