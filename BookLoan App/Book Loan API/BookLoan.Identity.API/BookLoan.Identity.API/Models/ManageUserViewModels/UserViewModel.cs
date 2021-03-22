using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookLoan.Models.ManageUserViewModels
{
    public class UserViewModel
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        // custom personal fields
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string Password { get; set; }
    }
}
