
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BookLoanHttpFunctionApp
{
    public static class OverdueBookHttpFunction
    {
        [FunctionName("OverdueBookHttpFunction")]
        public static IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "get", "post", 
                Route = "Loan/{id?}")]HttpRequest req, 
            int? id, 
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            int? loanId = id ?? 0;

            return id != null
                ? (ActionResult)new OkObjectResult($"The Loan {id} is overdue. Processing notice for borrower.")
                : new BadRequestObjectResult("Please pass in a valid Loan ID.");
        }
    }
}
