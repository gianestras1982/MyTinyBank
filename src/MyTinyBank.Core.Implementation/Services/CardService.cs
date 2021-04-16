using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

using MyTinyBank.Core.Constants;
using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;

namespace MyTinyBank.Core.Implementation.Services
{
    public class CardService : ICardService
    {
        private readonly MyTinyBankDbContext _dbContext;
        private readonly ICustomerService _customers;
        private readonly IAccountService _accounts;

        public CardService(MyTinyBankDbContext dbContext, ICustomerService customers, IAccountService accounts)
        {
            _dbContext = dbContext;
            _customers = customers;
            _accounts = accounts;
        }

        public ApiResult<Card> CardCheckOut(CardPay options)
        {

            if (options == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (String.IsNullOrWhiteSpace(options.CardNumber))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.CardNumber)}"
                };
            }

            if (String.IsNullOrWhiteSpace(options.ExpirationMonth))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.ExpirationMonth)}"
                };
            }

            if (String.IsNullOrWhiteSpace(options.ExpirationYear))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.ExpirationYear)}"
                };
            }

            if (String.IsNullOrWhiteSpace(options.Amount))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Amount)}"
                };
            }

            var CardExist = _dbContext.Set<Card>()
                            .Where(ca => ca.CardNumber == options.CardNumber)
                            .Any();
            if (!CardExist)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Card {options.CardNumber} doesnt exist."
                };
            }



            var card = _dbContext.Set<Card>()
                       .Where(ca => ca.CardNumber == options.CardNumber)
                       .Include(ca => ca.Accounts)
                       .SingleOrDefault();
            if (card.Accounts.Count == 0)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"WOW! PROBLEM... Card {options.CardNumber} has not account"
                };
            }


            if (!card.Active)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Sorry, card {options.CardNumber} is not active"
                };
            }



            if (card.Accounts.SingleOrDefault().State == AccountState.Inactive)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Sorry, account {card.Accounts.SingleOrDefault().AccountId} is not active"
                };
            }


            if (Decimal.Parse(options.Amount) > card.Accounts.SingleOrDefault().Balance)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Sorry, you ask {options.Amount} and you have {card.Accounts.SingleOrDefault().Balance}"
                };
            }


            if (Int32.Parse(options.ExpirationMonth) < 0 || Int32.Parse(options.ExpirationMonth) > 12)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Month must be between 1-12"
                };
            }


            DateTimeOffset dt  = card.Expiration;
            int month = dt.Month;
            int year = dt.Year;

            if (month != Int32.Parse(options.ExpirationMonth))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"The expiration month is wrong."
                };
            }

            if (year != Int32.Parse(options.ExpirationYear))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"The expiration year is wrong."
                };
            }


            card.Accounts.SingleOrDefault().Balance = card.Accounts.SingleOrDefault().Balance - Decimal.Parse(options.Amount);


            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"The transaction could not be processed " + ex.ToString()
                };
            }

            return new ApiResult<Card>()
            {
                Code = ApiResultCode.Success,
                Data = card
            };
        }









        //Dimitri ap edo kai kato einai dika mou practise test...










        public ApiResult<Card> CreateCard(Guid? customerId, string accountId, CreateCardOptions options)
        {
            if (options == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (options.cardType == CardType.Undefined)
            {
                options.cardType = CardType.Debit;//afto to kano giati to enum mou xtipaei sto json sto web.

                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.cardType)}"
                };
            }

            if (customerId == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(customerId)}"
                };
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(accountId)}"
                };
            }


            var card = new Card()
            {
                CardNumber = CreateCardNumber(),
                Active = true,
                CardType = options.cardType
            };

            var newCard = GetCardbyCardNumbr(card.CardNumber);
            if (newCard.Data != null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Card {card.CardNumber} already exists."
                };
            }
            

            var customer = _customers.GetCustomerById(customerId).Data;
            if (customer == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Customer {customer.CustomerId} doesnt exist."
                };
            }


            var account = _accounts.GetAccountByAccountId(accountId);
            if (account.Data == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Account {accountId} doesnt exist."
                };
            }

            if (!customer.Accounts.Contains(account.Data))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Customer ${customer.CustomerId} doesnt have account {account.Data}."
                };
            }

            card.Accounts.Add(account.Data);
            _dbContext.Add(card);


            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Card could not be saved --> " + ex.ToString()
                };
            }

            return new ApiResult<Card>()
            {
                Code = ApiResultCode.Success,
                Data = card
            };
        }

        private string CreateCardNumber()
        {
            var random = new Random();
            var cardNumber = $"{random.Next(1000, int.MaxValue).ToString().PadLeft(15, '0')}";

            return cardNumber;
        }

        //Ego o pelatis me customerId thelo na peraso tin cardNumber mou sto accountId eite diko mou eite allou.
        public ApiResult<Card> ConnectCard(Guid? customerId, string accountId, string cardNumber)
        {
            if (customerId == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(customerId)}"
                };
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(accountId)}"
                };
            }

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(cardNumber)}"
                };
            }


            var account = _dbContext.Set<Account>()
                      .Include(acc => acc.Cards)
                      .Where(acc => acc.AccountId == accountId)
                      .SingleOrDefault();
            if (account == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Account could not be found"
                };
            }



            var card = _dbContext.Set<Card>()
                      .Where(ca => ca.CardNumber == cardNumber)
                      .SingleOrDefault();
            if (card == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Card could not be found"
                };
            }



            var customer = _customers.GetCustomerById(customerId).Data;
            if (customer == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Customer could not be found"
                };
            }

            //Elegxos...an i iparxousa karta pou theloume na koumposoume sto logariasmo iparxei idi se afto ton logariasmo tote na skaei.
            var resCards = _dbContext.Set<Account>()
                           .Where(acc => acc.AccountId == accountId)
                           .SelectMany(acc => acc.Cards).ToList();
            if (resCards.Count > 0)
            {
                var CardExist = resCards.Where(ca => ca.CardNumber == cardNumber)
                                .Any();
                if (CardExist)
                {
                    return new ApiResult<Card>()
                    {
                        Code = ApiResultCode.BadRequest,
                        ErrorText = $"The card {cardNumber} already existis to the account {accountId}."
                    };
                }
            }
            //Telos elegxos///////////////////////////////////////////////////////////////////////////////////

            card.Accounts.Add(account);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Card could not be added --> " + ex.ToString()
                };
            }

            return new ApiResult<Card>()
            {
                Data = card
            };

        }

        public ApiResult<Card> GetCardbyCardNumbr(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(cardNumber)}"
                };
            }

            var card = _dbContext.Set<Card>()
                      .Where(ca => ca.CardNumber == cardNumber)
                      .Include(ca => ca.Accounts)
                      .ThenInclude(acc => acc.Customer)
                      .SingleOrDefault();

            if (card == null)
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Card number with number {cardNumber} could not found."
                };
            }
            else
            {
                return new ApiResult<Card>()
                {
                    Code = ApiResultCode.Success,
                    Data = card
                };
            }

        }
    }
}
