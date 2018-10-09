using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmClient.Models
{
    public class Account
    {
        public string name { get; set; }
        public int accountcategorycode { get; set; }
        public int customertypecode { get; set; }
        public int revenue { get; set; }
        public string description { get; set; }
        public string address1_stateorprovince { get; set; }
        public string address1_telephone1 { get; set; }
    }
}
