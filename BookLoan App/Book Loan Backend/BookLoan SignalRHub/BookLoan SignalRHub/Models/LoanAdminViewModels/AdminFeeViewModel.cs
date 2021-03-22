using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoan.Models
{
    public class AdminFeeViewModel
    {
        public int ID { get; set; }

        public decimal DailyOverdueFee { get; set; }

        public decimal LostBookFee { get; set; }
    }
}
