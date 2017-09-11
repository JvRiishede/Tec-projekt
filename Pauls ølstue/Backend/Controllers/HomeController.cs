using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Backend.HelperClasses;
using Data.Interface;
using Model;

namespace Backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [AuthorizeRoles(Privileges.Administrator, Privileges.Bartender)]
        public ActionResult Index()
        {
            return View(_userService.UserId);
        }
    }
}