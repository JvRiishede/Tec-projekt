using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        public int Id { get; set; }
        public int BrugerId { get; set; }
        public decimal Pris { get; set; }
        public DateTime Tidsstempel { get; set; }
    }
}
