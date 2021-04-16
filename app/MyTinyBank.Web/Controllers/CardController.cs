using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;
using MyTinyBank.Web.Models;
using MyTinyBank.Web.Extensions;
using MyTinyBank.Core.Constants;
using MyTinyBank.Web.MyModel;

namespace MyTinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICardService _cards;
        private readonly ICustomerService _customers;
        private readonly IAccountService _accounts;
        private readonly ILogger<HomeController> _logger;
        private readonly MyTinyBankDbContext _dbContext;

        public CardController(MyTinyBankDbContext dbContext, ILogger<HomeController> logger, ICustomerService customers, IAccountService accounts, ICardService cards)
        {
            _logger = logger;
            _cards = cards;
            _customers = customers;
            _accounts = accounts;
            _dbContext = dbContext;
        }

        [HttpGet]//https://localhost:5001/card
        public IActionResult Index()
        {
            return View();
        }




        ////card pay form
        //[HttpGet("card")]
        //public IActionResult Card()
        //{
        //    return View();
        //}


        [HttpPut("checkout")]
        public IActionResult CardCheckOut([FromBody] CardPay options)
        {

            var result = _cards.CardCheckOut(options);

            var transResult = new TransResult()
            {
                CodeResult = result.Code,
                ErrorText = result.ErrorText
            };

            return Json(transResult);
        }









        //Apo edo kai kato einai dika mou dimitri... afta den ta koitazeis einai testarismata mou...








        //adding card form
        [HttpGet("{customerId:guid}/AddCardForm")]
        public IActionResult AddCardForm(Guid customerId)
        {
            var custId = customerId;

            return View(custId);
        }

        //save add card form
        [HttpPut("{customerId:guid}/AddCardFormSave")]
        public IActionResult AddCardFormSave(Guid customerId, [FromBody] AddCardOptions options)
        {

            CreateCardOptions c = new CreateCardOptions();
            c.cardType = CardType.Debit;

            var card = _cards.CreateCard(customerId, options.accountId, c);

            if (card.Data != null)
            {
                card.Data.Accounts.Clear();                
            }

            return Json(card.Data);
        }

        //adding existing card form
        [HttpGet("{customerId:guid}/AddExistingCardForm")]
        public IActionResult AddExistingCardForm(Guid customerId)
        {
            var custId = customerId;

            return View(custId);
        }

        //save add existing card form
        [HttpPut("{customerId:guid}/AddExistingCardFormSave")]
        public IActionResult AddExistingCardFormSave(Guid customerId, [FromBody] AddExistingCardOptions options)
        {
            var card = _cards.ConnectCard(customerId, options.accountId, options.cardNumber);

            if (card.Data != null)
            {
                card.Data.Accounts.Clear();
            }

            return Json(card.Data);
        }
    }
}