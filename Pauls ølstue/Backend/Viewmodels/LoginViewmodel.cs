using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Viewmodels
{
    public class LoginViewmodel
    {
        public int VærelseNr { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}