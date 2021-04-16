using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTinyBank.Core.Constants;
using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;

namespace MyTinyBank.Core.Implementation.Services
{
    public class AccountService : IAccountService
    {
        private readonly MyTinyBankDbContext _dbContext;
        private readonly ICustomerService _customers;

        public AccountService(MyTinyBankDbContext dbContext, ICustomerService customers)
        {
            _dbContext = dbContext;
            _customers = customers;
        }

        public ApiResult<Account> CreateAccount(Guid customerId, CreateAccountOptions options)
        {
            if (options == null)
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.CurrencyCode))
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.CurrencyCode)}"
                };
            }


            var customer = _customers.GetCustomerById(customerId).Data;
            if (customer == null)
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Customer with id {customerId} could not found."
                };
            }


            var account = new Account()
            {
                AccountId = CreateAccountId(customer.CountryCode),
                CustomerId = customer.CustomerId,
                Balance = 0,
                CurrencyCode = options.CurrencyCode,
                Customer = customer,
                State = AccountState.Active,
                Description = options.Description
            };

            _dbContext.Add(account);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Account could not be saved --> " + ex.ToString()
                };
            }

            return new ApiResult<Account>()
            {
                Code = ApiResultCode.Success,
                Data = account
            };
        }

        private string CreateAccountId(string countryCode)
        {
            var random = new Random();
            var accountId = $"{countryCode}{random.Next(1000, int.MaxValue).ToString().PadLeft(20, '0')}";

            return accountId;
        }

        public IQueryable<Account> SearchAccount(SearchAccountOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var q = _dbContext.Set<Account>()
                .Include(acc => acc.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.AccountId))
            {
                q = q.Where(acc => acc.AccountId == options.AccountId);
            }

            if (!string.IsNullOrWhiteSpace(options.CurrencyCode))
            {
                q = q.Where(acc => acc.CurrencyCode == options.CurrencyCode);
            }

            if (options.State != AccountState.Undefined)
            {
                q = q.Where(acc => acc.State == options.State);
            }

            if (options.CustomerId != null)//Gia to Guid elegxos mono me to null. gia ta ipoloipa string IsNullOrWhiteSpace
            {
                q = q.Where(acc => acc.CustomerId == options.CustomerId);
            }

            return q;
        }

        public ApiResult<List<Account>> GetAccountByCustomerId(Guid? customerId)
        {
            if (customerId == null)
            {
                return new ApiResult<List<Account>>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(customerId)}"
                };
            }

            var account = _dbContext.Set<Account>()
                         .Include(acc => acc.Customer)
                         .Include(acc => acc.Cards)
                         .Where(acc => acc.CustomerId == customerId)
                         .ToList();

            if (account != null)
            {
                return new ApiResult<List<Account>>()
                {
                    Code = ApiResultCode.Success,
                    Data = account
                };
            }
            else
            {
                return new ApiResult<List<Account>>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Customer with id {customerId} could not found."
                };
            }
        }

        public ApiResult<Account> GetAccountByAccountId(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(accountId)}"
                };
            }

            var account = _dbContext.Set<Account>()
                         //.Include(acc => acc.Customer)
                         //.Include(acc => acc.Cards)
                         .Where(acc => acc.AccountId == accountId)
                         .SingleOrDefault();

            if (account != null)
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.Success,
                    Data = account
                };
            }
            else
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"accountId with id {accountId} could not found."
                };
            }
        }

        public ApiResult<Account> ChangeStateAccount(string accountId, string state)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(accountId)}"
                };
            }

            if (string.IsNullOrWhiteSpace(state))
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(state)}"
                };
            }

            var account = GetAccountByAccountId(accountId).Data;

            if (account != null)
            {
                var updstate = Enum.Parse<AccountState>(state, true);
                account.State = updstate;
            }
            else
            { 
            
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Account>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Account could not be changed the state --> " + ex.ToString()
                };
            }

            return new ApiResult<Account>()
            {
                Code = ApiResultCode.Success,
                Data = account
            };
        }

    }
}
