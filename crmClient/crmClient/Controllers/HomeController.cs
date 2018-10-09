using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crmClient.Models;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using Nito.AsyncEx;
using crmClient.ServerHandlers;
using Newtonsoft.Json;

namespace crmClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount(Account input)
        {
            var apiUrl = "api/Customer/Create";

            var account = new Account()
            {
                name = input.name,
                revenue = input.revenue,
                description = input.description,
                address1_telephone1 = input.address1_telephone1,
                address1_stateorprovince = input.address1_stateorprovince,
                accountcategorycode = input.accountcategorycode,
                customertypecode = input.customertypecode
            };

            var status =  CrmServerHandlers.HttpPostToServer(apiUrl, JsonConvert.SerializeObject(account));

            var result = string.Empty;
            if (status == 200 || status == 204)
            {
                result = "Action completed";
            }
            return RedirectToAction(nameof(Accounts));
        }

        public IActionResult Accounts()
        {
            var apiUrl = "api/Customer/GetAll";

            var accounts = CrmServerHandlers.HttpGetAll(apiUrl);
            return View(accounts);
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
