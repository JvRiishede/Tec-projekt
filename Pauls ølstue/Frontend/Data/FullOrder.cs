using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class FullOrder
    {
        public decimal FuldPris { get; set; }
        public int Brugerid { get; set; }
        public ObservableCollection<OrderItem> OrderList { get; set; }

    }
}
