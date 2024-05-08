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
using HealthCare.PL.Helper;
using System.Reflection;
using HealthCare.Core.AddRequest;

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


        [HttpPost("AddDoctorRequest")]
        public async Task<ActionResult> AddDoctorRequest(string Email)
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            //Check If Ptient Has Doctor
            if (patient.PatientDoctorId != null)
            {
                return BadRequest(new { Message = "You already have a doctor, can't add more than a doctor, you can remove your doctor then add another" });
            }

            // Get the doctor's ID from the database
            var doctor = await UserHelper.UserSearch(Email, "Doctor", _userManager);

            if (doctor == null)
            {
                return BadRequest(new { Message = "Doctor not found." });
            }

            // Check If Sent Request Before
            var IsRequestExist = await UserHelper.CheckIfNotificationExist(patient.Id, doctor.Id, _dbContext);
            if (IsRequestExist != null)
            {
                return BadRequest(new { Message = "This Request Already Sent Or Received" });
            }

            // Add the request to the database
            var result = await UserHelper.AddOrEditNotification(patient.Id, doctor.Id, patient.Email!, Email, _dbContext);

            if (result)
            {
                return Ok(new { Message = "Request sent successfully." });
            }

            return BadRequest(new { Message = "Failed to send the request. Try again" });
        }


        [HttpPost("AddObserverRequest")]
        public async Task<ActionResult> AddObserverRequest(string Email)
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's Data from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            //Check If Ptient Has Observer
            var IsObserverable = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == patient.Id);

            if (IsObserverable != null)
            {
                return BadRequest(new { Message = "You already have an observer, can't add more than an observer, you can remove your observer then add another" });
            }

            // Get the observer's ID from the database
            var observer = (Observer?)await UserHelper.UserSearch(Email, "Observer", _userManager);

            if (observer == null)
            {
                return BadRequest(new { Message = "Observer not found." });
            }

            //Check If Observer Has Ptient
            if (observer.PatientObserverId != null)
            {
                return BadRequest(new { Message = "Observer already has a patient, Can't add more than a patient, He can remove the patient then add another" });
            }

            // Check If Sent Request Before
            var IsRequestExist = await UserHelper.CheckIfNotificationExist(patient.Id, observer.Id, _dbContext);
            if (IsRequestExist != null)
            {
                return BadRequest(new { Message = "This Request Already Sent Or Received" });
            }

            // Add the request to the database
            var result = await UserHelper.AddOrEditNotification(patient.Id, observer.Id, patient.Email!, Email, _dbContext);

            if (result)
            {
                return Ok(new { Message = "Request sent successfully." });
            }

            return BadRequest(new { Message = "Failed to send the request. Try again" });
        }

        /*

        [HttpPut("DeleteDoctor")]
        public async Task<ActionResult> DeleteDoctor()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            if(patient.PatientDoctorId == null)
            {
                return BadRequest(new { Message = "You don't have a doctor to delete" });
            }

            //Make the patient doctor id is null
            patient.PatientDoctorId = null;
            var result = await _userManager.UpdateAsync(patient);

            if(result.Succeeded)
                return Ok(new { Message = "Doctor Deleted Successfully" });

            return BadRequest(new { Message = "Failed to delete the doctor, Try again" });
            
        }

        
        [HttpPut("DeleteObserver")]
        public async Task<ActionResult> DeleteObserver()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            var observer = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == patient.Id);

            if (observer == null)
            {
                return BadRequest(new { Message = "You don't have an observer to delete" });
            }

            //Make the patient observer id is null
            observer.PatientObserverId = null;
            var result = await _userManager.UpdateAsync(observer);

            if (result.Succeeded)
                return Ok(new { Message = "Observer Deleted Successfully" });

            return BadRequest(new { Message = "Failed to delete the observer, Try again" });

        }

        */

        [HttpPut("AcceptRequest")]
        public async Task<ActionResult> AcceptRequest(string SenderEmail)
        {
            // Get the patient's email from the user's claims
            var ReceiverEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var Receiver =  _dbContext.Patient.FirstOrDefault(P=>P.Email == ReceiverEmail)!;

            // Get the sender's ID from the database
            var SenderEmailCheck = await _userManager.FindByEmailAsync(SenderEmail);

            if (SenderEmailCheck == null)
            {
                return BadRequest(new { Message = "User not found." });
            }

            var Sender = _userManager.FindByEmailAsync(SenderEmail).Result!;


            // Check If Request Exist
            var notification = await UserHelper.CheckIfNotificationExist(Sender.Id, Receiver.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "Request not found." });
            }
            else if (notification.SenderEmail != SenderEmail && notification.ReceiverEmail != ReceiverEmail)
            {
                return BadRequest(new { Message = "Error In Request!" });
            }


            var role = await _userManager.GetRolesAsync(Sender);

            if (role.Contains("Doctor"))
            {
                //Check If Ptient Has Doctor
                if (Receiver.PatientDoctorId != null)
                {
                    return BadRequest(new { Message = "You already have a doctor, can't add more than a doctor, you can remove your doctor then add another" });
                }
                // Change the request status to accepted
                notification.Status = NotificationStatus.Approved;
                // Add the doctor to the patient's doctor
                Receiver.PatientDoctorId = Sender.Id;

                if( await _dbContext.SaveChangesAsync() > 0)
                {
                    return Ok(new { Message = "Request accepted successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to accept the request. Try again" });
                }
            }

            else if (role.Contains("Observer"))
            {
                //Check If Ptient Has Observer
                var IsObserverable = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == Receiver.Id);
                if (IsObserverable != null)
                {
                    return BadRequest(new { Message = "You already have an observer, can't add more than an observer, you can remove your observer then add another" });
                }

                var observer = (Observer) Sender;                

                //Check If Observer Has Ptient
                if (observer.PatientObserverId != null)
                {
                    return BadRequest(new { Message = "Observer already has a patient, Can't add more than a patient, He can remove the patient then add another" });
                }

                notification.Status = NotificationStatus.Approved;
                // Add the Observer to the patient's Observer
                observer.PatientObserverId = Receiver.Id;

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return Ok(new { Message = "Request accepted successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to accept the request. Try again" });
                }
            }

            return BadRequest(new { Message = "Failed to accept the request. Try again" });

        }


        [HttpPut("RemoveDoctor")]
        public async Task<ActionResult> RemoveDoctor()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            // check if the patient has a doctor
            if (patient.PatientDoctorId == null)
            {
                return BadRequest(new { Message = "You don't have a doctor to remove" });
            }

            // Update Notification Status to Canceled
            var notification = await UserHelper.CheckIfNotificationExist(patient.Id, patient.PatientDoctorId!, _dbContext, NotificationStatus.Approved);

            // Remove the doctor from the patient's doctor
            patient.PatientDoctorId = null;

            notification!.Status = NotificationStatus.Canceled;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { Message = "Doctor removed successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to remove the doctor. Try again" });
            }

        }

        [HttpPut("RemoveObserver")]
        public async Task<ActionResult> RemoveObserver()
        {
            // Get the patient's email from the user's claims
            var patientEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the patient's ID from the database
            var patient = (Patient)_userManager.FindByEmailAsync(patientEmail).Result!;

            // check if the patient has an observer
            var observer = _dbContext.Observer.FirstOrDefault(O => O.PatientObserverId == patient.Id);

            if (observer == null)
            {
                return BadRequest(new { Message = "You don't have an observer to remove" });
            }

            // Update Notification Status to Canceled
            var notification = await UserHelper.CheckIfNotificationExist(patient.Id, observer.Id, _dbContext, NotificationStatus.Approved);
            notification!.Status = NotificationStatus.Canceled;

            // Remove the observer from the patient's observer
            observer.PatientObserverId = null;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { Message = "Observer removed successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to remove the observer. Try again" });
            }

        }

        

    }
}