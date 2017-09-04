using Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Model;

namespace WebAPI.Controllers
{
    public class VarerController : ApiController
    {
        private readonly IUserService _userService;
        public VarerController(IUserService userService)
        {
            _userService = userService;
        }

    }
}
