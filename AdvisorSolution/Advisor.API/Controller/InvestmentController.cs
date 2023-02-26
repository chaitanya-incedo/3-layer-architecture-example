using Advisor.Core.Domain.DTOs;
using Advisor.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


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

        [HttpPost("Create"),Authorize(Roles = "advisor")]
        public async Task<ActionResult<InvestmentDTO>> Create(InvestmentDTO request)
        {
            var email = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var res = await _service.CreateInvestment(request,email);
            return Ok(res);
        }

        [HttpGet("GetInvestmentInformation"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<List<InvestmentDTO>>> GetInvestment(int userid)
        {
            var res = await _service.GetInvestment(userid);
            if (res is null)
                return NotFound();
            return Ok(res);
        }
        [HttpGet("GetTotalInvestment"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<List<InvestmentDTO>>> GetTotal(string clientID)
        {
            var res = await _service.GetTotal(clientID);
            return Ok(res);
        }

        [HttpPut("update"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<InvestmentDTO>> Update(InvestmentDTO request)
        {
            var res = await _service.UpdateInvestment(request);
            return Ok(res);
        }
        [HttpDelete("delete"), Authorize(Roles = "advisor")]
        public async Task<ActionResult<InvestmentDTO>> Delete(int request)
        {
            var res = await _service.DeleteInvestment(request);
            return Ok(res);
        }
    }
}
