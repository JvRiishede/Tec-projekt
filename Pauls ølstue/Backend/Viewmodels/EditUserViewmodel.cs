using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace Backend.Viewmodels
{
    public class EditUserViewmodel
    {
        public User User { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}