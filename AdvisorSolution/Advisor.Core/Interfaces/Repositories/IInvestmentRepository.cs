using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Interfaces.Repositories
{
    public interface IInvestmentRepository
    {
        InvestmentDTO CreateInvestment(InvestmentDTO request,string email);
        InvestmentDTO UpdateInvestment(InvestmentDTO request);
/*        List<InvestmentStrategy>? GetInvestment(int InvestmentStrategyId);*/
    }
}
