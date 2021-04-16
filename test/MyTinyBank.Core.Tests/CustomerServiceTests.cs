using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Implementation.Services;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;

using Xunit;
using MyTinyBank.Core.Tests;
using MyTinyBank.Core.Constants;

namespace MyTinyBank.Core.Tests
{
    public class CustomerServiceTests : IClassFixture<MyTinyBankFixture>
    {
        //Kanena allo service de mporei na mpei edo tha fame cycle error.

        private ICustomerService _customer;
        private readonly MyTinyBankDbContext _dbContext;

        public CustomerServiceTests(MyTinyBankFixture fixture)
        {
            _customer = fixture.Scope.ServiceProvider.GetRequiredService<ICustomerService>();
        }


        //Efaga xrono na ta peraso me tin mia alla mou skaei kato kato kai den to oloklirosa disixos.
        //Ta test fisika iparxoune memonomena to kathena sta Test Class tou.

        [Fact]
        public void Test_Add_Customer_Account_Card()
        {
            var customer = new Customer()
            {
                Firstname = "Sofia",
                Lastname = "Ourolidou",
                VatNumber = "333333333",
                Email = "s.ourolidou@gmail.com",
                IsActive = true,
                Address = "Irakleitou 17",
                CountryCode = "GR",
                CustType = CustomerType.PhysicalEntity,
                Phone = "2107482373",
                DateOfBirth = "26/02/1982"
            };

            var account = new Account()
            {
                Balance = 1000,
                CurrencyCode = "EUR",
                State = Constants.AccountState.Active,
                AccountId = "GR00000000003333333333", 
                Customer = customer, 
                Description = "My own account"
            };

            customer.Accounts.Add(account);

            var card = new Card()
            {
                Active = true,
                CardNumber = "333333333333333",
                CardType = Constants.CardType.Debit
            };

            account.Cards.Add(card);

            _dbContext.Add(customer);
            _dbContext.SaveChanges();

        }


        [Fact]
        public Customer Test_RegisterCustomer()
        {
            var options = new RegisterCustomerOptions()
            {
                Firstname = "Evaggelos",
                Lastname = "Gianestras",
                VatNumber = "333333333",
                Email = "e.gianestras@yahoo.gr",
                CountryCode = Country.GreekCountryCode,
                Address = "Drakontos 24",
                Phone = "2510232052",
                DateOfBirth = new DateTime(1952, 03, 25).ToString("yyyy-MM-dd"),
                CustType = CustomerType.PhysicalEntity

            };
            var customer = _customer.RegisterCustomer(options);
            Assert.NotNull(customer.Data);

            return customer.Data;
        }

        [Fact]
        public void Test_SearchCustomer_WithIdAndVat()
        {
            var options = new SearchCustomerOptions()
            {
                VatNumber = "111111111"
            };
            var customer = _customer.SearchCustomer(options).SingleOrDefault();
            Assert.NotNull(customer);
        }

        [Fact]
        public void Test_SearchCustomer_WithCountries()
        {
            var options = new SearchCustomerOptions()
            {
                CountryCodes = { Country.GreekCountryCode, Country.ItalyCountryCode, Country.ItalyCountryCode }
            };
            var customer = _customer.SearchCustomer(options).ToList();
            Assert.NotNull(customer);
        }

        [Fact]
        public void Test_GetCustomerById()
        {
            var customer = _customer.GetCustomerById(new Guid("C0366944-D2BD-4F78-8297-C0E9E2E8391D"));
            Assert.NotNull(customer.Data);
        }

        [Fact]
        public void Test_UpdateCustomer()
        {
            var options = new UpdateCustomerOptions()
            {
                VatNumber = "000000000"
            };

            var customer = _customer.UpdateCustomer(new Guid("78EBB419-804D-48BC-8467-5FA4C03DCE28"), options);
            Assert.NotNull(customer);
        }

        [Fact]
        public void Test_GetAllCardsByCustId()
        {
            var result = _customer.GetAllCardsByCustId();
        }

        [Fact]
        public void Test_Georgios()
        {
            var options = new SearchCustomerOptions()
            {
                FirstName = "Georgios"
            };

            var result = _customer.SearchCustomer(options)
                         .ToList();

            Assert.NotNull(result);
        }

        [Fact]
        public void Test_GetAllCardsByCustIdSecondVers()
        {
            var result = _customer.GetAllCardswithAccByCustId(new Guid("C0366944-D2BD-4F78-8297-C0E9E2E8391D"));

            var x = 1;
        }
    }
}