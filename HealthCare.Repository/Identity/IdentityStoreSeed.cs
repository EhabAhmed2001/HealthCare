using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Identity;

namespace HealthCare.Repository.Identity
{
    public static class IdentityStoreSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager, HealthCareContext dbContext)
        {
            if (!userManager.Users.Any())
            {
                var doctorJson = File.ReadAllText("../HealthCare.Repository/Identity/DataSeed/Doctor.json");
                var doctor = JsonSerializer.Deserialize<List<Doctor>>(doctorJson);
                await TransferData(doctor, "Doctor", userManager);

                var patientJson = File.ReadAllText("../HealthCare.Repository/Identity/DataSeed/Patient.json");
                var patient = JsonSerializer.Deserialize<List<Patient>>(patientJson);
                await TransferData(patient, "Patient", userManager, dbContext);

                var observerJson = File.ReadAllText("../HealthCare.Repository/Identity/DataSeed/Observer.json");
                var observer = JsonSerializer.Deserialize<List<Observer>>(observerJson);
                await TransferData(observer, "Observer", userManager);
            }
        }

        private static async Task TransferData<T> (List<T> users, string Role, UserManager<AppUser> userManager, HealthCareContext? dbContext = null) where T : AppUser
        {
            foreach (var user in users)
            {
                if (Role == "Patient")
                {
                    var patient = user as Patient;
                    var Hardware = dbContext!.Hardware.Where(HW => HW.Id == patient!.HardwareId).FirstOrDefault()!;
                    Hardware.IsUsed = true;
                    await dbContext.SaveChangesAsync();
                }

                var createUserResult = await userManager.CreateAsync(user, "P@ssw0rd");
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role);
                }
            }
        }
    }
}