using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoanEventHubSenderConsoleApp
{
    [Serializable]
    public class LoginAuditViewModel
    {
        public int ID { get; set; }

        [Required]
        public string UserLogin { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public DateTime WhenLoggedIn { get; set; }

        public LoginAuditViewModel() { }
    }
}

