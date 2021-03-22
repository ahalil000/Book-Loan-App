using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BookLoan.WebJob.Interfaces;
using BookLoan.WebJob.Data;

namespace BookLoan.WebJob.Services
{
    public class CustomDBService: ICustomDbService 
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _db;

        private int _result;

        public int GetResult => _result;

        public CustomDBService(IConfiguration configuration, ILogger<CustomDBService> logger, ApplicationDbContext db)
        {
            this._logger = logger;
            this._db = db;
        }

        public void RunDBProcess()
        {
            Console.WriteLine("Running custom DB service within Web Job");
            _logger.LogInformation($"The BookLoan Web Job custom DB service has been run.");
            _result = _db.Books.Count();
            UpdateFirstBook();
        }

        private void UpdateFirstBook()
        {
            var book = _db.Books.Where(b => b.ID == 1).SingleOrDefault();
            if (book != null)
            {
                book.DateUpdated = DateTime.Now;
                _db.SaveChanges();
            }
        }
    }
}