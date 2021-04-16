using Microsoft.Extensions.DependencyInjection;
using MyTinyBank.Core.Constants;
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
    public class CardServiceTests : IClassFixture<MyTinyBankFixture>
    {
        private ICardService _card;
        private ICustomerService _customer;

        public CardServiceTests(MyTinyBankFixture fixture)
        {
            _card = fixture.Scope.ServiceProvider.GetRequiredService<ICardService>();
        }

        [Fact]
        public void Test_CreateCard()
        {
            var options = new CreateCardOptions()
            {
                cardType = CardType.Debit
            };

            var card = _card.CreateCard(new Guid("C0366944-D2BD-4F78-8297-C0E9E2E8391D"), "GR00000000001301120947", options);
            Assert.NotNull(card);
        }






        //apo edo kai kato einai dika mou practise test.





        [Fact]
        public void Test_ConnectCard()
        {
            var card = _card.ConnectCard(new Guid("B0DBE44D-C23F-4EB1-B1DF-8E7856FA73EE"), "GR00000000002054751319", "000000383509419");
            Assert.NotNull(card);
        }

        [Fact]
        public void Test_GetCardbyCardNumbr()
        {
            var card = _card.GetCardbyCardNumbr("000000383509419");
            Assert.NotNull(card);
        }

    }
}
