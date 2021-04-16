using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTinyBank.Web.MyModel
{
    public class AddCardOptions
    {
        public string customerId { get; set; }
        public string accountId { get; set; }
        public string cardType { get; set; }
    }
}
