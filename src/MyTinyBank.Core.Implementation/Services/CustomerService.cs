using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;

using MyTinyBank.Core.Constants;
using MyTinyBank.Core.Implementation.Data;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services;
using MyTinyBank.Core.Services.Options;
using System.Collections.Generic;

namespace MyTinyBank.Core.Implementation.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly MyTinyBankDbContext _dbContext;

        public CustomerService(MyTinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApiResult<Customer> RegisterCustomer(RegisterCustomerOptions options)
        {
            if (options == null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Firstname))
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Firstname)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Lastname))
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Lastname)}"
                };
            }

            if (options.CustType == CustomerType.Undefined)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid customer type {nameof(options.CustType)}"
                };
            }

            if (!IsValidVatNumber(options.CountryCode, options.VatNumber))
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid Vat number {options.VatNumber}"
                };
            }

            var customerVat = SearchCustomer(new SearchCustomerOptions() 
                                         {
                                            VatNumber = options.VatNumber 
                                         }).SingleOrDefault();
            if (customerVat != null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.Conflict,
                    ErrorText = $"Customer with Vat number {options.VatNumber} already exists"
                };
            }


            var customer = new Customer()
            {
                Firstname = options.Firstname,
                Lastname = options.Lastname,
                VatNumber = options.VatNumber,
                Email = options.Email,
                CountryCode = options.CountryCode,
                Address = options.Address,
                Phone = options.Phone,
                DateOfBirth = options.DateOfBirth,
                CustType = options.CustType
            };

            _dbContext.Add(customer);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Customer could not be saved --> "+ex.ToString()
                };
            }

            return new ApiResult<Customer>()
            {
                Data = customer
            };
        }

        public IQueryable<Customer> SearchCustomer(SearchCustomerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // SELECT FROM CUSTOMER
            var q = _dbContext.Set<Customer>()
                //.Include(cust => cust.Accounts)//soso afta ta include kai then include ta exo sxoliasei giati tha mou eskage me cycle i anazitisi sto webapi pou kanoue stous customers sto table.
                //.ThenInclude(acc => acc.Cards)                  
                .AsQueryable();

            // SELECT FROM CUSTOMER WHERE CustomerId = options.CustomerId
            if (options.CustomerId != null)//Gia to Guid elegxos mono me to null. gia ta ipoloipa string IsNullOrWhiteSpace
            {
                q = q.Where(cust => cust.CustomerId == options.CustomerId);
            }

            // SELECT FROM CUSTOMER WHERE CustomerId = options.CustomerId
            // AND VatNumber = options.VatNumber
            if (!string.IsNullOrWhiteSpace(options.VatNumber))
            {
                q = q.Where(cust => cust.VatNumber == options.VatNumber);
            }

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                q = q.Where(cust => cust.Lastname == options.LastName);
            }

            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                q = q.Where(cust => cust.Firstname == options.FirstName);
            }

            if (options.CountryCodes.Count > 0)
            {
                q = q.Where(cust => options.CountryCodes.Contains(cust.CountryCode));
            }

            //if (options.TrackResults != null &&
            //  !options.TrackResults.Value)
            //{
            //    q = q.AsNoTracking();
            //}

            //if (options.Skip != null)
            //{
            //    q = q.Skip(options.Skip.Value);
            //}

            //q = q.Take(options.MaxResults ?? 500);

            return q;
        }

        public ApiResult<Customer> GetCustomerById(Guid? customerId)
        {
            if (customerId == null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(customerId)}"
                };
            }

            var customer = _dbContext.Set<Customer>()                
                           .Include(cust => cust.Accounts)
                           .ThenInclude(acc => acc.Cards)
                           .Where(cust => cust.CustomerId == customerId)
                           .SingleOrDefault();

            if (customer != null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.Success,
                    Data = customer
                };
            }
            else
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Customer ID {customerId} not found"
                };
            }
        }

        public ApiResult<Customer> UpdateCustomer(Guid? customerId, UpdateCustomerOptions options)
        {
            if (options == null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Bad request."
                };
            }

            if (customerId == null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Bad request {nameof(customerId)}."
                };
            }

            //var customer = GetCustomerById(customerId);
            var queryStr = SearchCustomer(new SearchCustomerOptions() 
                           {
                                CustomerId = customerId
                           });
            var customer = queryStr.SingleOrDefault();
            if (customer == null)
            {
                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Customer with id {customerId} not found."
                };
            }

            if (!string.IsNullOrWhiteSpace(options.Firstname))
            {
                customer.Firstname = options.Firstname;
            }

            if (!string.IsNullOrWhiteSpace(options.Lastname))
            {
                customer.Lastname = options.Lastname;
            }

            if (!string.IsNullOrWhiteSpace(options.Email))
            {
                customer.Email = options.Email;
            }

            if (!string.IsNullOrWhiteSpace(options.Address))
            {
                customer.Address = options.Address;
            }

            if (!string.IsNullOrWhiteSpace(options.Phone))
            {
                customer.Phone = options.Phone;
            }

            if (!string.IsNullOrWhiteSpace(options.DateOfBirth))
            {
                customer.DateOfBirth = options.DateOfBirth;
            }


            if (options.CustType != CustomerType.Undefined)
            {
                customer.CustType = options.CustType;
            }


            if (!string.IsNullOrWhiteSpace(options.CountryCode))
            {
                if (Country.VatLength.TryGetValue(options.CountryCode, out var vatLength))
                {
                    customer.CountryCode = options.CountryCode;
                }
                else
                {
                    return new ApiResult<Customer>()
                    {
                        Code = ApiResultCode.Conflict,
                        ErrorText = $"Something went wrong with country code {options.CountryCode}"
                    };
                }
            }

            if (!string.IsNullOrWhiteSpace(options.VatNumber))
            {
                if (IsValidVatNumber(customer.CountryCode, options.VatNumber))
                {
                    var customerVat = SearchCustomer(new SearchCustomerOptions()
                    {
                        VatNumber = options.VatNumber
                    }).SingleOrDefault();
                    if (customerVat != null && customerVat.VatNumber != customer.VatNumber)
                    {
                        return new ApiResult<Customer>()
                        {
                            Code = ApiResultCode.Conflict,
                            ErrorText = $"Customer with Vat number {options.VatNumber} already exists"
                        };
                    }
                    else
                    {
                        customer.VatNumber = options.VatNumber;
                    }
                }
                else
                {
                    return new ApiResult<Customer>()
                    {
                        Code = ApiResultCode.BadRequest,
                        ErrorText = $"Country Code {options.CountryCode} disagree with vat number {options.VatNumber}."
                    };
                }
            }

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return new ApiResult<Customer>()
                {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Customer could not be updated --> " + ex.ToString()
                };
            }

            return new ApiResult<Customer>()
            {
                Data = customer
            };    
        }

        public bool IsValidVatNumber(string countryCode, string vatNumber)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(vatNumber))
            {
                return false;
            }

            if (Country.VatLength.TryGetValue(countryCode, out var vatLength))
            {
                if (vatLength == vatNumber.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public ApiResult<List<Card>> GetAllCardsByCustId()
        {
            //kapoia vasika stoixeia apo ton pinaka ton pelaton
            var result1 = _dbContext.Set<Customer>()
                          .Include(cust => cust.Accounts)
                          .ThenInclude(acc => acc.Cards)
                          .Select(cust => new
                          {
                              onoma = cust.Firstname,
                              eponimo = cust.Lastname,
                              vat = cust.VatNumber,
                              accounts = cust.Accounts,
                              countAccounts = cust.Accounts.Count
                          }).ToList();

            //oloi oi logariasmoi olon ton pelaton
            var result2 = _dbContext.Set<Customer>()
                          .Include(cust => cust.Accounts)
                          .ThenInclude(acc => acc.Cards)
                          .SelectMany(cust => cust.Accounts)
                          .ToList();

            //oles oi kartes pou exoun oloi oi logariasmoi olon ton customers
            var result3 = _dbContext.Set<Customer>()
                          .Include(cust => cust.Accounts)
                          .ThenInclude(acc => acc.Cards)
                          .SelectMany(cust => cust.Accounts)
                          .SelectMany(acc => acc.Cards)
                          .ToList();

            //ola ta accounts apo ta opoia epilegoume to accountId kai tis kartes pou exoun sindedemenes epano tous
            var result4 = _dbContext.Set<Account>()
                         .Include(acc => acc.Cards)
                         .Select(acc => new
                         {
                             account = acc.AccountId,
                             card = acc.Cards
                         }).ToList();



            return null;
        }

        public ApiResult<List<AccCard>> GetAllCardswithAccByCustId(Guid customerId)
        {

            var result2 = _dbContext.Set<Account>()
                         .Where(acc => acc.CustomerId == customerId)
                         .Include(acc => acc.Cards)
                         .Select(acc => new
                         { 
                            account = acc.AccountId,
                            cards = acc.Cards
                         })
                         .ToList();

            List<AccCard> lista = new List<AccCard>();
            List<string> tmpList;
            AccCard acrd;
            foreach (var r in result2)
            {
                acrd = new AccCard();
                tmpList = new List<string>();

                acrd.CustomerId = customerId;
                acrd.Account = r.account;


                foreach (var c in r.cards)
                {
                    tmpList.Add(c.CardNumber);
                }
                acrd.ListaCard = tmpList;

                lista.Add(acrd);
            }

            return new ApiResult<List<AccCard>>()
            {
                Code = ApiResultCode.Success,
                Data = lista
            };

        }

        public ApiResult<List<AccCardTwo>> GetAllCardswithAccByCustIdTwo(Guid customerId)
        {

            var result2 = _dbContext.Set<Account>()
                         .Where(acc => acc.CustomerId == customerId)
                         .Include(acc => acc.Cards)
                         .Select(acc => new
                         {
                             account = acc.AccountId,
                             cards = acc.Cards
                         })
                         .ToList();

            List<AccCardTwo> lista = new List<AccCardTwo>();
            AccCardTwo acrd;
            foreach (var r in result2)
            {
                if (r.cards.Count == 0)
                {
                    acrd = new AccCardTwo();

                    acrd.Account = r.account;
                    acrd.Card = "-";
                    lista.Add(acrd);
                }
                else
                {
                    foreach (var c in r.cards)
                    {
                        acrd = new AccCardTwo();

                        acrd.Account = r.account;
                        acrd.Card = c.CardNumber;
                        lista.Add(acrd);
                    }
                }
            }

            return new ApiResult<List<AccCardTwo>>()
            {
                Code = ApiResultCode.Success,
                Data = lista
            };

        }

    }
}
