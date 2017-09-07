using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject.Web.WebApi;
using Data.Interface;
using Model;
using System.Web.Http.Cors;
using WebAPI.Classes;

namespace WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizeApi]
    public class HelpController : ApiController
    {
        private readonly IHelpService _helpService;

        public HelpController(IHelpService helpService)
        {
            _helpService = helpService;
        }

        public List<Help> GetAllHelp()
        {
            return _helpService.GetAllHelp();
        }
    }
}
