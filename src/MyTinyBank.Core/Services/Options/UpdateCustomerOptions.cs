using MyTinyBank.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTinyBank.Core.Services.Options
{
    public class UpdateCustomerOptions
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public CustomerType CustType { get; set; }
    }
}