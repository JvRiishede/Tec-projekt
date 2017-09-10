using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace Backend.Viewmodels
{
    public class DashboardViewmodel
    {
        public int UserTotal { get; set; }
        public int VarerTotal { get; set; }
        public int DrinksTotal { get; set; }
        public int OrdreTotal { get; set; }

        public List<ItemSold> TopVare { get; set; }
        public List<ItemSold> TopDrink { get; set; }
        public List<ItemSold> TopBruger { get; set; }
    }
}