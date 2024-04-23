using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using HealthCare.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HealthCare.PL.Controllers
{

    public class DoctorController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
       

        public DoctorController(UserManager<AppUser> userManager, ITokenService token)
        {
            _userManager = userManager;
            _token = token;
           

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserToReturnDto>> Register(DoctorRegisterDto model )
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "Observer with this email Already Exists!" });
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "Observer with this phone number Already Exists!" });
            }

            var Doctor = new Doctor()
            {
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
            };

            var Result = await _userManager.CreateAsync(Doctor, model.Password);
            if (Result.Succeeded)
            {
                // Assign the Doctor role to the newly registered user
                await _userManager.AddToRoleAsync(Doctor, "Doctor");

                var doctordto = new UserToReturnDto()
                {
                    UserName = Doctor.UserName,
                    Email = model.Email,
                    Role = "Doctor",
                    Token = await _token.CreateTokenAsync(Doctor),
                };
                return (doctordto);
            }
            BadRequestResult badRequestResult = BadRequest();
            return Ok(badRequestResult);

        }
        
    }
}
