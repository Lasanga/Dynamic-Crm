# Dynamic-Crm

Application demonstrating on how to connect with azure oauth and dynamic crm via web api requests.

Purpose of this project is to work along with dynamic crm that simplifies managing your sales, users etc... Also with oauth 2.0 authorzation user data is secured
Check crm;
https://dynamics.microsoft.com/en-us/


Mvc pattern used with two entites.
1. Client Application: with the presentation components
2. WebApi Applicaiton: handles all the web api requests to dynamic crm

## Steps:

Create client application with oauth 2.0 and publish to azure portal. steps in detail in;
https://github.com/Lasanga/simpleOAuthApp

Create web api project in .Net core 2.0 with authorization to the relevant active directory
Get the application id and register the application to your dynamic crm in security > users. for more details refere;
https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/walkthrough-register-dynamics-365-app-azure-active-directory

Once done application id as client id and secret key must be declared in the web api project to gain access token needed in calling the dynamic crm APIs

```c#
        [HttpGet]
        public string Get()
        {
            var api = "[ORGANIZATION URL]/api/data/v9.0/";

            return GetTokenAsync(new Uri(api), CrmConsts.clientId, CrmConsts.secretKey);
        }

        private string GetTokenAsync(Uri api, string clientId, string secretKey)
        {
            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(api).Result;

            var creds = new ClientCredential(clientId, secretKey);

            AuthenticationContext context = new AuthenticationContext(ap.Authority);

            var accessToken = context.AcquireTokenAsync(ap.Resource, creds).Result.AccessToken;
            BaseRequest.Token = accessToken;
            return accessToken;
        }
```
*Above code stores the token once logged in*

Afterwwards as prefered you can call the APIs you requrie. I have used APIs to create account for customers and retrieve them.

## Example: To create Account (status code returned: 204)

```c#
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
```

*For more information on dynamic crm web APIs*
1. Entity types and there properties 
https://docs.microsoft.com/en-us/dynamics365/customer-engagement/web-api/account?view=dynamics-ce-odata-9

2. Web requests and there responses
https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/webapi/web-api-basic-operations-sample


