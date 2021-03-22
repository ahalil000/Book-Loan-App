using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookLoan.Models;

namespace BookLoan.Data
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

            builder.Entity<BookViewModel>().ToTable("Books");
            builder.Entity<LoanViewModel>().ToTable("Loans");
            builder.Entity<ReviewViewModel>().ToTable("Reviews");
            builder.Entity<OverdueLoanViewModel>().ToTable("OverdueLoans");
            builder.Entity<AdminFeeViewModel>().ToTable("AdminFees");
        }

        public DbSet<LoanViewModel> Loans { get; set; }
        public DbSet<ReviewViewModel> Reviews { get; set; }
        public DbSet<BookViewModel> Books { get; set; }
    }
}
