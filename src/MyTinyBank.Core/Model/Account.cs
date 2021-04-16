using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Model
{
    public class Account
    {
        public string AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public Constants.AccountState State { get; set; }
        public Guid CustomerId { get; set; }
        public AuditInfo AuditInfo { get; set; }

        //Navigations Properties
        public List<Card> Cards { get; set; }
        public Customer Customer { get; set; }

        public Account()
        {
            AuditInfo = new AuditInfo();
            Cards = new List<Card>();
        }
    }
}
