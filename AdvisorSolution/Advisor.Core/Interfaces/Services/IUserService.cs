using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<string?> CreateClient(RegistrationDTO request, string email);
        Task<string?> CreateAdvisor(RegistrationDTO request);
        Task<string?> UpdateClient(AdvisorInfoDTO info, string ClientId);
    }
}
