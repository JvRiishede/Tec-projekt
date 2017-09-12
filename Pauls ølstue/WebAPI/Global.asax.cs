using System;
using System.Collections.Generic;
using System.Configuration;
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
using Model;
using Timer = System.Timers.Timer;

namespace WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IFormAuthenticationService _authenticationService;
        private readonly IPropertyService _propertyService;
        private readonly IQuickpayService _quickpayService;
        private double _timerInterval;

        public WebApiApplication()
        {
            var connectionService = new ConnectionInformationService(ConfigurationManager.ConnectionStrings["PaulsData"].ConnectionString);
            _authenticationService = new FormAuthenticationService();
            _propertyService = new PropertyService(connectionService);
            _quickpayService = new QuickpayService();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            _timerInterval = Convert.ToDouble(WebConfigurationManager.AppSettings["TimerIntervalInMilliseconds"]);
            var timer = new Timer(_timerInterval)
            {
                    Enabled = true
            };
            timer.Elapsed += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, ElapsedEventArgs args)
        {
            var current = DateTime.Now;
            var startRunTime = new DateTime(current.Year, current.Month, current.Day, 16,0,0);
            var latestRunTime = startRunTime.AddMilliseconds(_timerInterval);
            var interval = _propertyService.GetProperty(Properties.InvoiceInterval, MailIntervals.FirstDayInMonth);
            switch (interval)
            {
                case MailIntervals.FirstDayInMonth:
                    if (current.Day == 1 && current.CompareTo(startRunTime) >= 0 && current.CompareTo(latestRunTime) <= 0)
                    {
                        
                    }
                    break;
                case MailIntervals.EachDayInMonth:
                    if (current.CompareTo(startRunTime) >= 0 && current.CompareTo(latestRunTime) <= 0)
                    {

                    }
                    break;
                case MailIntervals.LastDayInMonth:
                    if ((current.Day == 30 || current.Day == 31) && current.CompareTo(startRunTime) >= 0 && current.CompareTo(latestRunTime) <= 0)
                    {

                    }
                    break;
            }

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
