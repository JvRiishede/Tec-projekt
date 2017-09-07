using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Token
    {
        public string Key { get; set; }
        public string Username { get; set; }
        public DateTime Expire { get; set; }
    }
}
