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
        public Task<string?> CreateAdvisor(RegistrationDTO request)
        {
            var res = _userRepository.CreateAdvisor(request);

            return Task.FromResult(res);
        }

        public Task<string?> CreateClient(RegistrationDTO request, string email)
        {
            var res = _userRepository.CreateClient(request, email);

            return Task.FromResult(res);
        }

        public Task<string?> UpdateClient(AdvisorInfoDTO info, string ClientId)
        {
            throw new NotImplementedException();
        }
    }
}
