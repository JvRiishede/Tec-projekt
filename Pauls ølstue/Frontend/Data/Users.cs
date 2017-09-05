using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data
{
    public class User
    {
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public int VærelseNr { get; set; }
        public string Email { get; set; }
        public byte[] Billede { get; set; }
        public Role Role { get; set; }
    }
}
