using Advisor.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advisor.Infrastructure.Data
{
    public class AdvisorDbContext : DbContext
    {
        public AdvisorDbContext(DbContextOptions<AdvisorDbContext> options) : base(options)
        {

        }
        public DbSet<AdvisorRegistrationDetails> AdvisorDetails { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<InvestorInfo> InvestorInfos { get; set; }
        public DbSet<InvestmentType> InvestmentTypes { get; set; }
        public DbSet<InvestmentStrategy> InvestmentStrategies { get; set; }
    }
}
