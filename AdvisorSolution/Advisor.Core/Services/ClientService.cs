using Advisor.Core.Domain.DTOs;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository clientRepository) {
            _repository=clientRepository;
        }
        public Task<AdvisorRegisterDTO?> CreateClient(AdvisorRegisterDTO request, string email)
        {
            var res = _repository.CreateClient(request,email);

            return Task.FromResult(res);
        }

        public Task<string> LoginClient(AdvisorLoginDTO request)
        {
            var res = _repository.LoginClient(request);
            return Task.FromResult(res);
        }
    }
}
