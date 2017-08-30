using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.Viewmodels
{
    public class MyPageViewmodel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public int RoomNr { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


        public HttpPostedFileBase Image { get; set; }
    }
}