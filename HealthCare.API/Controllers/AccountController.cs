using AutoMapper;
using HealthCare.Core;
using HealthCare.Core.AddRequest;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.Core.Specifications.NotificationSpecification;
using HealthCare.PL.DTOs;
using HealthCare.PL.Helper;
using HealthCare.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Security.Claims;


namespace HealthCare.PL.Controllers
{
    [Authorize]
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager <AppUser>_signInManager;
        private readonly ITokenService token;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HealthCareContext _dbContext;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService Token, IMapper Mapper, IUnitOfWork UnitOfWork, HealthCareContext DbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
             token = Token;
            _mapper = Mapper;
            _unitOfWork = UnitOfWork;
            _dbContext = DbContext;
        }

        [AllowAnonymous]
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

            var user = await _userManager.FindByEmailAsync(Email);

            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Patient"))
            {
                var CurrentPatient = (Patient)user;
                var patient = await UserHelper.GetPatientDataWithDoctorAndObserver(CurrentPatient.Id, _dbContext);
                var PatientToReturn = _mapper.Map<Patient, PatientDataWithDoctorAndObserverToReturnDto>(patient);
                PatientToReturn.Observer = _mapper.Map<Observer, ObserverToReturnDto>(patient!.PatientObserver);

                return Ok(PatientToReturn);
            }

            else if (role.Contains("Doctor"))
            {
                var CurrentDoctor = (Doctor)user;
                var DoctorToReturn = _mapper.Map<Doctor, DoctorToReturnDto>(CurrentDoctor);
                return Ok(DoctorToReturn);

            }
            else
            {
                var CurrentObserver = (Observer)user;
                var ObserverToReturn = _mapper.Map<Observer, ObserverToReturnDto>(CurrentObserver);
                return Ok(ObserverToReturn);

            }



        }


        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<IReadOnlyList<UserSearchToReturnDto>>> GetAllRequests()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Spec = new NotificationSpec(Email!);
            var AllNotifications = await _unitOfWork.CreateRepository<Notification>().GetAllWithSpecAsync(Spec);
            List<UserSearchToReturnDto> ReturnedNotifications = new List<UserSearchToReturnDto>();
            foreach (var Notification in AllNotifications) 
            {
                var user = await _userManager.FindByEmailAsync(Notification.SenderEmail);
                var MappedUser = _mapper.Map<AppUser, UserSearchToReturnDto>(user!);
                ReturnedNotifications.Add(MappedUser); 
            }

            return Ok(ReturnedNotifications);
        }


        [HttpGet("Search")]
        public async Task<ActionResult<UserSearchToReturnDto>> Search(string Email, string Role)
        {
            // Get the doctor data from the database
            var UserData = await _userManager.FindByEmailAsync(Email);

            if (UserData == null)
            {
                return BadRequest(new { Message = $"{Role} data not found." });
            }

            // Get User Role
            var role = await _userManager.GetRolesAsync(UserData);
            if (!role.Contains(Role))
            {
                return BadRequest(new { Message = $"{Role} data not found." });
            }

            var CurrentEmail = User.FindFirstValue(ClaimTypes.Email);

            var CurrentUser = await _userManager.FindByEmailAsync(CurrentEmail!);

            var CurrentRole = await _userManager.GetRolesAsync(CurrentUser!);

            if(CurrentRole.Contains(Role) || (Role == "Doctor" && CurrentRole.Contains("Observer")) || (Role == "Observer" && CurrentRole.Contains("Doctor")))
            {
                return BadRequest(new { Message = "You are not allowed to search for this user." });
            }

            var UserDto = _mapper.Map<AppUser, UserSearchToReturnDto>(UserData);
            /*    new UserSearchToReturnDto()
            {
                FirstName = ObserverData.FirstName,
                LastName = ObserverData.LastName,
                UserName = ObserverData.UserName!,
                PictureUrl = ObserverData.PictureUrl
            }; */

            return Ok(UserDto);
        }


        [HttpPut("RejectRequest")]
        public async Task<ActionResult> RejectRequest(string SenderEmail)
        {
            // Get the Receiver Email from the user's claims
            var ReceiverEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the Receiver's ID from the database
            var Receiver = _userManager.FindByEmailAsync(ReceiverEmail).Result!;

            // Get the sender's ID from the database
            var Sender = _userManager.FindByEmailAsync(SenderEmail).Result!;

            if (Sender == null)
            {
                return BadRequest(new { Message = "User not found." });
            }

            var notification = await UserHelper.CheckIfNotificationExist(Sender.Id, Receiver.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "Request not found." });
            }
            else if (notification.SenderEmail != SenderEmail && notification.ReceiverEmail != ReceiverEmail)
            {
                return BadRequest(new { Message = "Error In Request!" });
            }

            notification.Status = NotificationStatus.Rejected;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { Message = "Request rejected successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to reject the request. Try again" });
            }

        }


        [HttpPut("CancelRequest")]
        public async Task<ActionResult> CancelRequest(string ReceiverEmail)
        {
            // Get the Sender Email from the user's claims
            var SenderEmail = User.FindFirstValue(ClaimTypes.Email)!;

            // Get the Sender's ID from the database
            var Sender = await _userManager.FindByEmailAsync(SenderEmail)!;

            // Get the receiver's ID from the database
            var Receiver = await _userManager.FindByEmailAsync(ReceiverEmail)!;

            if (Receiver == null)
            {
                return BadRequest(new { Message = "User not found." });
            }

            var notification = await UserHelper.CheckIfNotificationExist(Sender.Id, Receiver.Id, _dbContext);

            if (notification == null)
            {
                return BadRequest(new { Message = "Request not found." });
            }
            else if (notification.SenderEmail != SenderEmail && notification.ReceiverEmail != ReceiverEmail)
            {
                return BadRequest(new { Message = "Error In Request!" });
            }

            notification.Status = NotificationStatus.Canceled;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Ok(new { Message = "Request canceled successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to cancel the request. Try again" });
            }

        }


    }
}
