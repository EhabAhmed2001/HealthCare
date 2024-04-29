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
using System.Security.Claims;
using HealthCare.Repository.Data;

namespace HealthCare.PL.Controllers
{
    public class PatientController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<PatientController> logger;
        private readonly HealthCareContext _dbContext;

        public PatientController(UserManager<AppUser> userManager, ITokenService token, RoleManager<IdentityRole> roleManager, ILogger<PatientController> logger, HealthCareContext dbContext)
        {
            _userManager = userManager;
            _token = token;
            _roleManager = roleManager;
            this.logger = logger;
            _dbContext = dbContext;
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


        [HttpGet("GetDoctorData")]
        public ActionResult<UserDto> GetDoctorData()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(patientEmail))
            {
                return BadRequest("Patient not found.");
            }

            // Get the patientdoctorID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result;

            // Get the doctor data from the database
            var doctorData = _dbContext.Doctor.FirstOrDefault(d => d.Id == patient.PatientDoctorId);

            if (doctorData == null)
            {
                return NotFound("Doctor data not found.");
            }

            var doctorDto = new UserDto
            {
                Email = doctorData.Email,
                UserName = doctorData.UserName,
                FisrtName = doctorData.FirstName,
                LastName = doctorData.LastName,
                Address = doctorData.Address,
                PhoneNamber = doctorData.PhoneNumber,
                PictureUrl = doctorData.PictureUrl,
            };
            return Ok(doctorDto);
        }





        [HttpGet("GetObserverData")]
        public ActionResult<UserDto> GetObserverData()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(patientEmail))
            {
                return BadRequest("Patient not found.");
            }

            // Get the patient's ID from the database
            var patient = _userManager.FindByEmailAsync(patientEmail).Result;


            // Get the observer data from the database
            var observerData = _dbContext.Observer.FirstOrDefault(o => o.PatientObserverId == patient.Id);

            if (observerData == null)
            {
                return NotFound("Observer data not found.");
            }

            var observerDto = new UserDto
            {
                Email = observerData.Email,
                UserName = observerData.UserName,
                FisrtName = observerData.FirstName,
                LastName = observerData.LastName,
                Address = observerData.Address,
                PhoneNamber = observerData.PhoneNumber,
                PictureUrl = observerData.PictureUrl,
            };

            return Ok(observerDto);
        }
    }
}