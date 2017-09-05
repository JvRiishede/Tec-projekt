using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Drink
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public DateTime Tidsstempel { get; set; }
        public List<Vare> Ingrediense { get; set; }
    }
}
