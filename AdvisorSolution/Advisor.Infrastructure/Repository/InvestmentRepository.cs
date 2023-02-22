using Advisor.Core.Domain.DTOs;
using Advisor.Core.Domain.Models;
using Advisor.Core.Interfaces.Repositories;
using Advisor.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Advisor.Infrastructure.Repository
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly AdvisorDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public InvestmentRepository(AdvisorDbContext context, IHttpContextAccessor httpContext)
        {
            _context=context;
            _httpContext = httpContext;
        }
        public InvestmentDTO CreateInvestment(InvestmentDTO request,string email)
        {
            var advisor = _context.Users.First(X => X.Email == email);

            InvestmentStrategy strategy = new InvestmentStrategy();
            InvestmentType type=new InvestmentType();
            InvestorInfo info=new InvestorInfo();

            info.UserID=request.UserID;
            info.InvestmentName=request.InvestmentName;
            info.Active=request.Active;
            info.CreatedDate=DateTime.Now;
            info.ModifiedBy = advisor.AdvisorID;
            info.ModifiedDate=DateTime.Now;
            info.DeletedFlag = 0;
            _context.InvestorInfos.Add(info);
            _context.SaveChanges();

            type.InvestmentTypeName=request.InvestmentTypeName;
            type.CreatedDate=DateTime.Now;
            type.ModifiedDate=DateTime.Now;
            type.ModifiedBy=advisor.AdvisorID;
            type.DeletedFlag= 0;
            _context.InvestmentTypes.Add(type);
            _context.SaveChanges();

            InvestmentType type1 = _context.InvestmentTypes.First(x=>x.InvestmentTypeName==request.InvestmentTypeName);
            InvestorInfo info1 = _context.InvestorInfos.First(s => s.UserID == request.UserID);

            strategy.ModifiedDate=DateTime.Now;
            strategy.DeletedFlag= 0;
            strategy.ModifiedBy= advisor.AdvisorID;
            strategy.InvestmentAmount=request.InvestmentAmount;
            strategy.ModelAPLID=request.ModelAPLID;
            strategy.AccountID=request.AccountID;
            strategy.StrategyName=request.StrategyName;
            strategy.InvestmentTypeID = type1.InvestmentTypeID;
            strategy.InvestorInfoID=info1.InvestorInfoID;
            _context.InvestmentStrategies.Add(strategy);
            _context.SaveChanges();

            return request;
            
        }

/*        public List<InvestmentStrategy>? GetInvestment(int InvestmentStrategyId)
        {
            if (!(_context.InvestmentStrategies.Any(X => X.InvestmentStrategyID == InvestmentStrategyId)))
                return null;
            InvestmentDTO ret = new InvestmentDTO();

            var pg = _context.InvestmentStrategies.Where(c => c.InvestmentStrategyID == InvestmentStrategyId).Include(c=>c.InvestorInfo).Include(c=>c.InvestmentTypes);
            ret.AccountID=p
        }*/

        public InvestmentDTO UpdateInvestment(int infoid,int typeid,int strategyid, InvestmentDTO request)
        {
            var oldinfo = _context.InvestorInfos.FirstOrDefault(X => X.InvestorInfoID == infoid);
            var time = DateTime.Now;

            oldinfo.InvestmentName = request.InvestmentName;
            oldinfo.Active = request.Active;
            oldinfo.ModifiedDate = time;
            oldinfo.DeletedFlag = 0;
            _context.InvestorInfos.Update(oldinfo);
            _context.SaveChanges();

            var oldtype = _context.InvestmentTypes.FirstOrDefault(X => X.InvestmentTypeID == typeid);
            oldtype.InvestmentTypeName = request.InvestmentTypeName;
            oldtype.ModifiedDate = time;
            oldtype.DeletedFlag = 0;
            _context.InvestmentTypes.Update(oldtype);
            _context.SaveChanges();

            InvestmentType type1 = _context.InvestmentTypes.First(x => x.InvestmentTypeID == typeid);
            InvestorInfo info1 = _context.InvestorInfos.First(s => s.InvestorInfoID == infoid);

            var oldstrategy = _context.InvestmentStrategies.FirstOrDefault(x => x.InvestmentStrategyID == strategyid);

            oldstrategy.ModifiedDate = time;
            oldstrategy.DeletedFlag = 0;
            oldstrategy.InvestmentAmount = request.InvestmentAmount;
            oldstrategy.ModelAPLID = request.ModelAPLID;
            oldstrategy.AccountID = request.AccountID;
            oldstrategy.StrategyName = request.StrategyName;
            oldstrategy.InvestmentTypeID = type1.InvestmentTypeID;
            oldstrategy.InvestorInfoID = info1.InvestorInfoID;
            _context.InvestmentStrategies.Update(oldstrategy);
            _context.SaveChanges();

            return request;
        }
    }
}
