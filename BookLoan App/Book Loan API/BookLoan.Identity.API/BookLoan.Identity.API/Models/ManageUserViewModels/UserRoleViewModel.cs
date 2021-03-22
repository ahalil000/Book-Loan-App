using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookLoan.Models.ManageUserViewModels
{
    public class UserRoleViewModel
    {
        public string UserID { get; set; }

        public string LoginName { get; set; }

        public string DisplayName { get; set; }

        public List<string> UserRoles { get; set; }

        public List<string> AvailableRoles { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string UserRolesList
        {
            get
            {
                string val = "";
                if (UserRoles.Count == 0)
                    return "None Assigned";
                UserRoles.ForEach(c => 
                {
                    val = val + c.ToString() + ",";
                });
                return val.TrimEnd(',');
            }

        }

        public IEnumerable<SelectListItem> AvailableRolesList {
            get
            {
                IEnumerable<SelectListItem> items = AvailableRoles.Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });
                return items;
            }
        }

    }
}

