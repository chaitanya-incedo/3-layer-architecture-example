using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Core.Domain.DTOs
{
    public class InvestmentDTO
    {
        public string clientID { get; set; }
        public string? InvestmentName { get; set; }
        public int Active { get; set; }
        //-------------------------------------------------------------
        public string? InvestmentTypeName { get; set; }
        //-------------------------------------------------------------
        public int strategyid { get; set; }
        public string? StrategyName { get; set; }
        public string? AccountID { get; set; }
        public string? ModelAPLID { get; set; }
        public decimal InvestmentAmount { get; set; }

    }
}
