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
using MyTinyBank.Web.Models;

namespace MyTinyBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyTinyBankDbContext _dbContext;
        private readonly ICustomerService _customers;
        private readonly IAccountService _accounts;
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(MyTinyBankDbContext dbContext, ILogger<HomeController> logger, ICustomerService customers, IAccountService accounts)
        {
            _dbContext = dbContext;
            _logger = logger;
            _customers = customers;
            _accounts = accounts;
        }

        public IActionResult Index()
        {


            //var results1 = _dbContext.Set<Customer>()
            //             .Include(cust => cust.Accounts)
            //             //.ThenInclude(acc => acc.Cards)
            //             .Where(cust => cust.CustomerId == new Guid("C0366944-D2BD-4F78-8297-C0E9E2E8391D"))
            //             .Select(a => new
            //                            {
            //                                Eponimo = a.Lastname,
            //                                Onoma = a.Firstname,
            //                                Logariasmoi = a.Accounts
            //                            }
            //                     ).SingleOrDefault();




            //var results3 = _dbContext.Set<Customer>()
            //               .Include(cust => cust.Accounts)
            //               .ThenInclude(acc => acc.Cards)
            //               .SelectMany(a => a.Accounts)
            //               .ToList();

            //var results4 = results3
            //               .SelectMany(a => a.Cards)
            //               .ToList();




            //var accounts = _dbContext.Set<Account>()
            //    .Include(a => a.Customer)
            //    .Select(a => new
            //    {
            //        AccountId = a.AccountId,
            //        Description = a.Description,
            //        Customer = new
            //        {
            //            FirstName = a.Customer.Firstname,
            //            LastName = a.Customer.Lastname
            //        }
            //    })
            //    .ToList();

            //return Json(accounts);

            //return View();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
