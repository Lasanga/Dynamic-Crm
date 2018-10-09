using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using crmClient.Models;
using crmClient.ServerHandlers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace crmClient.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            //var apiUrl = "api/Customer/Create";

            //var account = new Account()
            //{
            //    name = "lasanga",
            //    revenue = 1000,
            //    description = " hello world",
            //    address1_telephone1 = "011258963",
            //    address1_stateorprovince = "Colombo",
            //    accountcategorycode = 1
            //};

            //var status = CrmServerHandlers.HttpPostToServer(apiUrl, JsonConvert.SerializeObject(account));

            return View();
        }
    }
}