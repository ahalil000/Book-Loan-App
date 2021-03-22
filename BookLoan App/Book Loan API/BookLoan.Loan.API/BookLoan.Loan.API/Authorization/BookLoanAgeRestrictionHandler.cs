using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BookLoan.Authorization;
using BookLoan.Models;
using BookLoan.Services;

namespace BookLoan.Authorization
{
    public class BookLoanAgeRestrictionHandler : AuthorizationHandler<MinimumAgeRequirement, BookViewModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       MinimumAgeRequirement requirement,
                                                       BookViewModel resource)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            if (resource.Genre.Contains("Adult"))
            {
                if (calculatedAge >= requirement.MinimumAge)
                {
                    context.Succeed(requirement);
                }
            }
            else
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
