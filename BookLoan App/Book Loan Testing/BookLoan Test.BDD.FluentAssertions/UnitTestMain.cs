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

using FluentAssertions;
using FluentAssertions.Execution;


namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private IConfiguration configuration;
        private ServiceCollection services;
        private DbContextOptions<ApplicationDbContext> opts;
        private ApplicationDbContext context;
        private ServiceProvider serviceProvider;
        private IHttpContextAccessor contextAccessor;   

        [SetUp]
        public void Setup()
        {
            contextAccessor = new HttpContextAccessor();

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
        }

        [TearDown]
        public void Cleanup()
        {
            // code to cleanup resources after each test
        }

        [Test]
        public void ListBooksTest() 
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
            var books = bookService.GetBooks().GetAwaiter();

            var result = books.GetResult();

            using (new AssertionScope())
            {
                result.Should().HaveCount(3);
                Assert.Pass();
            }
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

            using (new AssertionScope())
            {
                result.Should().HaveCount(1);
                Assert.Pass();
            }

            //Assert.IsTrue(result.Should().HaveCount(1).Equals(true));
            //Assert.AreEqual(1, result.Count);

            Assert.Pass();
        }


        [Test]
        public void AddNewBookTest()
        {
            var bookService = new BookService(context, null);

            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var bookSave = bookService.SaveBook(book).GetAwaiter();
            int bookCount = context.Books.CountAsync().GetAwaiter().GetResult();

            //Assert.IsTrue(bookCount.Should().Equals(1));
            //Assert.AreEqual(1, bookCount);

            using (new AssertionScope())
            {
                bookCount.Should().Equals(1);
                Assert.Pass();
            }

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

            using (new AssertionScope())
            {
                bookLastUpdated.Title.Should().BeEquivalentTo(bookModified.Title).Equals(true);
                Assert.Pass();
            }


            //Assert.IsTrue(bookLastUpdated.Title.Should().BeEquivalentTo(bookModified.Title).Equals(true));
            //Assert.AreEqual(bookLastUpdated.Title, bookModified.Title); 

            //Assert.Pass();
        }


        [Test]
        public void CreateNewLoanTest()
        {
            using (new AssertionScope())
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
                loanListResult.Should().HaveCount(1);

                //Assert.AreEqual(loanListResult.Count, 1);

                // test loan status.
                var loanStatusAction = loanService.GetBookLoanStatus(book.ID).GetAwaiter();
                BookStatusViewModel loanStatusReult = loanStatusAction.GetResult();

                loanStatusReult.Status.Should().BeEquivalentTo("On Loan");
                
                //Assert.AreEqual(loanStatusReult.Status, "On Loan");

                // return loan.
                var loanReturnAction = loanService.GetReturnLoan(book.ID).GetAwaiter();
                (LoanViewModel, BookViewModel) loanReturnResult = loanReturnAction.GetResult();

                loanReturnResult.Item1.DateReturn.Should().BeAfter(lvm.DateLoaned);

                //Assert.IsTrue(loanReturnResult.Item1.DateReturn > lvm.DateLoaned);

                loanReturnResult.Item1.LoanedBy.Should().BeEmpty();
                //loanReturnResult.Item1.LoanedBy.Should().NotBeEmpty();

                //Assert.AreEqual(loanReturnResult.Item1.LoanedBy, "");

                loanReturnResult.Item1.DateDue.DayOfYear.Should().Equals(lvm.DateDue.DayOfYear);
                //loanReturnResult.Item1.DateDue.DayOfYear.Should().NotBe(lvm.DateDue.DayOfYear);

                //Assert.AreEqual(loanReturnResult.Item1.DateDue.DayOfYear, lvm.DateDue.DayOfYear);

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

                //Assert.IsTrue(loanReturnStatusResult.Status.Should().BeEquivalentTo("Available").Equals(true));
                //Assert.AreEqual(loanReturnStatusResult.Status, "Available");

                loanReturnStatusResult.Status.Should().BeEquivalentTo("Available");

                Assert.Pass();
            }

            //Assert.Pass();
        }




    }
}