using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLoan.Models
{
    // Update Status Model From User Update
    public class UpdateResponseModel 
    {
        public bool IsSuccessful { get; set; }

        public List<UpdateErrorResponseModel> errors { get; set; }
        public UpdateUserResponseModel userinfo { get; set; }

        public UpdateResponseModel()
        {
            errors = new List<UpdateErrorResponseModel>();
            userinfo = new UpdateUserResponseModel();
        }
    }
}
