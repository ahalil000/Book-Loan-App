using System;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLoan.Data;
//using BookLoan.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BookLoan.Models;

using BookLoanMicroservices.Messaging;
using BookLoanMicroservices.Models.MessagingModels;

using BookLoan.Catalog.API.Events;
using BookLoan.Catalog.API.Helpers;

using Microsoft.Data.SqlClient;


namespace BookLoan.Services
{
    public class BookService: IBookService
    {
        readonly ApplicationDbContext _db;
        private readonly ILogger _logger;

        private readonly IEventBus _eventBus;
        //private MessageBusHelper messageBusHelper;


        public BookService(ApplicationDbContext db,
            ILogger<BookService> logger,
            IEventBus eventBus)
        {
            _db = db;
            _logger = logger;
            _eventBus = eventBus;
            //messageBusHelper = new MessageBusHelper();
        }



        /// <summary>
        /// GetBooks()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooks()
        {
            return await _db.Books.ToListAsync();
        }


        /// <summary>
        /// GetBooksFilter()
        /// </summary>
        /// <returns></returns>
        public async Task<List<BookViewModel>> GetBooksFilter(string filter)
        {
            return await _db.Books.Where(b => b.Title.Contains(filter)).ToListAsync();
        }


        // GET: LoanViewModels/GetBookLoanStatus/5

        /// <summary>
        /// GetBook()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BookViewModel> GetBook(int id)
        {
            BookLoan.Models.BookViewModel book = await _db.Books.
                Where(m => m.ID == id).SingleOrDefaultAsync();
            if (book != null)
                return book;
            return null;
        }


        /// <summary>
        /// ApplyPatching()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task ApplyPatching(PatchBookViewModel vm)
        {
            BookLoanComparer bookLoanComparer = new BookLoanComparer();            

            if (vm.books.Count > 0)
            {
                foreach (BookViewModel book in vm.books)
                {
                    BookLoan.Models.BookViewModel bookoriginal = await _db.Books.
                        Where(m => m.ID == book.ID).SingleOrDefaultAsync();
                    if (bookoriginal != null)
                    {
                        // If different record then update
                        if (!bookLoanComparer.Equals(bookoriginal, book))
                        {
                            await this.UpdateBook(book.ID, book);
                        }
                    }
                    else
                    {
                        // If a new record then insert
                        if (book.ID == 0)
                        {
                            await this.SaveBook(book);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// SaveBook()
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task SaveBook(BookViewModel vm)
        { 
            _db.Add(vm);
            vm.DateCreated = DateTime.Now;

            await _db.SaveChangesAsync();

            var book = _db.Books.Where(a => a.Title == vm.Title).SingleOrDefault();
            if (book == null)
            {
                return;
            }

            int id = book.ID;

            // send integration event to notify other microservices.
            //BookRecordSyncModel bookRecordSyncData = new BookRecordSyncModel()
            //{
            //    CommandType = "Update",
            //    bookData = vm
            //};

            //await messageBusHelper.InitSendMessages(bookRecordSyncData);
            BookInventoryNewIntegrationEvent bookInventoryNewIntegrationEvent
                = new BookInventoryNewIntegrationEvent(id, vm);
            await _eventBus.Publish(bookInventoryNewIntegrationEvent);
            await Task.Delay(2000);
            //await _eventBus.Unsubscribe(BookInventoryNewIntegrationEvent>();
            //messageBusHelper.MessageSentEvent.WaitOne(20000);
            //messageBusHelper.ReceiveMessageHandler += ProcessMessagesAsync;
            //await messageBusHelper.InitReceiveMessages();
            //await Task.Delay(2000);
            //messageBusHelper.MessageReceivedEvent.WaitOne(20000);
            //if (!messageBusHelper.queueClient.IsClosedOrClosing)
            //    await messageBusHelper.queueClient.CloseAsync();


        }

        /// <summary>
        /// UpdateBook()
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<BookViewModel> UpdateBook(int Id, BookViewModel vm)
        {
            BookViewModel book = await _db.Books.Where(a => a.ID == Id).SingleOrDefaultAsync();
            if (book != null)
            {
                string originalEdition = book.Edition;
                book.Title = vm.Title;
                book.Author = vm.Author;
                book.Edition = vm.Edition;
                book.Genre = vm.Genre;
                book.ISBN = vm.ISBN;
                book.Location = vm.Location;
                book.YearPublished = vm.YearPublished;
                book.DateUpdated = DateTime.Now;
                _db.Update(book);
                await _db.SaveChangesAsync();

                // Detect a changed field.
                if (vm.Edition != originalEdition)
                {
                    BookInventoryChangedIntegrationEvent bookInventoryChangedIntegrationEvent
                        = new BookInventoryChangedIntegrationEvent(Id, book.Edition, originalEdition);                  
                    await _eventBus.Publish(bookInventoryChangedIntegrationEvent);
                    await Task.Delay(2000);
                }
            }
            return book;
        }

        public async Task<List<BookViewModel>> GetNLatestBooksV1(int numberBooks)
        {
            var latestNBooks = await _db.Books
                .FromSqlRaw("SELECT TOP " + numberBooks.ToString() + " * " +
                            " FROM Books ")
                .OrderByDescending(b => b.DateCreated)
                .ToListAsync();
            return latestNBooks;
        }

        public async Task<List<BookViewModel>> GetNLatestBooksV2(int numberBooks)
        {
            var latestNBooks = await _db.Books
                .FromSqlInterpolated(
                    $"SELECT * FROM Books")
                .OrderByDescending(b => b.DateCreated)
                .Take(numberBooks)
                .ToListAsync();
            return latestNBooks;
        }

        public async Task<List<BookViewModel>> GetNLatestBooksV3(int numberBooks)
        {
            var latestNBooks = await _db.Books
                .FromSqlRaw("EXECUTE GetNLatestBooks {0}", numberBooks)
                .ToListAsync();
            return latestNBooks;
        }

        public async Task<List<BookViewModel>> GetNLatestBooksV4(int numberBooks)
        {
            var latestNBooks = await _db.Books
                .FromSqlInterpolated($"EXECUTE GetNLatestBooks {numberBooks}")
                .ToListAsync();
            return latestNBooks;
        }

        public async Task<List<BookViewModel>> GetNLatestBooksV5(int numberBooks)
        {
            SqlParameter nthLatestParam = new SqlParameter("latestNBooks", numberBooks);

            var latestNBooks = await _db.Books
                .FromSqlRaw("EXECUTE GetNLatestBooks @latestNBooks", nthLatestParam)
                .ToListAsync();
            return latestNBooks;
        }
    }
}
