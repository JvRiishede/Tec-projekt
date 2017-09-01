using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Backend.HelperClasses;
using Model;

namespace Backend.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeRoles(Privileges.Administrator, Privileges.Bartender)]
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}