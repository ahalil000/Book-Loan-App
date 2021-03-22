namespace BookLoan.WebJob.Interfaces
{
    public interface ICustomDbService
    {
        public void RunDBProcess();
        public int GetResult { get;  }
    }
}
