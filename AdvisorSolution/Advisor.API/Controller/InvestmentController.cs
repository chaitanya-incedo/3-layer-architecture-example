using Advisor.Core.Domain.DTOs;
using Advisor.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advisor.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {

        private readonly IInvestmentService _service;

        private readonly IHttpContextAccessor _httpContext;

        public InvestmentController(IInvestmentService service, IHttpContextAccessor httpContext)
        {
            _service = service;
            _httpContext = httpContext;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<InvestmentDTO>> Create(InvestmentDTO request)
        {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var res = await _service.CreateInvestment(request,email);
            return Ok("Investment Added.");
        }
    }
}
