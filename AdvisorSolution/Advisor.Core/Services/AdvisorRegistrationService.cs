using Advisor.Core.Domain;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Services
{
    public class AdvisorRegistrationService : IAdvisorRegistrationService
    {
        private readonly IAdvisorRegistrationRepository _repository;

        public AdvisorRegistrationService(IAdvisorRegistrationRepository repository)
        {
            _repository= repository;
        }
        public AdvisorRegistrationDetails CreateUser(AdvisorDTO advisor)
        {
            var res=_repository.CreateUser(advisor);
            return res;
        }

        public string LoginAdvisor(AdvisorLoginDTO request)
        {
            var res=_repository.LoginAdvisor(request);
            return res;
        }

        public string VerifyAdvisor(string token)
        {
            return _repository.VerifyAdvisor(token);
        }
    }
}
