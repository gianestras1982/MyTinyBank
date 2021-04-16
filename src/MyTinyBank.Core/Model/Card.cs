using MyTinyBank.Core.Constants;
using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Model
{
    public class Card
    {
        public Guid CardId { get; set; }
        public string CardNumber { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Active { get; set; }
        public CardType CardType { get; set; }

        //Navigation Property
        public List<Account> Accounts { get; set; }

        public Card()
        {
            CardId = Guid.NewGuid();
            Active = true;
            Accounts = new List<Account>();
            Expiration = DateTimeOffset.Now.AddYears(6);
        }
    }
}
