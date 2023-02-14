﻿using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advisor.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdvisorRegistrationService _service;

        private readonly IHttpContextAccessor _httpContext;

        public AuthController( IAdvisorRegistrationService service, IHttpContextAccessor httpContext)
        {
            _service = service;
            _httpContext= httpContext;
        }

        [HttpGet, Authorize(Roles = "advisor")]
        public ActionResult<string> GetMe()
        {
            Console.WriteLine("here");
            string result = string.Empty;
            if (_httpContext.HttpContext != null)
            {
                result = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            
            return Ok(result);
        }










        [HttpPost("Register")]
        public async Task<ActionResult<AdvisorRegisterDTO>> Register(AdvisorRegisterDTO request)
        {
            var res =await _service.CreateAdvisor(request);
            if (res == null)
                return BadRequest("User already Exists.");
            return Ok("User created");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(AdvisorLoginDTO request)
        {
            var res = await _service.LoginAdvisor(request);
            if (res.Equals("Email doesn't exist.") || res.Equals("Wrong password."))
                return BadRequest(res);

            return Ok(res);
        }

        [HttpPost("change-password"),  Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> ChangePassword()
        {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var res = await _service.ChangePassword(email);
            if (res.Equals("Bad Request."))
                return BadRequest(res);
            //send an email to the user with the password reset token
            return Ok(res);
        }

        /*[HttpPost("Reset-password/{token}")]*/
        [HttpPost("Reset-password-after-login"), Authorize(Roles = "advisor")]
        /*public async Task<ActionResult<string>> ResetPassword(PasswordResetDTO reset, string token){*/
        public async Task<ActionResult<string>> ResetPasswordLogin(PasswordResetDTO reset) {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var res = await _service.ResetPassword(reset, email);
            if(res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }


        /*[HttpPost("Reset-password/{token}")]*/
        [HttpPost("Reset-password-without-login")]
        /*public async Task<ActionResult<string>> ResetPassword(PasswordResetDTO reset, string token){*/
        public async Task<ActionResult<string>> ResetPassword(PasswordResetWithoutLoginDTO reset)
        {
            var res = await _service.ForgotPassword(reset);
            if (res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }

    }
}
