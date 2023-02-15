using Advisor.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        public DbSet<AdvisorClient> AdvisorClients { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasMany(t => t.AdvisorsList).WithOne(m=>m.Advisor).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Users>().HasMany(t => t.ClientList).WithOne(m => m.Client).OnDelete(DeleteBehavior.NoAction);

        }
    }

}
