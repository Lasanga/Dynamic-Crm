using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using crmWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;

namespace crmWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {

        [HttpPost]
        [Route("Create")]
        public async Task<int> PostAsync([FromBody]Account account)
        {
            return await CreateAccountAsync(account); 
        }

        private async Task<int> CreateAccountAsync(Account account)
        {
            var apiUrl = "api/data/v9.0/accounts";
            var content = JsonConvert.SerializeObject(account);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseRequest.BaseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BaseRequest.Token);

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(content.ToString(), Encoding.UTF8, "application/json");

                var response = AsyncContext.Run(() => client.SendAsync(request));
                var status = (int)response.StatusCode;

                return await Task.FromResult(status);
            }
        }

        [HttpGet]
        [Route("GetALL")]
        public string GetAllAccounts()
        {
            return GetAllResponse();  
        }

        private string GetAllResponse()
        {
            var apiUrl = "api/data/v9.0/accounts?$select=name,revenue,accountcategorycode,description,customertypecode,address1_stateorprovince,address1_telephone1";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseRequest.BaseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BaseRequest.Token);

                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

                var response = AsyncContext.Run(() => client.SendAsync(request));
                var status = (int)response.StatusCode;

                var result = string.Empty;

                if (status == 200)
                {
                    result = AsyncContext.Run(() => response.Content.ReadAsStringAsync());
                }

                return result;
            }
        }
    }
}