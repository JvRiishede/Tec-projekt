using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Data.Interface;
using Model;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IFormAuthenticationService _authenticationService;

        public AccountController(IUserService userService, IFormAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        public object Login([FromUri]int roomnr, string password)
        {
            var user = _userService.FindByRoomAndPassword(roomnr, password);
            if (user != null)
            {
                return new
                {
                    User = user,
                    Token = _authenticationService.SetToken(roomnr.ToString())
                };
            }
            return new { };
        }
        [AllowAnonymous]
        [HttpGet]
        public Dictionary<string, Token> CurrentList()
        {
            return _authenticationService.CurrentList();
        }
    }
}
