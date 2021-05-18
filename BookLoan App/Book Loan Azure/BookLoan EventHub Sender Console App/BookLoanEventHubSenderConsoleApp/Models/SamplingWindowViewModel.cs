using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoanEventHubSenderConsoleApp
{
    [Serializable]
    public class SamplingWindowViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public DateTime StartSampleDateTime { get; set; }

        [Required]
        public DateTime EndSampleDateTime { get; set; }

        public SamplingWindowViewModel() { }
    }
}

