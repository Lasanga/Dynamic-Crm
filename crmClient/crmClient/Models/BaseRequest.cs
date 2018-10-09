using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmClient.Models
{
    public class BaseRequest
    {
        public static string BaseUrl = "https://crm-server-api.azurewebsites.net";
        public static string AccessToken { get; set; }
    }
}
