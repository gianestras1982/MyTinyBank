using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyTinyBank.Web.MyModel
{
    public class AddAccountOptions
    {
        public string Currency { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
    }
}
