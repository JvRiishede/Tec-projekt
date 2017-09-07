using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Data.Interface;
using Model;

namespace Data.Service
{
    public class FormAuthenticationService : IFormAuthenticationService
    {
        private static Dictionary<string, Token> _authorizedTokens;

        public FormAuthenticationService()
        {
            if (_authorizedTokens == null)
            {
                _authorizedTokens = new Dictionary<string, Token>();
            }
        }
        public void SetCookie(User user, string token, bool isPersistent)
        {
            var ticket = new FormsAuthenticationTicket(1, token, DateTime.Now, DateTime.Now.AddYears(1), isPersistent, string.Format("{0}:{1}:{2}", user.Id, token, user.Role.Name));

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

        public string SetToken(string username)
        {
            var guid = Guid.NewGuid().ToString();
            _authorizedTokens.Add(guid, new Token
            {
                Username = username,
                Expire = DateTime.Now.AddHours(2)
            });
            return guid;
        }

        public bool ValidateToken(string token)
        {
            _authorizedTokens.TryGetValue(token, out Token authToken);
            if (authToken != null && authToken.Expire >= DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public void Signout()
        {
            FormsAuthentication.SignOut();
        }
    }
}