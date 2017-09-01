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
            var ticket = new FormsAuthenticationTicket(1, user.Id.ToString(), DateTime.Now, DateTime.Now.AddYears(1), isPersistent,string.Format("{0}:{1}", user.Id, user.Role.Name));

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