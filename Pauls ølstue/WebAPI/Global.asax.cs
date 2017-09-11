using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Timers;
using Data.Interface;
using Data.Service;
using Timer = System.Timers.Timer;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IFormAuthenticationService _authenticationService;

        public WebApiApplication()
        {
            _authenticationService = new FormAuthenticationService();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var timer = new Timer(Convert.ToDouble(WebConfigurationManager.AppSettings["TimerIntervalInMilliseconds"]))
            {
                    Enabled = true
            };
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, ElapsedEventArgs args)
        {
            _authenticationService.ClearOldTokens();
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
