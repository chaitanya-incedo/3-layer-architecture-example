using Advisor.Core.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Services
{
    public interface IInvestmentService
    {
        Task<InvestmentDTO> CreateInvestment(InvestmentDTO request,string email);
        Task<InvestmentDTO> UpdateInvestment(InvestmentDTO request, string email);
        Task<InvestmentDTO> GetInvestment(InvestmentDTO request);
    }
}
