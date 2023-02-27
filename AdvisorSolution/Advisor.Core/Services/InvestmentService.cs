﻿using Advisor.Core.Domain.DTOs;
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
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvestmentRepository _repository;

        public InvestmentService(IInvestmentRepository repository)
        {
            _repository = repository;
        }
        public Task<InvestmentDTO> CreateInvestment(InvestmentDTO request,string email)
        {
            var res = _repository.CreateInvestment(request,email);
            return Task.FromResult(res);
        }

        public Task<string> DeleteInvestment(int strategyid)
        {
            var res = _repository.DeleteInvestment(strategyid);
            return Task.FromResult(res);
        }

        public Task<List<InvestmentDTO>> GetInvestment(string InvestmentStrategyId)
        {
            var res = _repository.GetInvestment(InvestmentStrategyId);
            return Task.FromResult(res);
        }

        public Task<decimal> GetTotal(string clientID)
        {
            var res = _repository.GetTotal(clientID);
            return Task.FromResult(res);
        }

        public Task<InvestmentDTO> UpdateInvestment(InvestmentDTO request)
        {
            var res = _repository.UpdateInvestment(request);
            return Task.FromResult(res);
        }
    }
}
