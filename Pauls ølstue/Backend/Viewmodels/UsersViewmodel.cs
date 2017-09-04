using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Classes;
using Model;

namespace Backend.Viewmodels
{
    public class UsersViewmodel
    {
        public List<User> Users { get; set; }
        public User User { get; set; }
        public UserSort Sort { get; set; }
        public int UserTotal { get; set; }

        public static string Sortable(UserSort sort, string field)
        {
            string icon = "";
            string action = "";
            string fieldName = "";

            switch (field)
            {
                case "RoomNr":
                    fieldName = "Værelse nr";
                    if (sort == UserSort.RoomNrDesc)
                    {
                        icon = "arrow-down";
                        action = "RoomNrAsc";
                    }
                    else if (sort == UserSort.RoomNrAsc)
                    {
                        icon = "arrow-up";
                        action = "RoomNrDesc";
                    }
                    else
                    {
                        action = "RoomNrAsc";
                    }
                    break;
                case "Firstname":
                    fieldName = "Fornavn";
                    if (sort == UserSort.FirstnameDesc)
                    {
                        icon = "arrow-down";
                        action = "FirstnameAsc";
                    }
                    else if (sort == UserSort.FirstnameAsc)
                    {
                        icon = "arrow-up";
                        action = "FirstnameDesc";
                    }
                    else
                    {
                        action = "FirstnameAsc";
                    }
                    break;
                case "Lastname":
                    fieldName = "Efternavn";
                    if (sort == UserSort.LastnameDesc)
                    {
                        icon = "arrow-down";
                        action = "LastnameAsc";
                    }
                    else if (sort == UserSort.LastnameAsc)
                    {
                        icon = "arrow-up";
                        action = "LastnameDesc";
                    }
                    else
                    {
                        action = "LastnameAsc";
                    }
                    break;
                case "Email":
                    fieldName = "Email";
                    if (sort == UserSort.EmailDesc)
                    {
                        icon = "arrow-down";
                        action = "EmailAsc";
                    }
                    else if (sort == UserSort.EmailAsc)
                    {
                        icon = "arrow-up";
                        action = "EmailDesc";
                    }
                    else
                    {
                        action = "EmailAsc";
                    }
                    break;
                case "Type":
                    fieldName = "Type";
                    if (sort == UserSort.TypeDesc)
                    {
                        icon = "arrow-down";
                        action = "TypeAsc";
                    }
                    else if (sort == UserSort.TypeAsc)
                    {
                        icon = "arrow-up";
                        action = "TypeDesc";
                    }
                    else
                    {
                        action = "TypeAsc";
                    }
                    break;
            }

            return string.Format(@"<a data-action=""{0}"">{2} <span class=""fa fa-{1}""></span></a>", action, icon, fieldName);
        }
    }
}