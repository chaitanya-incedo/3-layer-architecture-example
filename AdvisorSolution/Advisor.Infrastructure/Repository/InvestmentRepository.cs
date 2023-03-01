using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;

namespace Advisor.Infrastructure.Repository
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private static Random random = new Random();

        public InvestmentRepository(AdvisorDbContext context, IHttpContextAccessor httpContext)
        {
            _context=context;
            _httpContext = httpContext;
        }
        public string AccountIDGenerator() {
            const string chars = "A1BC2DE3FG5H6I7J4K8L9MN0OPQRSTUVWXYZ";
            var newId = "A" + new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            var res = _context.InvestmentStrategies.Any(u => u.AccountID == newId);
            if (res == true)
            {
                return AccountIDGenerator();
            }
            return newId;
        }
        public InvestmentDTO CreateInvestment(InvestmentDTO request,string email)
        {
            var advisor = _context.Users.First(X => X.Email == email);

            InvestmentStrategy strategy = new InvestmentStrategy();
            InvestorInfo info=new InvestorInfo();

            var client = _context.Users.First(x => x.ClientID == request.clientID);
            info.UserID=client.UserID;
            info.InvestmentName=request.InvestmentName;
            info.Active=request.Active;
            info.CreatedDate=DateTime.Now;
            info.ModifiedBy = advisor.AdvisorID;
            info.ModifiedDate=DateTime.Now;
            info.DeletedFlag = 0;
            _context.InvestorInfos.Add(info);
            _context.SaveChanges();

            var type = _context.InvestmentTypes.First(x => x.InvestmentTypeName == request.InvestmentTypeName);
            

            strategy.ModifiedDate=DateTime.Now;
            strategy.DeletedFlag= 0;
            strategy.ModifiedBy= advisor.AdvisorID;
            strategy.InvestmentAmount=request.InvestmentAmount;
            strategy.ModelAPLID=request.ModelAPLID;
            strategy.AccountID = AccountIDGenerator();
            strategy.StrategyName=request.StrategyName;
            strategy.InvestmentTypeID = type.InvestmentTypeID;
            strategy.InvestorInfoID=info.InvestorInfoID;
            _context.InvestmentStrategies.Add(strategy);
            _context.SaveChanges();

            return request;
            
        }

        public string DeleteInvestment(int strategyid)
        {
            var strategy=_context.InvestmentStrategies.FirstOrDefault(x=>x.InvestmentStrategyID==strategyid);
            strategy.DeletedFlag = 1;
            int id = strategy.InvestorInfoID;
            var info = _context.InvestorInfos.FirstOrDefault(x => x.InvestorInfoID == id);
            info.DeletedFlag = 1;
            _context.InvestmentStrategies.Update(strategy);
            _context.InvestorInfos.Update(info);
            _context.SaveChanges(true);
            return "deleted";
        }

        public List<InvestmentDTO> GetInvestment(string clientid)
        {
            var client = _context.Users.First(x => x.ClientID == clientid);
            List<int> infoIDS= new List<int>();
            List<InvestorInfo> infos= new List<InvestorInfo>();
            infos=_context.InvestorInfos.Where(x=>x.UserID==client.UserID).ToList();

            List<InvestmentStrategy> strategies= new List<InvestmentStrategy>();
            foreach (var i in infos) {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine(i.InvestorInfoID);
                var x = _context.InvestmentStrategies.First(x => x.InvestorInfoID == i.InvestorInfoID);
                if (x.DeletedFlag == 1)
                    continue;
                strategies.Add(x);
            }
            List<InvestmentDTO> res= new List<InvestmentDTO>();
            foreach (var s in strategies) {
                var strategy = _context.InvestmentStrategies.FirstOrDefault(X => X.InvestmentStrategyID == s.InvestmentStrategyID);
                var info = _context.InvestorInfos.FirstOrDefault(X => X.InvestorInfoID == strategy.InvestorInfoID);
                var type = _context.InvestmentTypes.FirstOrDefault(X => X.InvestmentTypeID == strategy.InvestmentTypeID);
                InvestmentDTO ret = new InvestmentDTO();
                ret.InvestmentName = info.InvestmentName;
                ret.InvestmentTypeName = type.InvestmentTypeName;
                ret.AccountID = strategy.AccountID;
                ret.ModelAPLID = strategy.ModelAPLID;
                ret.Active = info.Active;
                ret.strategyid = strategy.InvestmentStrategyID;
                ret.StrategyName = strategy.StrategyName;
                ret.InvestmentAmount = strategy.InvestmentAmount;
                res.Add(ret);
            }
            return res;
        }

        public decimal GetTotal(string clientID)
        {
            int userid = _context.Users.First(x => x.ClientID == clientID).UserID;
            List<InvestorInfo> infos=_context.InvestorInfos.Where(x=>x.UserID==userid).ToList();
            decimal total = 0;
            foreach (var i in infos) {
                if (i.DeletedFlag == 1)
                    continue;
                var strat = _context.InvestmentStrategies.First(x => x.InvestorInfoID == i.InvestorInfoID);
                if (strat.DeletedFlag == 1)
                    continue;
                total += strat.InvestmentAmount;
            }
            return total;
        }

        public InvestmentDTO UpdateInvestment(InvestmentDTO request)
        {
            var oldstrategy = _context.InvestmentStrategies.FirstOrDefault(x => x.InvestmentStrategyID == request.strategyid);
            var time = DateTime.Now;
            oldstrategy.ModifiedDate = time;
            oldstrategy.DeletedFlag = 0;
            oldstrategy.InvestmentAmount = request.InvestmentAmount;
            oldstrategy.ModelAPLID = request.ModelAPLID;
            oldstrategy.AccountID = request.AccountID;
            oldstrategy.StrategyName = request.StrategyName;
            var newtype = _context.InvestmentTypes.FirstOrDefault(X => X.InvestmentTypeName == request.InvestmentTypeName);
            oldstrategy.InvestmentTypeID = newtype.InvestmentTypeID;

            var oldinfo = _context.InvestorInfos.FirstOrDefault(X => X.InvestorInfoID == oldstrategy.InvestorInfoID);

            oldinfo.InvestmentName = request.InvestmentName;
            oldinfo.Active = request.Active;
            oldinfo.ModifiedDate = time;
            oldinfo.DeletedFlag = 0;
            _context.InvestorInfos.Update(oldinfo);
            _context.SaveChanges();
     
            _context.InvestmentStrategies.Update(oldstrategy);
            _context.SaveChanges();

            return request;
        }
    }
}
