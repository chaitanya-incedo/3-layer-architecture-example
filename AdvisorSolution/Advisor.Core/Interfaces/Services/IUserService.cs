using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<RegistrationDTO?> CreateClient(RegistrationDTO request, string email);
        Task<RegistrationDTO?> CreateAdvisor(RegistrationDTO request);
    }
}
