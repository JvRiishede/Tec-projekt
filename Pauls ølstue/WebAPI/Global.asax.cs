using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated)
            {
                HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(cookie.Value);
                    if (authTicket == null)
                    {
                        return;
                    }

                    var roles = authTicket.UserData.Split(':')[1].Split(';');
                    var user = new GenericPrincipal(context.User.Identity, roles);
                    context.User = Thread.CurrentPrincipal = user;
                }
            }

        }
    }
}
