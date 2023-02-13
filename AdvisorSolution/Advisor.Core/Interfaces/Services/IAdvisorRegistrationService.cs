using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;

namespace Advisor.Core.Interfaces.Services
{
    public interface IAdvisorRegistrationService
    {
        AdvisorRegistrationDetails CreateUser(AdvisorDTO advisor);
        string LoginAdvisor(AdvisorLoginDTO request);
        public string VerifyAdvisor(string token);
    }
}
