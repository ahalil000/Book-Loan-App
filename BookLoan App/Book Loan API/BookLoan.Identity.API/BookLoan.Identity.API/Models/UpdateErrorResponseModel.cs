﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLoan.Models
{
    // Response Model From User Update
    public class UpdateErrorResponseModel 
    {
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}
