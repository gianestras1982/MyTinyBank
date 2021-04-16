using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTinyBank.Core.Constants
{
    public class AccCard
    {
        public Guid CustomerId { get; set; }
        public string Account { get; set; }
        public List<string> ListaCard { get; set; }
    }
}
