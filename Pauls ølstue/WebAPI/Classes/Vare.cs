using Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebAPI.Classes
{
    public class Vare
    {
        public int VareId { get; set; }
        public string VareNavn { get; set; }
        public int VarePris { get; set; }
        public bool ErDrink { get; set; }
        public DateTime Tidsstempel { get; set; }
    }
    public class VareManager
    {
        public static List<Vare> Varer()
        {
            var varer = new List<Vare>();
            var dbConn = DBConnection.Instance();
            if (dbConn.IsConnect())
            {
                string query = "Select Id, Navn, Pris, Tidsstempel FROM Vare;";
                var cmd = new MySqlCommand(query, dbConn.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    varer.Add(new Vare { VareId = reader.GetInt32(0), VareNavn = reader.GetString(1), VarePris = reader.GetInt32(2), Tidsstempel = reader.GetDateTime(3), ErDrink = false });
                }
                reader.Close();


                query = "Select Id, Navn, Tidsstempel FROM Drink";
                cmd = new MySqlCommand(query, dbConn.Connection);
                var reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    varer.Add(new Vare { VareId = reader2.GetInt32(0), VareNavn = reader2.GetString(1), Tidsstempel = reader2.GetDateTime(2), ErDrink = true });
                }
                dbConn.Close();
            }
            return varer;
        }
    }
}