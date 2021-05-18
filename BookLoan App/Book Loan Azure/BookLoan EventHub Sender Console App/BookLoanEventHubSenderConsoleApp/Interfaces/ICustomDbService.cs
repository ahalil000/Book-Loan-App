using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLoanEventHubSenderConsoleApp
{
    public interface ICustomDbService
    {
        public Task<List<LoginAuditViewModel>> RetrieveNextAuditEntries();

        public void ReadSamplingWindow();

        public Task UpdateSamplingWindow();

        public int GetResult { get;  }
    }
}
