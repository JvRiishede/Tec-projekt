using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        public static string GetDisplayname(this Enum enumValue)
        {
            try
            {
                var attr = enumValue.GetType().GetMember(enumValue.ToString()).First()
                    .GetCustomAttribute<DisplayAttribute>();

                if (attr == null)
                {
                    // Denne kommer hvis der ikke er en [Display] på vores enum..
                    return enumValue.ToString();
                }
                return attr.Name;
            }
            catch
            {
                return enumValue.ToString();
            }
        }
    }
}