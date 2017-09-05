using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Data
{
    public class Vare
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public decimal Pris { get; set; }
        public DateTime Tidsstempel { get; set; }
    }

    public class Drink
    {
        public int Id { get; set; }
        public string Navn { get; set; }
        public DateTime Tidsstempel { get; set; }
        public List<Vare> Ingrediense { get; set; }
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
                    varer.Add(new Vare { Id = reader.GetInt32(0), Navn = reader.GetString(1), Pris = reader.GetInt32(2), Tidsstempel = reader.GetDateTime(3)});
                }
                reader.Close();


                query = "Select Id, Navn, Tidsstempel FROM Drink";
                cmd = new MySqlCommand(query, dbConn.Connection);
                var reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    varer.Add(new Vare { Id = reader2.GetInt32(0), Navn = reader2.GetString(1), Tidsstempel = reader2.GetDateTime(2) });
                }
                dbConn.Close();
            }
            return varer;
        }
    }
}
