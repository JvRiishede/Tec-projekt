using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
namespace Data.Interface
{
    public interface IFormAuthenticationService
    {
        void SetCookie(User user, string token, bool isPersistent);
        string SetToken(string username);
        bool ValidateToken(string token);
        Dictionary<string, Token> CurrentList();
        void Signout();
    }
}