using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Backend.Interfaces;
using Backend.Viewmodels;
using Model;

namespace Backend.Services
{
    public class FormAuthenticationService : IFormAuthenticationService
    {
        public void SetCookie(User user, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(1, user.VærelseNr.ToString(), DateTime.Now, DateTime.Now.AddYears(1), isPersistent, user.Role.ToString());

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);

        }
        public void Signout()
        {
            FormsAuthentication.SignOut();
        }
    }
}