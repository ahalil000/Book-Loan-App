using Microsoft.AspNet.OData.Batch;

namespace BookLoan.Catalog.API.Helpers
{
    public class ODataHelper
    {
        public static DefaultODataBatchHandler CreateDefaultODataBatchHander(
            int nestDepth, int maxOpsPerChangeSet, int maxReceivedMsgSize)
        {
            var defaultODataBatchHander = new DefaultODataBatchHandler();
            defaultODataBatchHander.MessageQuotas.MaxNestingDepth = nestDepth;
            defaultODataBatchHander.MessageQuotas.MaxOperationsPerChangeset = maxOpsPerChangeSet;
            defaultODataBatchHander.MessageQuotas.MaxReceivedMessageSize = maxReceivedMsgSize;
            return defaultODataBatchHander;
        }
    }
}
