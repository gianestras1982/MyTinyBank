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
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly IAccountService _accounts;
        private readonly ILogger<HomeController> _logger;
        private readonly MyTinyBankDbContext _dbContext;

        public CustomerController(MyTinyBankDbContext dbContext, ILogger<HomeController> logger, ICustomerService customers, IAccountService accounts)
        {
            _logger = logger;
            _customers = customers;
            _accounts = accounts;
            _dbContext = dbContext;
        }

        [HttpGet]//https://localhost:5001/customer
        public IActionResult Index()
        {
            var options = new SearchCustomerOptions
            {
                MaxResults = 100
            };

            var customers = _customers.SearchCustomer(options)
                            .OrderByDescending(c => c.AuditInfo.Created)
                            .ToList();

            return View(customers);
        }


        //search
        [HttpGet("search")]
        public IActionResult Search(SearchCustomerOptions options)
        {
            var customers = _customers.SearchCustomer(options)//sos exo kopsei ta include se account kai karts gia na min peso se cycle logo return json. an itan return view de tha ipirxe thema.
                            .OrderByDescending(c => c.AuditInfo.Created)
                            .ToList();

            return Json(customers);
            //edo tha mporousame na valoume kai return Ok(customer).
        }


        //add customer form
        [HttpGet("addCustomerForm")]
        public IActionResult AddCustomerForm()
        {
            return View();
        }

        //add customer save
        [HttpPut("addCustomerSave")]
        public IActionResult AddCustomerSave([FromBody] RegisterCustomerOptions options)
        {
            var result = _customers.RegisterCustomer(options);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }

            return Ok();
        }

        //update
        [HttpPut("{id:guid}")]
        public IActionResult UpdateDetails(Guid id, [FromBody] UpdateCustomerOptions options)
        {
            var result = _customers.UpdateCustomer(id, options);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }
            return Ok();
        }


        //get
        [HttpGet("{id:guid}")]
        public IActionResult GetCustomerDetails(Guid id)
        {
            var customer = _customers.GetCustomerById(id).Data;

            return View(customer);
        }



        [HttpGet("{accountId}/accountDtls")]
        public IActionResult AccountDtls(string accountId)
        {
            var result = _accounts.GetAccountByAccountId(accountId).Data;

            return Json(result);
            //return View(result);
        }


        [HttpGet("accCard/{customerId:guid}")]
        public IActionResult GetCards(Guid customerId)
        {
            var result = _customers.GetAllCardswithAccByCustId(customerId);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }

            var r = result.Data;

            return View(r);
        }



        [HttpGet("accCardTwo/{customerId:guid}")]
        public IActionResult GetCardsTwo(Guid customerId)
        {
            var result = _customers.GetAllCardswithAccByCustIdTwo(customerId);

            if (!result.IsSuccessful())
            {
                return result.ToActionResult();
            }

            var r = result.Data;

            return Json(r);
        }



        [HttpPost]
        public IActionResult Register([FromBody] Core.Services.Options.RegisterCustomerOptions options)
        {
            return Ok(options);
        }
    }
}
