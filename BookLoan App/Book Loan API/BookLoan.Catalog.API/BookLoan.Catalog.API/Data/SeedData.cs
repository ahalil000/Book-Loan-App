using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using BookLoan.Models;
using BookLoan.Data;
using BookLoan.Services;
//using Microsoft.Extensions.Options;

//using MembershipWebApp.Interfaces;

namespace BookLoan.Domain
{
    /// <summary>
    /// SeedData
    /// Use this class to generate once off or periodic regeneration of rendomized membership
    /// data for mocking or testing purposes.
    /// </summary>
    public class SeedData
    {
        private ApplicationDbContext db;
        private AppConfiguration appConfiguration;

        public SeedData(ApplicationDbContext _db, AppConfiguration _appConfiguration)
        {
            db = _db;
            appConfiguration = _appConfiguration;
        }

        public void GenerateAdminFees()
        {
            //if (db.AdminFees.Count() > 0)
            //    return;
            try
            {
                //AdminFeeViewModel avm = new AdminFeeViewModel()
                //{
                //    LostBookFee = appConfiguration.LostBookFee,
                //    DailyOverdueFee = appConfiguration.OverdueBookFee
                //};
                //db.Add(avm);
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
            }

        }

        public void GenerateBooks()
        {
            if (db.Books.Count() > 0)
                return;

            try
            {
                BookViewModel bvm = new BookViewModel()
                {
                    //ID = 1,
                    Title = "The Lord of the Rings",
                    Author = "J. R. R. Tolkien",
                    Genre = "fantasy",
                    YearPublished = 1954,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);
                bvm = new BookViewModel()
                {
                    //ID = 2,
                    Title = "The Alchemist (O Alquimista)",
                    Author = "Paulo Coelho",
                    Genre = "fantasy",
                    YearPublished = 1988,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 3,
                    Title = "The Little Prince (Le Petit Prince)",
                    Author = "Antoine de Saint-Exupéry",
                    Genre = "fantasy",
                    YearPublished = 1943,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 4,
                    Title = "Grimms' Fairy Tales (Kinder- und Hausmärchen)",
                    Author = "Jacob and Wilhelm Grimm",
                    Genre = "folklore",
                    YearPublished = 1812,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 5,
                    Title = "Harry Potter and the Philosopher's Stone",
                    Author = "J. K. Rowling",
                    Genre = "fantasy",
                    YearPublished = 1997,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 6,
                    Title = "The Hobbit",
                    Author = "J. R. R. Tolkien",
                    Genre = "fantasy",
                    YearPublished = 1937,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 7,
                    Title = "And Then There Were None",
                    Author = "Agatha Christie",
                    Genre = "mystery",
                    YearPublished = 1939,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);

                bvm = new BookViewModel()
                {
                    //ID = 8,
                    Title = "Dream of the Red Chamber (红楼梦)",
                    Author = "Cao Xueqin",
                    Genre = "family sage",
                    YearPublished = 1791,
                    Edition = "0",
                    ISBN = "",
                    Location = "sydney",
                    DateCreated = DateTime.Today,
                    DateUpdated = DateTime.Today
                };
                db.Add(bvm);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
            }
        }
    }
}
