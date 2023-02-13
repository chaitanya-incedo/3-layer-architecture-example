using Advisor.Core.Domain;
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
        public async Task<ActionResult<AdvisorRegistrationDetails>> Register(AdvisorDTO request)
        {
            var res = _service.CreateUser(request);
            if (res == null)
                return BadRequest("User already Exists.");
            return Ok("User created");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(AdvisorLoginDTO request)
        {
            var res = _service.LoginAdvisor(request);
            if (res.Equals("Email doesn't exist.") || res.Equals("Wrong password."))
                return BadRequest(res);

            return Ok(res);
        }

        [HttpPost("Forgot-password"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> ForgotPassword(string email) {
            var res = _service.ForgotPassword(email);
            if (res.Equals("Bad Request."))
                return BadRequest(res);
            //send an email to the user with the password reset token
            return Ok(res);
        }

        [HttpPost("Reset-password/{token}"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> ResetPassword(PasswordResetDTO reset) {
            var res = _service.ResetPassword(reset);
            if(res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }

    }
}
