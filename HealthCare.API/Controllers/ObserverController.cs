using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using HealthCare.Core.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.Core.Entities;
using HealthCare.PL.DTOs;

namespace HealthCare.PL.Controllers
{
    public class ObserverController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        public ObserverController(UserManager<AppUser> userManager, ITokenService token)
        {
            _userManager = userManager;
            _token = token;    
        }

        [HttpPost("ObserverRegister")]
        public async Task<ActionResult<UserToReturnDto>> Register(ObserverRegisterDto Observers)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(Observers.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "Observer with this email Already Exists!" });
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Observers.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "Observer with this phone number Already Exists!" });
            }

            var Observer = new Observer()
            {
                UserName = Observers.Email.Split('@')[0],
                Email = Observers.Email,
                PhoneNumber = Observers.PhoneNumber,
                FirstName = Observers.FirstName,
                LastName = Observers.LastName,
                Address = Observers.Address,
                PatientObserverId = Observers.PatientObserverId,
            };

            var Result = await _userManager.CreateAsync(Observer,Observers.password);

            if (Result.Succeeded)
            {
                await _userManager.AddToRoleAsync(Observer, "Observer");

                var ObserverDto = new UserToReturnDto()
                {
                    UserName = Observers.Email.Split('@')[0],
                    Email = Observers.Email,
                    Role = "Observer",
                    Token = await _token.CreateTokenAsync(Observer)
                };
                return Ok(ObserverDto);
            }
            else
                return BadRequest(Result.Errors);
        }
    }
}