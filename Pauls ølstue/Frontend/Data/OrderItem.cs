using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool ErDrink { get; set; }
        public string Full { get; set; }
        public void Combine()
        {
            Full = Name + "   " + "Antal: " + Amount.ToString();
        }
    }
}
