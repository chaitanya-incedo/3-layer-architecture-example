using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        string? CreateAdvisor(RegistrationDTO request);
        string? CreateClient(RegistrationDTO request, string email);
        string? UpdateClient(AdvisorInfoDTO info, string ClientId);
    }
}
