using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.Viewmodels;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Model;
namespace Backend.Interfaces
{
    public interface IFormAuthenticationService
    {
        void SetCookie(User user, bool isPersistent);
        void Signout();
    }
}