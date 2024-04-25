using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Security.Claims;


namespace HealthCare.PL.Controllers
{
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager <AppUser>_signInManager;
        private readonly ITokenService token;
        
      public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService Token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
             token = Token;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToReturnDto>> login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new { message = "Email Or Password Is Not Correct" });

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.password, false);
            if (!Result.Succeeded) return Unauthorized(new { message = "Email or password is not correct" });

            var role = await _userManager.GetRolesAsync(User);
            return Ok(new UserToReturnDto()
            {
                UserName = User.UserName,
                Email = User.Email,
                Token = await token.CreateTokenAsync(User),
                Role = role[0]
            });

        }


        // Get Current User

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email is null)
            {
                return BadRequest(new {message= "There is no active users."});
            }
            var user = await _userManager.FindByEmailAsync(Email);


            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Patient"))
            {
                var CurrentPatient = (Patient)user;
                var PatientToReturn = new CurrentPatientDto()
                {
                    Email = CurrentPatient.Email,
                    UserName = CurrentPatient.UserName,
                    FisrtName = CurrentPatient.FirstName,
                    LastName = CurrentPatient.LastName,
                    Address = CurrentPatient.Address,
                    PhoneNamber = CurrentPatient.PhoneNumber,
                    Gender = CurrentPatient.Gender,
                    BOD = CurrentPatient.BOD,
                    PictureUrl = CurrentPatient.PictureUrl,

                };

                return Ok(PatientToReturn);
            }
            else if (role.Contains("Doctor"))
            {
                var CurrentDoctor = (Doctor)user;
                var DoctorToReturn = new CurrentDoctorDto()
                {
                    Email = CurrentDoctor.Email,
                    UserName = CurrentDoctor.UserName,
                    FisrtName = CurrentDoctor.FirstName,
                    LastName = CurrentDoctor.LastName,
                    Address = CurrentDoctor.Address,
                    PhoneNamber = CurrentDoctor.PhoneNumber,
                    PictureUrl = CurrentDoctor.PictureUrl,
                };
                return Ok(DoctorToReturn);

            }
            else
            {
                var CurrentObserver = (Observer)user;
                var ObserverToReturn = new CurrentObserverDto()
                {
                    Email = CurrentObserver.Email,
                    UserName = CurrentObserver.UserName,
                    FisrtName = CurrentObserver.FirstName,
                    LastName = CurrentObserver.LastName,
                    Address = CurrentObserver.Address,
                    PhoneNamber = CurrentObserver.PhoneNumber,
                    PictureUrl = CurrentObserver.PictureUrl,
                };
                return Ok(ObserverToReturn);

            }



        }


    }
}
