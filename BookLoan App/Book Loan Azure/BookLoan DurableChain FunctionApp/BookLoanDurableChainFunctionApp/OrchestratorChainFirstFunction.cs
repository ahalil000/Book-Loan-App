using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using BookLoan.Models;


namespace BookLoanDurableChainFunctionApp
{
    public static class OrchestratorChainFirstFunction
    {
        [FunctionName("OrchestratorChainFirstFunction")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            var input = context.GetInput<ReviewViewModel>();

            outputs.Add(await context.CallActivityAsync<string>("OrchestratorChainFirstFunction_SetApprover", input));
            outputs.Add(await context.CallActivityAsync<string>("OrchestratorChainFirstFunction_EnableReviewEntry", input));

            return outputs;
        }

        [FunctionName("OrchestratorChainFirstFunction_SetApprover")]
        public static string SetApprover([ActivityTrigger] ReviewViewModel record, ILogger log)
        {
            log.LogInformation($"Set review record {record.ID} approver to {record.Reviewer}.");
            return $"Review record {record.ID} approver to {record.Reviewer}!";
        }

        [FunctionName("OrchestratorChainFirstFunction_EnableReviewEntry")]
        public static string EnableReviewEntry([ActivityTrigger] ReviewViewModel record, ILogger log)
        {
            log.LogInformation($"Set visibility of review record {record.ID} to {record.IsVisible}.");
            return $"Set visibility of review record {record.ID} to {record.IsVisible}.";
        }

        [FunctionName("OrchestratorChainFirstFunction_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("OrchestratorChainFirstFunction", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}