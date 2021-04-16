using MyTinyBank.Core.Constants;
using System;

namespace MyTinyBank.Core.Services.Options
{
    public class RegisterCustomerOptions
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
