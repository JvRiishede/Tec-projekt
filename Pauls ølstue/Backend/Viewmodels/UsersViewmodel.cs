using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;

namespace Backend.Viewmodels
{
    public class UsersViewmodel
    {
        public List<User> Users { get; set; }
        public User User { get; set; }
    }
}