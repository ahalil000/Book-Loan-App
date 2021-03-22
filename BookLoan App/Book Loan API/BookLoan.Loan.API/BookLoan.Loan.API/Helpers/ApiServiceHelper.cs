using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json; // JsonConvert
using System.Net.Http.Headers; // MediaTypeWithQualityHeaderValue, AuthenticationHeaderValue etc


namespace BookLoan.Helpers
{
    public class ApiServiceHelper: IDisposable
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public ApiServiceHelper(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="contentval"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAPI(string uri, object contentparam, string authtoken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            var client = _clientFactory.CreateClient();

            var content = JsonConvert.SerializeObject(contentparam);

            //var response = await client.PostAsync(request.RequestUri,
            //    new StringContent(content, System.Text.Encoding.UTF8,
            //    "application/json"));
            StringContent stringContent =
                new StringContent(content, System.Text.Encoding.UTF8,
                    "application/json");

            var response = await client.PostAsync(request.RequestUri,
                stringContent);

            return response;
        }



        public async Task<HttpResponseMessage> GetAPI(string uri, object contentparam, string authtoken = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var client = _clientFactory.CreateClient();

            if (authtoken != null)
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authtoken);
            }

            var response = await client.SendAsync(request);

            return response;
        }

        public void Dispose()
        {
            //
        }
    }
}
