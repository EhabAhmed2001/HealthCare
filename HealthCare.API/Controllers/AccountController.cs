﻿using HealthCare.Core.Entities.identity;
using HealthCare.Core.Services;
using HealthCare.PL.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Data;

namespace HealthCare.PL.Controllers
{
    public class AccountController : ControllerBase
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
        public async Task<ActionResult<UserDto>> login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized();
           var Result= await _signInManager.CheckPasswordSignInAsync(User,model.password,false);
            if (!Result.Succeeded) return Unauthorized();
            var role = await _userManager.GetRolesAsync(User);
            return Ok(new UserDto()
            {

                DisplayName = User.UserName,
                Email = User.Email,
                Token = await token.CreateTokenAsync(User),
                Role = role[0]
                
        }) ;
            
        }
        
    }
}