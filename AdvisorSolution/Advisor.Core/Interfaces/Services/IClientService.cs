using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Services
{
    public interface IClientService
    {
        Task<AdvisorRegisterDTO?> CreateClient(AdvisorRegisterDTO request, string email);
        Task<string> LoginClient(AdvisorLoginDTO request);
        Task<AdvisorInfoDTO?> UpdateClient(AdvisorInfoDTO info, string ClientId);
    }
}
