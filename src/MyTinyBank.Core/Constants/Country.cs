using System;
using System.Collections.Generic;

namespace MyTinyBank.Core.Constants
{
    public static class Country
    {
        public const string GreekCountryCode = "GR";
        public const string CyprusCountryCode = "CY";
        public const string ItalyCountryCode = "IT";

        public static readonly IReadOnlyDictionary<string, int> VatLength = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) 
        {
            { GreekCountryCode, 9 },
            { CyprusCountryCode, 11 },
            { ItalyCountryCode, 10 }
        };
    }
}
