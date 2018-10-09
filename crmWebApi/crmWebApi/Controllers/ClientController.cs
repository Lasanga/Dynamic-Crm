using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmWebApi.CrmConstants;
using crmWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace crmWebApi.Controllers
{

    [Produces("application/json")]
    [Route("api/Client")]
    public class ClientController : Controller
    {
        [HttpGet]
        public string Get()
        {
            var api = "https://bidrik.api.crm4.dynamics.com/api/data/v9.0/";

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
    }
}