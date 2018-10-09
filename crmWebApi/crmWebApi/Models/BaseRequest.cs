using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmWebApi.Models
{
    public class BaseRequest
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the token. </summary>
        ///
        /// <value> The token. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string Token { get; set; }

        public static string BaseUrl = "https://bidrik.api.crm4.dynamics.com/";
    }
}
