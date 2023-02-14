using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;

namespace Advisor.Core.Interfaces.Repositories
{
    public interface IAdvisorRegistrationRepository
    {
        AdvisorRegisterDTO? CreateAdvisor(AdvisorRegisterDTO request);
        string LoginAdvisor(AdvisorLoginDTO request);
        string ChangePasswordAdv(string email);
        string ResetPasswordAdvAfterLogin(PasswordResetDTO request,string email);
        string ForgotPassword(PasswordResetWithoutLoginDTO request);

/*        Task<AdvisorInfoToReturn?> GetAdvisorById(int id);
        Task<List<AdvisorInfoToReturn>?> GetAllAdvisors();

        Task<AdvisorInfoToReturn?> UpdateAdvisor(int id, AdvisorInfoToReturn advisor);
        Task<List<AdvisorInfo>?> DeleteAdvisor(int id);*/

    }
}
