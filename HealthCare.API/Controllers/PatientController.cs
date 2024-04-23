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

namespace HealthCare.PL.Controllers
{
    public class PatientController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<PatientController> logger;

        public PatientController(UserManager<AppUser> userManager, ITokenService token, RoleManager<IdentityRole> roleManager, ILogger<PatientController> logger)
        {
            _userManager = userManager;
            _token = token;
            _roleManager = roleManager;
            this.logger = logger;
        }
        /*
                [HttpPost("PatientRegister")]
                public async Task<ActionResult<PatientDto>> Register(PatientRegisterDto Patients)
                {

                    var existingUserByEmail = await _userManager.FindByEmailAsync(Patients.Email);

                    if (existingUserByEmail != null)
                    {
                        logger.LogError("Patient with this email Already Exists!");
                        return BadRequest(new { message = "Patient with this email Already Exists!" });
                    }

                    var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Patients.PhoneNumber);

                    if (existingUserByPhoneNumber != null)
                    {
                        logger.LogError("Patient with this phone number Already Exists!");
                        return BadRequest(new { message = "Patient with this phone number Already Exists!" });
                    }

                    var Patient = new Patient()
                    {
                        UserName = Patients.Email.Split('@')[0],
                        FirstName = Patients.FirstName,
                        LastName = Patients.LastName,
                        HardwareId = Patients.HardwareId,
                        Gender = Patients.Gender,
                        BOD = Patients.BOD,
                        PhoneNumber = Patients.PhoneNumber,
                        Address = Patients.Address,
                        Email = Patients.Email,
                    };

                    var Result = await _userManager.CreateAsync(Patient, Patients.Password!);

                    if (Result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(Patient, "Patient");

                        var PatientDto = new PatientDto()
                        {
                            UserName = Patients.UserName,
                            Email = Patients.Email,
                            Role = "Patient",
                            Token = await _token.CreateTokenAsync(Patient)
                        };
                        return Ok(PatientDto);
                    }
                    else
                    {
                        return BadRequest(Result.Errors);
                    }
    }*/
    }
}