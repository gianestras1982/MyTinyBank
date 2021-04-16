using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTinyBank.Web.MyModel
{
    public class AddExistingCardOptions
    {
        public string cardNumber { get; set; }
        public string customerId { get; set; }
        public string accountId { get; set; }
    }
}
