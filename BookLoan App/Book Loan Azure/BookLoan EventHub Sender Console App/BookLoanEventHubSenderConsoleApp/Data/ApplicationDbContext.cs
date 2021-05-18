using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BookLoanEventHubSenderConsoleApp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LoginAuditViewModel>().ToTable("LoginAudits");
            builder.Entity<SamplingWindowViewModel>().ToTable("SamplingWindows");
        }

        public DbSet<LoginAuditViewModel> LoginAudits { get; set; }
        public DbSet<SamplingWindowViewModel> SamplingWindows { get; set; }
    }
}
