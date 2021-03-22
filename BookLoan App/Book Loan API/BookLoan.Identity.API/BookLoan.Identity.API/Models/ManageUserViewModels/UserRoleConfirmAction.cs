using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models.ManageUserViewModels
{
    public class UserRoleConfirmAction
    {
        public string UserID { get; set; }

        public string LoginName { get; set; }

        public string DisplayName { get; set; }

        public string Action { get; set; }

        public string SelectedRole { get; set; }
    }
}
