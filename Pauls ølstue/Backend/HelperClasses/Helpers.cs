using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Model;

namespace Backend.HelperClasses
{
    public static class Helpers
    {
        public static bool IsInRoles(this IPrincipal principal, params Privileges[] roles)
        {
            return roles.All(a => principal.IsInRole(a.ToString()));
        }
    }
}