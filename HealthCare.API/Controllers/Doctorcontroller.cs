using HealthCare.Core.Entities;
using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using HealthCare.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HealthCare.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Doctorcontroller : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _token;
       

        public Doctorcontroller(UserManager<AppUser> userManager, ITokenService token)
        {
            _userManager = userManager;
            _token = token;
           

        }

        [HttpPost("Register")]

        public async Task<ActionResult<DoctorDTO>> Register(DoctorRegisterDto model )
        {

            var Doctor = new AppUser()
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
                    
                    var doctordto = new DoctorDTO()
                    {
                        FirstName = model.FirstName,
                        Email = model.Email,
                        Role = "Doctor",
                        Token = await _token.CreateTokenAsync(Doctor),
                    };
                    return (doctordto);
            
            }
            BadRequestResult badRequestResult = BadRequest();
            return Ok(badRequestResult);
        }
    
        
    }
}
