using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.Interfaces;
using Backend.Viewmodels;
using Data.Interface;

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
        public ActionResult LogIn(LoginViewmodel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _userService.FindByRoomAndPassword(model.VærelseNr, model.Password);
            if (user != null)
            {
                _authenticationService.SetCookie(user, model.RememberMe);
                Session["Fullname"] = user.Fornavn + " " + user.Efternavn;
                return Redirect("/Dashboard/Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult MyPage()
        {

            var model = new MyPageViewmodel
            {
                User = _userService.FindById(_userService.UserId)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult MyPage(MyPageViewmodel model, string command)
        {
            switch (command)
            {
                case "saveProfile":
                    _userService.SaveProfile(model.User.Fornavn, model.User.Efternavn, model.User.Email, _userService.UserId);
                    break;
                case "saveLogin":
                    _userService.SaveCredentials(model.User.VærelseNr, model.Password, _userService.UserId);
                    break;
                case "saveImage":
                    var file = new byte[model.Image.ContentLength];
                    model.Image.InputStream.Read(file, 0, model.Image.ContentLength);
                    _userService.SaveImage(file, _userService.UserId);
                    break;
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            _authenticationService.Signout();
            return RedirectToAction("LogIn");
        }
    }
}