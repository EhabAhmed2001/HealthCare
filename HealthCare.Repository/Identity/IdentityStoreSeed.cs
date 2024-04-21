using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HealthCare.Core.Entities.identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace HealthCare.Repository.Identity
{
    public static class IdentityStoreSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = File.ReadAllText("./UserData.json");
                var users = JsonConvert.DeserializeObject<List<UserData>>(userData);

                var roleData = File.ReadAllText("./RoleData.json");
                var roles = JsonConvert.DeserializeObject<List<string>>(roleData);

                foreach (var user in users)
                {
                    var appUser = new AppUser
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = new HealthCare.Core.Entities.Address
                        {
                            Street = user.Address.Street,
                            Region = user.Address.Region,
                            City = user.Address.City,
                            Country = user.Address.Country
                        }
                    };

                    var createUserResult = await userManager.CreateAsync(appUser, "P@ssw0rd");
                    if (createUserResult.Succeeded)
                    {
                        // Assign role to user
                        var roleResult = await userManager.AddToRoleAsync(appUser, roles[0]);
                        if (roleResult.Succeeded)
                        {
                            Console.WriteLine($"Created user {appUser.UserName} with role {roles[0]}");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to assign role to user {appUser.UserName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user {appUser.UserName}: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }

    public class UserData
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}