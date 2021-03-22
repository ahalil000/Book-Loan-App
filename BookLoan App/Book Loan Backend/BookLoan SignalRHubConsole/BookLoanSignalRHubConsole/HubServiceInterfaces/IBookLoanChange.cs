using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HubServiceInterfaces
{
#region IBookLoanChange
    public interface IBookLoanChange
    {
        Task SendBookCatalogChanges(int numberBookChanges);
    }
#endregion
}
