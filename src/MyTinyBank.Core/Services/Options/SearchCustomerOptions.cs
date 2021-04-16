using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Services.Options
{
    public class SearchCustomerOptions
    {
        public Guid? CustomerId { get; set; }
        public string VatNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<string> CountryCodes { get; set; }
        public int? MaxResults { get; set; }
        public bool? TrackResults { get; set; }
        public int? Skip { get; set; }

        public SearchCustomerOptions()
        {
            CountryCodes = new List<string>();
        }
    }
}
