using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public string QuickpayGuid { get; set; }
        public int OrdreId { get; set; }
        public decimal Beløb { get; set; }
        public DateTime Tidsstempel { get; set; }
    }
}
