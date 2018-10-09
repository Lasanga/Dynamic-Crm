using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmClient.Models
{
    public class AccountGetDto : Account
    {
        public string accountid { get; set; }
        public string _transactioncurrencyid_value{ get; set; }
        public string address1_composite { get; set; }
        public string type { get; set; }
        public string accType { get; set; }
    }
}
