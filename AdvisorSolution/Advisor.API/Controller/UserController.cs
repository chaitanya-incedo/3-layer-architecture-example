using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;
using Advisor.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Claims;

namespace Advisor.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAdvisorRegistrationService _service;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IClientService _clientService;
        private readonly IUserService _userService;

        public UserController(IUserService userService,IAdvisorRegistrationService service, IHttpContextAccessor httpContext, IClientService clientService)
        {
            _userService = userService;
            _service = service;
            _httpContext = httpContext;
            _clientService=clientService;
        }


        [HttpPost("Registration")]
        public async Task<ActionResult<string>> Registration(RegistrationDTO request)
        {
            if (request.RoleID == 2)
            {
                var advisorid = request.AdvisorID;
                var res = await _userService.CreateClient(request, advisorid);
                if (res == null)
                    return BadRequest("Client already Exists.");
                return Ok(res);
            }
            else {
                var res = await _userService.CreateAdvisor(request);
                if (res == null)
                    return BadRequest("Advisor already Exists.");
                return Ok(res);
            }
            
        }
        [HttpPost("AdvisorLogin")]
        public async Task<ActionResult<string>> AdvisorLogin(AdvisorLoginDTO request)
        {
            var res = await _service.LoginAdvisor(request);
            if (res.Equals("Email doesn't exist.") || res.Equals("Wrong password."))
                return BadRequest(res);

            return Ok(res);
        }
        /*[HttpPost("Reset-password/{token}")]*/
        [HttpPost("AdvisorForgot")]
        /*public async Task<ActionResult<string>> ResetPassword(PasswordResetDTO reset, string token){*/
        public async Task<ActionResult<string>> AdvisorForgotPassword(PasswordResetWithoutLoginDTO reset)
        {
            var res = await _service.ForgotPassword(reset);
            if (res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPost("AdvisorReset")]
        public async Task<ActionResult<string>> AdvisorResetPassword(PasswordResetDTO request)
        {
            var res = await _service.ResetPassword(request);
            if (res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }







        /*                                [HttpPost("ClientRegister"), Authorize(Roles = "advisor")]
                                        public async Task<ActionResult<AdvisorRegisterDTO>> ClientRegister(AdvisorRegisterDTO request)
                                        {
                                            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                                            var res = await _clientService.CreateClient(request,email);
                                            if (res == null)
                                                return BadRequest("User already Exists.");
                                            return Ok(res);
                                        }*/

        /*[HttpPost("ClientLogin")]
        public async Task<ActionResult<string>> ClientLogin(AdvisorLoginDTO request)
        {
            var res = await _clientService.LoginClient(request);
            if (res.Equals("Email doesn't exist.") || res.Equals("Wrong password."))
                return BadRequest(res);

            return Ok(res);
        }*/



        [HttpGet("GetAllClients"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<List<AdvisorInfoDTO>>> GetAllClientsForAnAdvisor()
        {
            var result = string.Empty;
            if (_httpContext.HttpContext != null)
            {
                result = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            Console.WriteLine(result);
            return Ok(await _service.GetAllClientsForAnAdvisor(result));
        }









        /*        [HttpGet, Authorize(Roles = "advisor")]
                public ActionResult<string> GetMeForAdvisor()
                {
                    Console.WriteLine("here");
                    string result = string.Empty;
                    if (_httpContext.HttpContext != null)
                    {
                        result = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                    }

                    return Ok(result);
                }*/

        [HttpGet("AdvisorInfo"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<AdvisorInfoDTO?>> GetAdvisorInfo()
        {

            var result = string.Empty;
            if (_httpContext.HttpContext != null)
            {
                result = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }

            AdvisorInfoDTO res = await _service.GetAdvisorInfo(result);
            if (res is null)
                return NoContent();
            return Ok(res);
        }
        [HttpGet("clientInfo"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<AdvisorInfoDTO?>> GetClientInfo(string id)
        {
            AdvisorInfoDTO res = await _service.GetClientInfo(id);
            if (res is null)
                return NoContent();
            return Ok(res);
        }

        [HttpGet("GetAllAdvisors"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<List<AdvisorInfoDTO>>> GetAllAdvisors()
        {
            return Ok(await _service.GetAllAdvisors());
        }

/*        [HttpPost("AdvisorRegister")]
        public async Task<ActionResult<AdvisorRegisterDTO>> AdvisorRegister(AdvisorRegisterDTO request)
        {
            var res = await _service.CreateAdvisor(request);
            if (res == null)
                return BadRequest("User already Exists.");
            return Ok(res);
        }*/

        

        /*[HttpPost("Advisor-change-password"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> AdvisorChangePassword()
        {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var res = await _service.ChangePassword(email);
            if (res.Equals("Bad Request."))
                return BadRequest(res);
            //send an email to the user with the password reset token
            return Ok(res);
        }

        *//*[HttpPost("Reset-password/{token}")]*//*
        [HttpPost("Advisor-Reset-password-after-login"), Authorize(Roles = "advisor")]
        *//*public async Task<ActionResult<string>> ResetPassword(PasswordResetDTO reset, string token){*//*
        public async Task<ActionResult<string>> AdvisorResetPasswordLogin(PasswordResetDTO reset)
        {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var res = await _service.ResetPassword(reset, email);
            if (res.Equals("Session expired."))
                return BadRequest(res);
            return Ok(res);
        }*/

       

        [HttpPut("Update"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> Update(AdvisorInfoDTO info)
        {
            if (info.AdvisorID[0] == 'A')
            {
                var result = string.Empty;
                if (_httpContext.HttpContext != null)
                {
                    result = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
                }
                Console.WriteLine(result);
                var res = await _service.UpdateAdvisor(result, info);
                if (res is null)
                    return NoContent();
                return Ok(res);
            }
            else {
                AdvisorInfoDTO res = await _clientService.UpdateClient(info);
                if (res is null)
                    return NoContent();
                return Ok(res);
            }
        }
       
        

        [HttpDelete("Delete"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<string>> DeleteUser(string id)
        {
            var res = await _service.DeleteUser(id);
            return Ok(res);
        }
    }
}
