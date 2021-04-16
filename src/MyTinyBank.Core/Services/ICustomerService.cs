using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTinyBank.Core.Constants;
using MyTinyBank.Core.Model;
using MyTinyBank.Core.Services.Options;

namespace MyTinyBank.Core.Services
{
    public interface ICustomerService
    {
        public ApiResult<Customer> RegisterCustomer(RegisterCustomerOptions options);
        public IQueryable<Customer> SearchCustomer(SearchCustomerOptions options);
        public ApiResult<Customer> GetCustomerById(Guid? customerId);
        public ApiResult<Customer> UpdateCustomer(Guid? customerId, UpdateCustomerOptions options);
        public ApiResult<List<Card>> GetAllCardsByCustId();
        public ApiResult<List<AccCard>> GetAllCardswithAccByCustId(Guid customerId);
        public ApiResult<List<AccCardTwo>> GetAllCardswithAccByCustIdTwo(Guid customerId);
    }
}
