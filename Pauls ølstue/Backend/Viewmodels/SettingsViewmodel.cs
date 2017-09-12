using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backend.Viewmodels
{
    public class SettingsViewmodel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Displayname { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        
        public string Currency { get; set; }
        public bool DisableTestCards { get; set; }
        public int SelectedInterval { get; set; }
        public IEnumerable<SelectListItem> Intervals { get; set; }
    }
}