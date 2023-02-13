using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Services
{
    public interface IAdvisorRegistrationService
    {
        AdvisorRegistrationDetails CreateUser(AdvisorDTO advisor);
        string LoginAdvisor(AdvisorLoginDTO request);
    }
}
