using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using Ninject.Web.WebApi;
using Data.Interface;
using Model;
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        public List<User> GetUsers()
        {
            return _userService.GetUsers().ToList();
        }

        [HttpPost]
        public object GetPageduser([FromBody]UserSearchTerms terms)
        {
            return new
            {
                Total=_userService.GetUserTotal(),
                User = _userService.GetPagedUser(terms)
            };
        }
    }
}
