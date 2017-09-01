using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace Backend.HelperClasses
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params Privileges[] roles)
        {
            var allowedRoles = roles.Select(a => Enum.GetName(typeof(Privileges), a));
            Roles = string.Join(",", allowedRoles);
        }
    }
}