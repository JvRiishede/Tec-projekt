using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backend.HelperClasses;
using Model;

namespace Backend.Controllers
{
    [AuthorizeRoles(Privileges.Administrator)]
    public class BaseController : Controller
    {
       
    }
}