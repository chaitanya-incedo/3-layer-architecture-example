using Advisor.Core.Domain.DTOs;
using Advisor.Core.Interfaces.Services;
using Advisor.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Services
{
    public class UserService : IUserService
    {   
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<RegistrationDTO?> CreateAdvisor(RegistrationDTO request)
        {
            var res = _userRepository.CreateAdvisor(request);

            return Task.FromResult(res);
        }

        public Task<RegistrationDTO?> CreateClient(RegistrationDTO request, string email)
        {
            var res = _userRepository.CreateClient(request, email);

            return Task.FromResult(res);
        }

        public Task<AdvisorInfoDTO?> UpdateClient(AdvisorInfoDTO info, string ClientId)
        {
            throw new NotImplementedException();
        }
    }
}
