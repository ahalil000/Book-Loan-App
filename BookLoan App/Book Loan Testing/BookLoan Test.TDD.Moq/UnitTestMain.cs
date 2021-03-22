using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using BookLoan.Models;
using BookLoan.Data;
using BookLoan.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using BookLoanTest.TDD.Moq;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        //private IBookService bookService;
        //private ILoanService loanService;
        private IConfiguration configuration;
        private ServiceCollection services;
        private DbContextOptions<ApplicationDbContext> opts;
        private ApplicationDbContext context;
        private ServiceProvider serviceProvider;
        private IHttpContextAccessor contextAccessor;
        //private UserManager<ApplicationUser> userManager;
   

        [SetUp]
        public void Setup()
        {
            contextAccessor = new HttpContextAccessor();
            //userManager = new UserManager<ApplicationUser>(context);

            services = new ServiceCollection();

            opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: "In_Memory_Db")
                            .Options;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("In_Memory_Db"));

            serviceProvider = services.BuildServiceProvider();
            context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<ILoanService, LoanService>();

            //bookService = serviceProvider.GetService<IBookService>();
            //loanService = serviceProvider.GetService<ILoanService>();

        }

        [TearDown]
        public void Cleanup()
        {
            // code to cleanup resources after each test
        }

        [Test]
        public void ListBooksTest() 
        {
            // setup mock classes
            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var mockDataContext = new Mock<MockApplicationDbContext>();
            var mockBookService = new Mock<MockBookService>();

            // setup properties
            mockBookService.SetupAllProperties();
            mockBookService.SetupProperty(f => f.bookCount, 0);

            // setup first record insertion
            mockDataContext.Setup(p => p.Add(book)).
                Callback(() => mockBookService.Object.bookCount++);
            mockDataContext.Setup(p => p.SaveChanges());

            // action insertion and save record
            mockDataContext.Object.Add(book);
            mockDataContext.Object.SaveChanges();

            book = new BookViewModel() { ID = 2, Author = "D.Lee", Title = "Red Mine", YearPublished = 1968 };

            // setup second record insertion
            mockDataContext.Setup(p => p.Add(book)).
                Callback(() => mockBookService.Object.bookCount++);
            mockDataContext.Setup(p => p.SaveChanges());

            // action insertion and save record
            mockDataContext.Object.Add(book);
            mockDataContext.Object.SaveChanges();

            book = new BookViewModel() { ID = 3, Author = "V.Prescott", Title = "Boundary Rider", YearPublished = 1974 };

            // setup third record insertion
            mockDataContext.Setup(p => p.Add(book)).
                Callback(() => mockBookService.Object.bookCount++);
            mockDataContext.Setup(p => p.SaveChanges());

            // action insertion and save record
            mockDataContext.Object.Add(book);
            mockDataContext.Object.SaveChanges();

            // retrieve record count property
            var bookCount = mockBookService.Object.bookCount;

            // test result
            Assert.AreEqual(3, bookCount);
        }

        [Test]
        public void AddAndDeleteBookTest()
        {
            // setup mock classes
            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var mockDataContext = new Mock<MockApplicationDbContext>(MockBehavior.Strict);
            var mockBookService = new Mock<MockBookService>();

            var sequence = new MockSequence();

            mockDataContext.InSequence(sequence).Setup(x => x.Add(book)).Callback(
                () => mockBookService.Object.bookCount++).Returns(1);
            mockDataContext.InSequence(sequence).Setup(x => x.Delete(book)).Callback(
                () => mockBookService.Object.bookCount--).Returns(1);
            mockDataContext.InSequence(sequence).Setup(x => x.SaveChanges()).Returns(1);

            // action insertion, deletion and save record
            mockDataContext.Object.Add(book);
            mockDataContext.Object.Delete(book);
            mockDataContext.Object.SaveChanges();

            mockDataContext.VerifyAll();

            // retrieve record count property
            var bookCount = mockBookService.Object.bookCount;

            // test result
            Assert.AreEqual(0, bookCount);
        }


        [Test]
        public void AddAndDeleteBookTestFail()
        {
            // setup mock classes
            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var mockDataContext = new Mock<MockApplicationDbContext>(MockBehavior.Strict);
            var mockBookService = new Mock<MockBookService>();

            var sequence = new MockSequence();

            mockDataContext.InSequence(sequence).Setup(x => x.Add(book)).Callback(
                () => mockBookService.Object.bookCount++).Returns(1);
            mockDataContext.InSequence(sequence).Setup(x => x.Delete(book)).Callback(
                () => mockBookService.Object.bookCount--).Returns(1);
            mockDataContext.InSequence(sequence).Setup(x => x.SaveChanges()).Returns(1);

            // action insertion, deletion and save record
            try
            {
                mockDataContext.Object.Add(book);
                mockDataContext.Object.SaveChanges();
                mockDataContext.Object.Delete(book);

                Assert.Fail();
            }
            catch (MockException ex)
            {
                Assert.Pass();
            }

            //mockDataContext.VerifyAll();

            // retrieve record count property
            //var bookCount = mockBookService.Object.bookCount;

            // test result
            //Assert.AreEqual(0, bookCount);
        }


        [Test]
        public void FilterBooksTest()
        {
            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            context.Add(book);
            context.SaveChanges();

            book = new BookViewModel() { ID = 2, Author = "D.Lee", Title = "Red Mine", YearPublished = 1968 };
            context.Add(book);
            context.SaveChanges();

            book = new BookViewModel() { ID = 3, Author = "V.Prescott", Title = "Boundary Rider", YearPublished = 1974 };
            context.Add(book);
            context.SaveChanges();

            var bookService = new BookService(context, null);
            var books = bookService.GetBooksFilter("Rivers").GetAwaiter();
            var result = books.GetResult();
            Assert.AreEqual(1, result.Count);

            Assert.Pass();
        }


        [Test]
        public void AddNewBookTest()
        {
            var bookService = new BookService(context, null);

            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var bookSave = bookService.SaveBook(book).GetAwaiter();
            int bookCount = context.Books.CountAsync().GetAwaiter().GetResult();
            Assert.AreEqual(1, bookCount);
                
            Assert.Pass();
        }


        [Test]
        public void UpdateBookTest()
        {
            var bookService = new BookService(context, null);

            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var bookSaveAction = bookService.SaveBook(book).GetAwaiter();
            bookSaveAction.GetResult();

            var bookGet = bookService.GetBook(1).GetAwaiter();
            var bookModified = bookGet.GetResult();
            bookModified.Title = "Rivers Run Dry 2";

            var bookUpdateAction = bookService.UpdateBook(1, bookModified).GetAwaiter();
            bookGet = bookService.GetBook(1).GetAwaiter();
            var bookLastUpdated = bookGet.GetResult();

            Assert.AreEqual(bookLastUpdated.Title, bookModified.Title); 

            Assert.Pass();
        }


        [Test]
        public void CreateNewLoanTest()
        {
            var bookService = new BookService(context, null);
            var loanService = new LoanService(context, null, contextAccessor, bookService, null, null);

            // create book
            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var bookSaveAction = bookService.SaveBook(book).GetAwaiter();
            bookSaveAction.GetResult();

            // generate and post a sample loan.
            LoanViewModel lvm = loanService.CreateNewBookLoan(book.ID);
            lvm.DateLoaned = DateTime.Now;
            lvm.DateDue = DateTime.Now.AddDays(7);
            lvm.LoanedBy = "bill@aaa.com";
            var loanSaveAction = loanService.SaveLoan(lvm).GetAwaiter();
            loanSaveAction.GetResult();

            // test the book was saved as an existing loan.
            var loanListAction = loanService.GetBookLoans(book.ID).GetAwaiter();
            List<LoanViewModel> loanListResult = loanListAction.GetResult();
            Assert.AreEqual(loanListResult.Count, 1);

            // test loan status.
            var loanStatusAction = loanService.GetBookLoanStatus(book.ID).GetAwaiter();
            BookStatusViewModel loanStatusReult = loanStatusAction.GetResult();
            Assert.AreEqual(loanStatusReult.Status, "On Loan");

            // return loan.
            var loanReturnAction = loanService.GetReturnLoan(book.ID).GetAwaiter();
            (LoanViewModel, BookViewModel) loanReturnResult = loanReturnAction.GetResult();
            Assert.IsTrue(loanReturnResult.Item1.DateReturn > lvm.DateLoaned);
            Assert.AreEqual(loanReturnResult.Item1.LoanedBy, "");
            Assert.AreEqual(loanReturnResult.Item1.DateDue.DayOfYear, lvm.DateDue.DayOfYear);

            // get loan details
            var loanDetailAction = loanService.GetLoan(lvm.ID).GetAwaiter();
            LoanViewModel loanDetailResult = loanDetailAction.GetResult();
            loanDetailResult.DateReturn = DateTime.Now;

            // post loan return data
            var loanUpdateAction = loanService.UpdateLoan(loanDetailResult).GetAwaiter();
            loanUpdateAction.GetResult();

            // re-test loan status
            var loanReturnStatusAction = loanService.GetBookLoanStatus(book.ID).GetAwaiter();
            BookStatusViewModel loanReturnStatusResult = loanReturnStatusAction.GetResult();
            Assert.AreEqual(loanReturnStatusResult.Status, "Available");

            Assert.Pass();
        }




    }
}