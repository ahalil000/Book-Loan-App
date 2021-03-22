using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLoan.Models
{
    // Response Status Model From User Registration 
    public class RegistrationResponseModel 
    {
        public bool IsSuccessful { get; set; }

        public List<RegistrationErrorResponseModel> errors { get; set; }
        public RegistrationUserResponseModel userinfo { get; set; }

        public RegistrationResponseModel()
        {
            errors = new List<RegistrationErrorResponseModel>();
            userinfo = new RegistrationUserResponseModel();
        }
    }
}
