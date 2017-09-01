using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum Privileges
    {
        Bartender = 1,
        Administrator = 2
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
