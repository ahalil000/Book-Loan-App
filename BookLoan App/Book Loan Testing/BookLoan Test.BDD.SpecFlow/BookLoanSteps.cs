using System;
using TechTalk.SpecFlow;

using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.InMemory;
using BookLoan.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;

using BookLoan.Models;
using BookLoan.Services;

using System.Collections.Generic;
using NUnit.Framework;


namespace BookLoanTest.BDD.SpecFlow
{
    [Binding]
    public class BookLoanSteps
    {
        private IConfiguration configuration;
        private ServiceCollection services;
        private DbContextOptions<ApplicationDbContext> opts;
        private ApplicationDbContext context;
        private ServiceProvider serviceProvider;
        private IHttpContextAccessor contextAccessor;

        private List<BookViewModel> result;


        public BookLoanSteps()
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

        // Steps for scenario "List books"

        [Given(@"I have opened the book loan application")]
        public void GivenIHaveOpenedTheBookLoanApplication()
        {
            //ScenarioContext.Current.Pending();
        }

        [When(@"I list the books")]
        public void WhenIListTheBooks()
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
        }

        [Then(@"I see a list of books")]
        public void ThenISeeAListOfBooks()
        {
            var bookService = new BookService(context, null);
            var books = bookService.GetBooks().GetAwaiter();

            result = books.GetResult();
            Assert.AreEqual(3, result.Count);
        }

        // Steps for scenario "Add a new book"

        [Given(@"I have student access to the application")]
        public void GivenIHaveStudentAccessToTheApplication()
        {
            //ScenarioContext.Current.Pending();
        }

        [Given(@"I have added a new book record")]
        public void GivenIHaveAddedANewBookRecord()
        {
            var bookService = new BookService(context, null);

            var book = new BookViewModel() { ID = 1, Author = "W.Smith", Title = "Rivers Run Dry", YearPublished = 1954 };
            var bookSave = bookService.SaveBook(book).GetAwaiter();
        }

        [Given(@"I list the books")]
        public void GivenIListTheBooks()
        {
            //ScenarioContext.Current.Pending();
        }

        [Then(@"I will see at least one book")]
        public void ThenIWillSeeAtLeastOneBook()
        {
            int bookCount = context.Books.CountAsync().GetAwaiter().GetResult();
            Assert.AreEqual(1, bookCount);
        }



    }
}
