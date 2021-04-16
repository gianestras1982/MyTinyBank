using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services.Options;

namespace MyTinyBank.Core.Services
{
    public interface IAccountService
    {
        public ApiResult<Account> CreateAccount(Guid customerId, CreateAccountOptions options);
        public IQueryable<Account> SearchAccount(SearchAccountOptions options);
        public ApiResult<List<Account>> GetAccountByCustomerId(Guid? customerId);
        public ApiResult<Account> GetAccountByAccountId(string accountId);
        public ApiResult<Account> ChangeStateAccount(string accountId, string state);
    }
}
