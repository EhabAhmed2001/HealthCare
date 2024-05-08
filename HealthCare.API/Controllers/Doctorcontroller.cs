using AutoMapper;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using HealthCare.PL.Helper;
using HealthCare.Repository.Data;
using HealthCare.Service;
using Microsoft.AspNetCore.Hosting.Server;
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
                return BadRequest(new { message = "This Email Already Exists!" });
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "This Phone Number Already Exists!" });
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


        [HttpGet("GetAllPatients")]
        public async Task<ActionResult<List<UserSearchToReturnDto>>> GetAllPatients()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var doctor = (Doctor)_userManager.FindByEmailAsync(Email!).Result!;

            var Patients = await _dbContext.Patient.Where(p => p.PatientDoctorId == doctor.Id).ToListAsync();

            if (Patients == null || Patients.Count() == 0)
                return BadRequest(new { message = "No Patient Found!, Add One Or More To Show Details" });

            var ReturnedData = _mapper.Map<List<Patient>, List<UserSearchToReturnDto>>(Patients!);

            return Ok(ReturnedData);
        }


        [HttpGet("GetPatientData")]
        public async Task<ActionResult<PatientWithHistoryAndObserverToReturnDto>> GetPatientData(string PatientEmail)
        {
            var currentDoctorEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentDoctor = (Doctor)_userManager.FindByEmailAsync(currentDoctorEmail!).Result!;

            var PatientData = await UserHelper.GetPatientDataWithObserver(PatientEmail, _dbContext);

            if (PatientData == null || PatientData.PatientDoctorId != currentDoctor.Id)
                return BadRequest(new { message = "No Patient Found!" });

            var ReturnedData = _mapper.Map<Patient, PatientWithHistoryAndObserverToReturnDto>(PatientData!);
            ReturnedData.Observer = _mapper.Map<Observer, ObserverToReturnDto>(PatientData.PatientObserver!);
            return Ok(ReturnedData);
        }

        [HttpPost("AddPatientRequest")]
        public async Task<ActionResult<UserSearchToReturnDto>> AddPatientRequest(string PatientEmail)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Doctor = (Doctor)_userManager.FindByEmailAsync(Email!).Result!;


            // Check if the patient exists
            var patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);
            if (patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            //Check If Patient Has An Doctor

            if (patient.PatientDoctorId is not null)
            {
                return BadRequest(new { Message = "This Patient already has an Doctor, can't add more than an Doctor, he can remove his Doctor" });
            }

            var IsRequestExist = await UserHelper.CheckIfNotificationExist(patient.Id, Doctor.Id, _dbContext);
            if (IsRequestExist != null)
            {
                return BadRequest(new { Message = "This Request Already Sent Or Received" });
            }


            var Result = await UserHelper.AddOrEditNotification(Doctor.Id, patient.Id, Doctor.Email!, PatientEmail, _dbContext);
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

            // Check If Patient Has A Doctor
            if (Patient.PatientDoctorId != null)
                return BadRequest(new { message = "This Patient already has a Doctor!. To Sent Request, He Can Delete His Doctor Before" });


            var notification = await UserHelper.CheckIfNotificationExist(Patient.Id, Doctor.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "No Request Found!" });
            }
            else if(notification.SenderEmail != PatientEmail && notification.ReceiverEmail != Doctor.Email)
            {
                return BadRequest(new { Message = "Error In Request!" });
            }

            // Change the notification status to approved
            notification.Status = NotificationStatus.Approved;
            // Add the doctor to the patient's doctor
            Patient.PatientDoctorId = Doctor.Id;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { message = "Request Accepted Successfully!" });
            }

            return BadRequest(new { message = "Failed to accept request!" });
        }


        [HttpPut("DeletePatient")]
        public async Task<ActionResult> DeletePatient(string PatientEmail)
        {
            // Get the Doctor's email from the user's claims
            var DoctorEmail = User.FindFirstValue(ClaimTypes.Email)!;
            
            // Get the Doctor's Data from the database
            var Doctor = (Doctor)_userManager.FindByEmailAsync(DoctorEmail).Result!;

            // Get the patient's data from the database
            var Patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);

            // Check If Patient Exist And Has The Same Doctor
            if (Patient is null || Patient.PatientDoctorId != Doctor.Id)
                return BadRequest(new { Message = "No Patient Exist" });


            var notification = await UserHelper.CheckIfNotificationExist(Patient.Id, Patient.PatientDoctorId, _dbContext, NotificationStatus.Approved);

            if (notification is not null)
                notification!.Status = NotificationStatus.Canceled;


            //Make the patient Doctor id is null
            Patient.PatientDoctorId = null;
            var result = await _userManager.UpdateAsync(Doctor);

            if (result.Succeeded)
                return Ok(new { Message = "Patient Deleted Successfully" });

            return BadRequest(new { Message = "Failed To Delete The Patient, Try again" });

        }

    }
}