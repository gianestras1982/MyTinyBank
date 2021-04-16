using Microsoft.Extensions.DependencyInjection;
using MyTinyBank.Core.Constants;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyTinyBank.Core.Tests
{
    public class AccountServiceTests : IClassFixture<MyTinyBankFixture>
    {
        private IAccountService _account;

        public AccountServiceTests(MyTinyBankFixture fixture)
        {
            _account = fixture.Scope.ServiceProvider.GetRequiredService<IAccountService>();
        }


        [Fact]
        public void Test_CreateAccount()
        {
            var options = new CreateAccountOptions()
            {
                CurrencyCode = "EUR",
                Description = "salary"
            };

            Guid customerId = new Guid();
            var account = _account.CreateAccount(customerId, options);
            Assert.NotNull(account);

            //return account.Data;
        }

        [Fact]
        public void Test_SearchAccount()
        {
            var options = new SearchAccountOptions()
            {
                CustomerId = new Guid("78EBB419-804D-48BC-8467-5FA4C03DCE28"),
                AccountId = "GR00000000000230569169",
                State = AccountState.Active
            };

            var account = _account.SearchAccount(options).SingleOrDefault();
            Assert.NotNull(account);
        }

        [Fact]
        public void Test_GetAccountByCustomerId()
        {
            var customerId = new Guid("78EBB419-804D-48BC-8467-5FA4C03DCE28");

            var account = _account.GetAccountByCustomerId(customerId);
            Assert.NotNull(account);
        }
    }
}
