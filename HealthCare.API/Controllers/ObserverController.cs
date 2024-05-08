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
using AutoMapper;
using HealthCare.PL.Helper;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Hosting.Server;
using HealthCare.Core.AddRequest;

namespace HealthCare.PL.Controllers
{
    public class ObserverController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        private readonly HealthCareContext _dbContext;

        public ObserverController(UserManager<AppUser> userManager, ITokenService token, IMapper Mapper, HealthCareContext DbContext)
        {
            _userManager = userManager;
            _token = token;
            _mapper = Mapper;
            _dbContext = DbContext;
        }

        [HttpPost("ObserverRegister")]
        public async Task<ActionResult<UserToReturnDto>> Register(ObserverRegisterDto Observers)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(Observers.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { message = "This Email Already Exists!" });
            }

            var existingUserByPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Observers.PhoneNumber);

            if (existingUserByPhoneNumber != null)
            {
                return BadRequest(new { message = "This Phone Number Already Exists!" });
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

        [HttpGet("GetPatientData")]
        public async Task<ActionResult<PatientDataWithDoctorAndObserverToReturnDto>> GetPatientData()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Observer = (Observer) _userManager.FindByEmailAsync(Email!).Result!;

            var Patient = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == Observer.PatientObserverId);
            if(Patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            var PatientData = await UserHelper.GetPatientDataWithDoctor(Patient.Id, _dbContext);

            var ReturnedData = _mapper.Map<AppUser, PatientDataWithDoctorAndObserverToReturnDto>(PatientData!);
            return Ok(ReturnedData);
        }

        [HttpPost("AddPatientRequest")]
        public async Task<ActionResult<UserSearchToReturnDto>> AddPatientRequest(string PatientEmail)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Observer = (Observer)_userManager.FindByEmailAsync(Email!).Result!;

            // check if observer has a patient
            if (Observer.PatientObserverId != null)
                return BadRequest(new { message = "You Already Have A Patient!, Delete Him And Add Another" });

            // Check if the patient exists
            var patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);
            if (patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            //Check If Ptient Has An Observer
            var IsObserverable = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == patient.Id);

            if (IsObserverable != null)
            {
                return BadRequest(new { Message = "This Patient Already Has An Observer, Can't Add More Than A Patient" });
            }

            var IsRequestExist = await UserHelper.CheckIfNotificationExist(patient.Id, Observer.Id, _dbContext);
            if (IsRequestExist != null)
            {
                return BadRequest(new { Message = "This Request Already Sent Or Received" });
            }


            var Result = await UserHelper.AddOrEditNotification(Observer.Id, patient.Id, Observer.Email!, PatientEmail, _dbContext);
            if (Result)
                return Ok(new { message = "Request Sent Successfully!" });
            else
                return BadRequest(new { message = "Failed to send request!" });
        }


        [HttpPut("AcceptPatientRequest")]
        public async Task<ActionResult<UserSearchToReturnDto>> AcceptPatientRequest(string PatientEmail)
        {
            var ObserverEmail = User.FindFirstValue(ClaimTypes.Email);
            var Observer = (Observer?)_userManager.FindByEmailAsync(ObserverEmail!).Result!;

            var Patient = (Patient?)await UserHelper.UserSearch(PatientEmail, "Patient", _userManager);

            if (Patient == null)
                return BadRequest(new { message = "No Patient Found!" });

            // check if observer has a patient
            if (Observer.PatientObserverId != null)
                return BadRequest(new { message = "You Already Have A Patient!, Delete Him And Add Another" });

            // Check If Patient Has An Observer
            var IsObserverable = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == Patient.Id);
            if (IsObserverable != null)
            {
                return BadRequest(new { Message = "This Patient Has Already An Observer " });
            }


            var notification = await UserHelper.CheckIfNotificationExist(Patient.Id, Observer.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "No Request Found!" });
            }
            else if (notification.SenderEmail != PatientEmail && notification.ReceiverEmail != ObserverEmail)
            {
                return BadRequest(new { Message = "Error In Request!" });
            }

            // Change the notification status to approved
            notification.Status = NotificationStatus.Approved;
            // Add the doctor to the patient's doctor
            Observer.PatientObserverId = Patient.Id;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { message = "Request Accepted Successfully!" });
            }

            return BadRequest(new { message = "Failed to accept request!" });
        }


        [HttpPut("DeletePatient")]
        public async Task<ActionResult> DeletePatient()
        {
            // Get the Observer's email from the user's claims
            var ObserverEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the Observer's ID from the database
            var observer = (Observer)_userManager.FindByEmailAsync(ObserverEmail).Result!;

            if (observer.PatientObserverId == null)
            {
                return BadRequest(new { Message = "You Don't Have A Patient To Delete" });
            }

            // Update Notification Status to Canceled
            var notification = await UserHelper.CheckIfNotificationExist(observer.Id, observer.PatientObserverId, _dbContext, NotificationStatus.Approved);
            
            if (notification is not null)
                notification!.Status = NotificationStatus.Canceled;

            //Make the patient observer id is null
            observer.PatientObserverId = null;
            var result = await _userManager.UpdateAsync(observer);

            if (result.Succeeded)
                return Ok(new { Message = "Patient Deleted Successfully" });

            return BadRequest(new { Message = "Failed To Delete The Patient, Try Again" });

        }




    }
}