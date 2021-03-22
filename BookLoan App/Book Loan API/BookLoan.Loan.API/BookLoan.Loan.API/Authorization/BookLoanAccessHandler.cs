using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using BookLoan.Services;

namespace BookLoan.Authorization
{
    public class BookLoanAccessHandler : AuthorizationHandler<BookLoanRequirement, ReportService>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       BookLoanRequirement requirement,
                                                       ReportService resource)
        {
            Task<bool> anyOverdueLoansResult = resource.CurrentUserAnyOverdueLoans();
            bool anyOverdueLoans = anyOverdueLoansResult.GetAwaiter().GetResult();

            if (!anyOverdueLoans)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
