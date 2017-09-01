using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.Viewmodels;
using Data.Interface;

namespace Backend.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Users
        public ActionResult Index()
        {
            var model = new UsersViewmodel
            {
                Users = _userService.GetUsers().ToList()
            };
            return View(model);
        }
    }
}