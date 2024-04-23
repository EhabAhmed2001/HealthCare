using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Entities.identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HealthCare.Repository.Identity
{
    public static class IdentityStoreSeed
    {
        private static string[] roles = { "Patient", "Doctor", "Observer" };

        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var patientJson = File.ReadAllText("../HealthCare.Repository/Data/DataSeed/Patient.json");
                var patient = JsonConvert.DeserializeObject<AppUser>(patientJson);

                var doctorJson = File.ReadAllText("../HealthCare.Repository/Data/DataSeed/Doctor.json");
                var doctor = JsonConvert.DeserializeObject<AppUser>(doctorJson);

                var observerJson = File.ReadAllText("../HealthCare.Repository/Data/DataSeed/Observer.json");
                var observer = JsonConvert.DeserializeObject<AppUser>(observerJson);

                var users = new List<AppUser> { patient, doctor, observer };

                foreach (var user in users)
                {
                    user.Address = new HealthCare.Core.Entities.Address
                    {
                        Street = user.Address.Street,
                        Region = user.Address.Region,
                        City = user.Address.City,
                        Country = user.Address.Country
                    };

                    var createUserResult = await userManager.CreateAsync(user, "P@ssw0rd");
                    if (createUserResult.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, roles[0]);
                        if (roleResult.Succeeded)
                        {
                            Console.WriteLine($"Created user {user.UserName} with role {roles[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to assign role to user {user.UserName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user {user.UserName}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }
}