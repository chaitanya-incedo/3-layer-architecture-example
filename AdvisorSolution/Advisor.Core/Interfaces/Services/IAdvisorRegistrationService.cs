﻿using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;


namespace Advisor.Core.Interfaces.Services
{
    public interface IAdvisorRegistrationService
    {
        Task<string?> CreateAdvisor(AdvisorRegisterDTO request);
        Task<string> LoginAdvisor(AdvisorLoginDTO request);
        Task<string> ChangePassword(string email);
        Task<string> ResetPassword(PasswordResetDTO request);
        Task<string> ForgotPassword(PasswordResetWithoutLoginDTO request);
        Task<AdvisorInfoDTO?> GetAdvisorInfo(string email);
        Task<AdvisorInfoDTO> GetClientInfo(string id);
        Task<List<AdvisorInfoDTO>> GetAllAdvisors();
        Task<string> UpdateAdvisor(string email, AdvisorInfoDTO info);
        Task<string> DeleteUser(string id);
        Task<List<ClientInfoDto>> GetAllClientsForAnAdvisor(string email);

    }
}
