using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services.Options;

namespace MyTinyBank.Core.Services
{
    public interface ICardService
    {

        public ApiResult<Card> CardCheckOut(CardPay options);

        public ApiResult<Card> CreateCard(Guid? customerId, string accountId, CreateCardOptions options);

        public ApiResult<Card> ConnectCard(Guid? customerId, string accountId, string cardNumber);

        public ApiResult<Card> GetCardbyCardNumbr(string cardNumber);

        
    }
}
