using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.Viewmodels;
using Data.Interface;
using Model;
using System.Reflection;

namespace Backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFormAuthenticationService _authenticationService;

        public AccountController(IUserService userService, IFormAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string LogIn(User user, string token, bool rememberMe)
        {
            if (token.Length > 0)
            {
                _authenticationService.SetCookie(user, token, rememberMe);
                return "/Dashboard/Index";
            }
            return "";
        }

        [HttpGet]
        public ActionResult MyPage()
        {

            var model = new EditUserViewmodel()
            {
                User = _userService.FindById(_userService.UserId),
                Roles = _userService.GetRoles().Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
            };

            return View("_EditUser", model);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            _authenticationService.Signout();
            return RedirectToAction("LogIn");
        }
    }
}