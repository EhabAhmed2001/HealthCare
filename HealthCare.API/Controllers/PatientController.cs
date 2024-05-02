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
using AutoMapper;

namespace HealthCare.PL.Controllers
{
    public class PatientController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HealthCareContext _dbContext;
        private readonly IMapper _mapper;

        public PatientController(UserManager<AppUser> userManager, ITokenService token, RoleManager<IdentityRole> roleManager, HealthCareContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _token = token;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost("PatientRegister")]
        public async Task<ActionResult<UserToReturnDto>> Register(PatientRegisterDto Patients)
        {
            // Check if the email is already exists
            var existingUserByEmail = await _userManager.FindByEmailAsync(Patients.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "This Email Is Already Exists!" });
            }

            // Check if the phone number is already exists
            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Patients.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "This Phone Number Is Already Exists!" });
            }

            // Check if the hardware is already used
            var IsHardwreUsed = await _dbContext.Patient.FirstOrDefaultAsync(p => p.HardwareId == Patients.HardwareId);

            if(IsHardwreUsed != null)
            {
                return BadRequest(new { message = "This Hardware Is Already Used!" });
            }

            //Check if the hardware is exists
            var Hardware = await _dbContext.Hardware.FirstOrDefaultAsync(HW => HW.Id == Patients.HardwareId);
            if (Hardware == null)
            {
                return BadRequest(new { message = "This Hardware Is Not Exist!" });
            }

            var Patient = new Patient()
            {
                FirstName = Patients.FirstName,
                LastName = Patients.LastName,
                Address = Patients.Address,
                UserName = Patients.Email.Split('@')[0],
                HardwareId = Patients.HardwareId,
                Gender = char.ToUpper(Patients.Gender[0]),
                BOD = Patients.BOD,
                PhoneNumber = Patients.PhoneNumber,
                Email = Patients.Email,
                BloodType = Patients.BloodType,
            };

            var Result = await _userManager.CreateAsync(Patient, Patients.Password!);


            if (Result.Succeeded)
            {
                await _userManager.AddToRoleAsync(Patient, "Patient");

                // change the hardware status to used
                Hardware!.IsUsed = true;
                await _dbContext.SaveChangesAsync();

                var PatientDto = new UserToReturnDto()
                {
                    UserName = Patient.UserName,
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
        }


        [HttpGet("GetDoctorData")]
        public ActionResult<DoctorToReturnDto> GetDoctorData()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patientdoctorID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            // Get the doctor data from the database
            var doctorData = _dbContext.Doctor.FirstOrDefault(d => d.Id == patient.PatientDoctorId);

            if (doctorData == null)
            {
                return BadRequest(new { Message = "Doctor data not found." });
            }

            var doctorDto = _mapper.Map<Doctor, DoctorToReturnDto>(doctorData);

            return Ok(doctorDto);
        }





        [HttpGet("GetObserverData")]
        public ActionResult<ObserverToReturnDto> GetObserverData()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;


            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            // Get the observer data from the database
            var observerData = _dbContext.Observer.FirstOrDefault(o => o.PatientObserverId == patient.Id);

            if (observerData == null)
            {
                return NotFound(new { message = "Observer data not found." });
            }

            var observerDto = _mapper.Map<Observer, ObserverToReturnDto>(observerData);

            return Ok(observerDto);
        }
    }
}