using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BookLoan.Interfaces;

namespace BookLoan.Helpers
{
    public class ApiServiceRetry: IApiServiceRetry
    {
        private readonly ILogger _logger;
        private readonly int _maxRetries;
        private int _numRetries;

        /// <summary>
        /// ApiServiceRetry()
        /// ApiServiceRetry()
        /// Api service with retry.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="maxRetries"></param>
        public ApiServiceRetry(ILogger<ApiServiceRetry> logger,
            int maxRetries = 20)
        {
            this._maxRetries = maxRetries;
            this._logger = logger;
            this._numRetries = 0;
        }


        /// <summary>
        /// RunAsync()
        /// Retry the task uo to number of maximum retries
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task RunAsync(Func<Task> func)
        {
            retry:
            try
            {
                await func();
            }
            catch (Exception ex) when (ex is TimeoutException ||
                ex is System.Net.Http.HttpRequestException)
            {
                _logger.LogError("Error ApiServiceRetry(): {0}. " +
                    "Message: {1}. Inner Message: {1} ",
                    ex.GetType().ToString(),
                    ex.Message,
                    ex.InnerException.Message);

                IncrementRetry();
                goto retry;
            }
        }


        /// <summary>
        /// IncrementRetry()
        /// Increment retry. Throw exception if maximum retry exceeded.
        /// </summary>
        public void IncrementRetry()
        {
            if (_numRetries == _maxRetries)
            {
                throw new TimeoutException("Maximum retry attempts exceeded.");
            }
            ++_numRetries;
        }
    }
}
