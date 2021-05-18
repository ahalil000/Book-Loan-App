using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLoanEventHubSenderConsoleApp
{
    [Serializable]
    public class SamplingWindowConfiguration
    {
        public DateTime StartSampleDateTime { get; set; }

        public DateTime EndSampleDateTime { get; set; }

        public SamplingWindowConfiguration() { }
    }
}

