using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Data.Interface;
using Data.Service;

namespace WebAPI.Classes
{
    public class AuthorizeApi : AuthorizeAttribute
    {
        private readonly IFormAuthenticationService _authenticationService;

        public AuthorizeApi()
        {
            _authenticationService = new FormAuthenticationService();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization?.Parameter;
            if (token == null || !_authenticationService.ValidateToken(token))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}