using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLoanEventHubSenderConsoleApp
{
    public interface ICustomEventHubSenderService
    {
        public Task SendEventBatch(List<LoginAuditViewModel> entries);
    }
}
