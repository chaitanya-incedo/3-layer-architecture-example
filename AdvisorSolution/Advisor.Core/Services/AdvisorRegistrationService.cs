using Advisor.Core.Domain;
using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advisor.Core.Services
{
    public class AdvisorRegistrationService : IAdvisorRegistrationService
    {
        private readonly IAdvisorRegistrationRepository _repository;

        public AdvisorRegistrationService(IAdvisorRegistrationRepository repository)
        {
            _repository = repository;
        }

        public Task<AdvisorRegisterDTO?> CreateAdvisor(AdvisorRegisterDTO request)
        {
            var res = _repository.CreateAdvisor(request);

            return Task.FromResult(res);
        }

        public Task<string> LoginAdvisor(AdvisorLoginDTO request)
        {
            var res = _repository.LoginAdvisor(request);
            return Task.FromResult(res);
        }

        public Task<string> ChangePassword(string email)
        {
            var res = _repository.ChangePasswordAdv(email);
            return Task.FromResult(res);
        }

        public Task<string> ResetPassword(PasswordResetDTO reset, string email)
        {
            return Task.FromResult(_repository.ResetPasswordAdvAfterLogin(reset, email));
        }

        public Task<string> ForgotPassword(PasswordResetWithoutLoginDTO request)
        {
            return Task.FromResult(_repository.ForgotPassword(request));
        }

        public Task<AdvisorInfoDTO?> GetAdvisorInfo(string email)
        {
            return Task.FromResult(_repository.GetAdvisorInfo(email));
        }

        public Task<List<AdvisorInfoDTO>> GetAllAdvisors()
        {
            return Task.FromResult(_repository.GetAllAdvisors());
        }

        public Task<AdvisorInfoDTO> UpdateAdvisor(string email, AdvisorInfoDTO info)
        {
            return Task.FromResult(_repository.UpdateAdvisor(email, info));
        }

        public Task<List<AdvisorInfoDTO>> DeleteUser(string email)
        {
            return Task.FromResult(_repository.DeleteUser(email));
        }

    }

}