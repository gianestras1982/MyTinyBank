using MyTinyBank.Core.Constants;
using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Services.Options
{
    public class SearchAccountOptions
    {
        public string AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public AccountState State { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
