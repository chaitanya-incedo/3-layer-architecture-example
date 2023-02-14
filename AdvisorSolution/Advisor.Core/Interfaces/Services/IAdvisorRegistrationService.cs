using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;


namespace Advisor.Core.Interfaces.Services
{
    public interface IAdvisorRegistrationService
    {
        Task<AdvisorRegisterDTO?> CreateAdvisor(AdvisorRegisterDTO request);
        Task<string> LoginAdvisor(AdvisorLoginDTO request);
        Task<string> ChangePassword(string email);
        Task<string> ResetPassword(PasswordResetDTO request,string email);
        Task<string> ForgotPassword(PasswordResetWithoutLoginDTO request);

        /*        Task<AdvisorInfoToReturn?> GetAdvisorById(int id);
                Task<List<AdvisorInfoToReturn>?> GetAllAdvisors();

                Task<AdvisorInfoToReturn?> UpdateAdvisor(int id, AdvisorInfoToReturn advisor);
                Task<List<AdvisorInfo>?> DeleteAdvisor(int id);*/

    }
}
