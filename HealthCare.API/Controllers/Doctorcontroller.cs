using AutoMapper;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using HealthCare.PL.Helper;
using HealthCare.Repository.Data;
using HealthCare.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace HealthCare.PL.Controllers
{

    public class DoctorController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        private readonly HealthCareContext _dbContext;

        public DoctorController(UserManager<AppUser> userManager, ITokenService token, IMapper Mapper, HealthCareContext DbContext)
        {
            _userManager = userManager;
            _token = token;
            _mapper = Mapper;
            _dbContext = DbContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserToReturnDto>> Register(DoctorRegisterDto model )
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "Doctor with this email Already Exists!" });
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "Doctor with this phone number Already Exists!" });
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


        [HttpGet("GetPatientData")]
        public async Task<ActionResult<UserSearchToReturnDto>> GetPatientData()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Observer = (Observer)_userManager.FindByEmailAsync(Email!).Result!;

            var Patient = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == Observer.PatientObserverId);
            if (Patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            var PatientData = await UserHelper.GetPatientData(Patient.Id, _dbContext);

            var ReturnedData = _mapper.Map<AppUser, PatientWithHistoryToReturnDto>(PatientData!);
            return Ok(ReturnedData);
        }

        [HttpGet("GetPatientHistory")]
        public async Task<ActionResult<HistoryToReturnDto>> GetPatientHistory(string email)
        {
            var Patient = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (Patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            var PatientHistory = await UserHelper.GetPatientHistory(Patient.Id, _dbContext);

            var ReturnedData = _mapper.Map<AppUser, HistoryToReturnDto>(PatientHistory!);
            return Ok(ReturnedData);
        }


        [HttpPut("AddPatientRequest")]
        public async Task<ActionResult<UserSearchToReturnDto>> AddPatientRequest(string PatientEmail)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Doctor = (Doctor)_userManager.FindByEmailAsync(Email!).Result!;

            // check if Doctor has a patient
            if (Doctor.PatientDoctorId != null)
                return BadRequest(new { message = "You already have a patient!, Delete Him And Add Another" });

            // Check if the patient exists
            var patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);
            if (patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            //Check If Patient Has An Doctor
            var IsDoctorable = _dbContext.Doctor.FirstOrDefault(O => O.PatientDoctorId == patient.Id);

            if (IsDoctorable != null)
            {
                return BadRequest(new { Message = "This Patient already have an Doctor, can't add more than an Doctor, he can remove his Doctor" });
            }

            var IsRequestExist = await UserHelper.CheckIfNotificationExist(patient.Id, Doctor.Id, _dbContext);
            if (IsRequestExist != null)
            {
                return BadRequest(new { Message = "This Request Already Sent Or Received" });
            }


            var Result = await UserHelper.AddOrEditToNotificatiopn(Doctor.Id, patient.Id, Doctor.Email!, PatientEmail, _dbContext);
            if (Result)
                return Ok(new { message = "Request Sent Successfully!" });
            else
                return BadRequest(new { message = "Failed to send request!" });
        }


        [HttpPut("AcceptPatientRequest")]
        public async Task<ActionResult<UserSearchToReturnDto>> AcceptPatientRequest(string PatientEmail)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Doctor = (Doctor?)_userManager.FindByEmailAsync(Email!).Result!;

            var Patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);

            if (Patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            // check if observer has a patient
            if (Doctor.PatientDoctorId != null)
                return BadRequest(new { message = "You already have an Doctor!, Delete Him And Add Another" });

            // Check If Patient Has An Doctor
            var IsDoctorable = _dbContext.Doctor.FirstOrDefault(O => O.PatientDoctorId == Patient.Id);
            if (IsDoctorable != null)
            {
                return BadRequest(new { Message = "This Patient Has Already An Doctor " });
            }


            var notification = await UserHelper.CheckIfNotificationExist(Patient.Id, Doctor.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "No Request Found!" });
            }

            // Change the notification status to approved
            notification.Status = NotificationStatus.Approved;
            // Add the doctor to the patient's doctor
            Doctor.PatientDoctorId = Patient.Id;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { message = "Request Accepted Successfully!" });
            }

            return BadRequest(new { message = "Failed to accept request!" });
        }


        [HttpPut("DeletePatient")]
        public async Task<ActionResult> DeletePatient()
        {
            // Get the Doctor's email from the user's claims
            var DoctorEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the Doctor's ID from the database
            var Doctor = (Doctor)_userManager.FindByEmailAsync(DoctorEmail).Result!;

            if (Doctor.PatientDoctorId == null)
            {
                return BadRequest(new { Message = "You don't have an Doctor to delete" });
            }

            //Make the patient Doctor id is null
            Doctor.PatientDoctorId = null;
            var result = await _userManager.UpdateAsync(Doctor);

            if (result.Succeeded)
                return Ok(new { Message = "Doctor Deleted Successfully" });

            return BadRequest(new { Message = "Failed to delete the Doctor, Try again" });

        }
    }
}