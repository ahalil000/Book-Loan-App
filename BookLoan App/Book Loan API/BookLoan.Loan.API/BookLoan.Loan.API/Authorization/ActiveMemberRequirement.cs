using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BookLoan.Authorization
{
    public class ActiveMemberRequirement: IAuthorizationRequirement    
    {
        public bool _IsActiveMember { get; }

        public ActiveMemberRequirement(bool IsActiveMember)
        {
            _IsActiveMember = IsActiveMember;
        }

    }
}
