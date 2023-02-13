using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Advisor.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdvisorRegistrationService _service;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IAdvisorRegistrationService service)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AdvisorRegistrationDetails>> Register(AdvisorDTO request)
        {
            var res=_service.CreateUser(request);
            return Ok(res);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(AdvisorLoginDTO request)
        {
            var res=_service.LoginAdvisor(request);
            if (res.Equals("Email doesn't exist.") || res.Equals("Wrong password."))
                return BadRequest(res);
            
            return Ok(res);
        }
    }
}
