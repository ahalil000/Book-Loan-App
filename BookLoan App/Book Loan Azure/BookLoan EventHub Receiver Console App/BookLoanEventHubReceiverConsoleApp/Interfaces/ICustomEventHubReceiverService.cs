using System.Threading.Tasks;

namespace BookLoanEventHubReceiverConsoleApp
{
    public interface ICustomEventHubReceiverService
    {
        public Task ProcessEventHubEvents();
    }
}
