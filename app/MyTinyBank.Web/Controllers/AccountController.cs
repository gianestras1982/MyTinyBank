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
using MyTinyBank.Core;

namespace MyTinyBank.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly IAccountService _accounts;
        private readonly ICardService _cards;
        private readonly ILogger<HomeController> _logger;
        private readonly MyTinyBankDbContext _dbContext;

        public AccountController(MyTinyBankDbContext dbContext, ILogger<HomeController> logger, ICustomerService customers, IAccountService accounts, ICardService cards)
        {
            _logger = logger;
            _customers = customers;
            _accounts = accounts;
            _cards = cards;
            _dbContext = dbContext;
        }

        [HttpGet]//https://localhost:5001/account
        public IActionResult Index()
        {
            var result = _dbContext.Set<Account>()
                        .Include(acc => acc.Customer)
                        .Select(a => new
                        {
                            account = a.AccountId,
                            vatNumber = a.Customer.VatNumber
                        })
                        .ToList();

            List<AccntsList> rslt = new List<AccntsList>();
            if (result.Count > 0)
            {
                AccntsList acc;

                foreach (var r in result)
                {
                    acc = new AccntsList();

                    acc.AccountNumber = r.account;
                    acc.VatNumber = r.vatNumber;

                    rslt.Add(acc);
                }
            }
            else
            {
                var finalResult = new ApiResult<List<AccntsList>>()
                {
                    Code = ApiResultCode.NotFound,
                    ErrorText = "There aren't any accounts yet."
                };

                return finalResult.ToActionResult();
            }

            return View(rslt);
        }


        [HttpGet("{custId}/addAccountForm")]
        public IActionResult AddAccountForm(Guid custId)
        {
            return View(custId);
        }


        [HttpPut("addAccountSave")]
        public IActionResult AddAccountSave([FromBody] AddAccountOptions options)
        {
            var opt = new CreateAccountOptions()
            {
                CurrencyCode = options.Currency,
                Description = options.Description
            };

            var result = _accounts.CreateAccount(new Guid(options.CustomerId), opt);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }

            result.Data.Customer = null;//an de to ekana afto yha eskage to apo kato tou logo cycle. Eidallos tha mporousa na kano return Ok(), aplos dokimasa.
            return Ok(result);
        }


        [HttpGet("{accountId}")]
        public IActionResult GetDetailsAccount(string accountId)
        {
            var result = _accounts.GetAccountByAccountId(accountId);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }


            var AccStateArray = Enum.GetValues(typeof(AccountState));

            var dtslAcc = new DtlsAcc()
            {
                AccountObj = result.Data,
                AccStates = AccStateArray
            };


            return Ok(dtslAcc);
        }


        [HttpPut("changeStateAcc")]
        public IActionResult ChangeStateAcc([FromBody] AccState options)
        {

            var result = _accounts.ChangeStateAccount(options.account, options.state);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }

            return Ok();
        }


    }
}
