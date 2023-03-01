using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Repositories
{
    public interface IClientRepository
    {
        AdvisorRegisterDTO? CreateClient(AdvisorRegisterDTO request, string email);
        string LoginClient(AdvisorLoginDTO request);
        AdvisorInfoDTO? UpdateClient(AdvisorInfoDTO info);
    }
}
