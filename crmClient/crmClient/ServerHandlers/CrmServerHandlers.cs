using crmClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crmClient.ServerHandlers
{
    public class CrmServerHandlers
    {
        public static void HttpGetFromServer(string url)
        {
            var baseUrl = BaseRequest.BaseUrl;
            var apiUrl = url;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                var response = AsyncContext.Run(() => client.SendAsync(request));
                var status = (int)response.StatusCode;

                if (status == 200)
                {
                    BaseRequest.AccessToken = AsyncContext.Run(() => response.Content.ReadAsStringAsync().Result);
                }
                else if (status != 204)  // No content
                {
                    throw new Exception("Response Status Code: " + status);
                }
            }
        }


        public static int HttpPostToServer(string url, string content)
        {
            var baseUrl = BaseRequest.BaseUrl;
            var apiUrl = url;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                var response = AsyncContext.Run(() => client.SendAsync(request));

                var status = (int)response.StatusCode;

                return status;
            }
        }

        public static List<AccountGetDto> HttpGetAll(string url)
        {
            var baseUrl = BaseRequest.BaseUrl;
            var apiUrl = url;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                var response = AsyncContext.Run(() => client.SendAsync(request));
                var status = (int)response.StatusCode;


                var accounts = new List<AccountGetDto>();

                if (status == 200)
                {
                    var responseData = AsyncContext.Run(() => response.Content.ReadAsStringAsync());

                    dynamic data = JsonConvert.DeserializeObject(responseData);

                    JObject jObject = JObject.Parse(data);

                    var values = jObject.SelectToken("value").ToList();
                    try
                    {
                        foreach (var value in values)
                        {
                            if (!string.IsNullOrEmpty(value.SelectToken("customertypecode").ToString()))
                            {
                                var type = string.Empty;
                                switch ((int)value.SelectToken("customertypecode"))
                                {
                                    case 1:
                                        type = "Competitor";
                                        break;

                                    case 2:
                                        type = "Consultant";
                                        break;

                                    case 3:
                                        type = "Customer";
                                        break;

                                    case 10:
                                        type = "Supplier";
                                        break;

                                    case 4:
                                        type = "Investor";
                                        break;

                                    default:
                                        break;
                                }

                                string accType = string.Empty;
                                switch ((int)value.SelectToken("accountcategorycode"))
                                {
                                    case 1:
                                        accType = "Preffered Customer";
                                        break;

                                    case 2:
                                        accType = "Standard";
                                        break;

                                    default:
                                        break;
                                }

                                accounts.Add(new AccountGetDto()
                                {
                                    name = value.SelectToken("name").ToString(),
                                    revenue = (int)value.SelectToken("revenue"),
                                    description = value.SelectToken("description").ToString(),
                                    accountcategorycode = (int)value.SelectToken("accountcategorycode"),
                                    address1_stateorprovince = value.SelectToken("address1_stateorprovince").ToString(),
                                    address1_telephone1 = value.SelectToken("address1_telephone1").ToString(),
                                    customertypecode = (int)value.SelectToken("customertypecode"),
                                    type = type,
                                    accType = accType
                                });

                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                return accounts;
            }
        }
    }
}
