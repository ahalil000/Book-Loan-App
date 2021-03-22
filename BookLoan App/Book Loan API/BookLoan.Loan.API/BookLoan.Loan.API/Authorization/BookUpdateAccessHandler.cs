using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using BookLoan.Services;

namespace BookLoan.Authorization
{
    public class BookUpdateAccessHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var user = context.User;

            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is ActiveMemberRequirement)
                {
                    if (!user.Identity.IsAuthenticated)
                    {
                        context.Succeed(requirement);
                    }
                }
                if ((requirement == BookLoanOperations.Create) ||
                    (requirement == BookLoanOperations.Update))
                {
                    if (user.IsInRole("Admin"))
                        context.Succeed(requirement);
                }
                if (requirement == BookLoanOperations.Read)
                {
                    context.Succeed(requirement);
                }

            }
            return Task.CompletedTask;
        }
    }
}
