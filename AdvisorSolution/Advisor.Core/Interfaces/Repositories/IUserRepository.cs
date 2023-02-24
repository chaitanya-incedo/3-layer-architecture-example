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
        RegistrationDTO? CreateAdvisor(RegistrationDTO request);
        RegistrationDTO? CreateClient(RegistrationDTO request, string email);
        AdvisorInfoDTO? UpdateClient(AdvisorInfoDTO info, string ClientId);
    }
}
