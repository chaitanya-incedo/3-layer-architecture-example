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
        AdvisorInfoDTO? GetAdvisorInfo(string email);
        List<AdvisorInfoDTO> GetAllAdvisors();
        AdvisorInfoDTO UpdateAdvisor(string email, AdvisorInfoDTO info);
        List<AdvisorInfoDTO> DeleteUser(string email);

    }
}
