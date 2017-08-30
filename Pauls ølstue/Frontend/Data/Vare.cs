using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Data
{
    public class Vare
    {
        public int VareId { get; set; }
        public string VareNavn { get; set; }
        public int VarePris { get; set;}
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
                    varer.Add(new Vare { VareId = reader.GetInt32(0), VareNavn = reader.GetString(1), VarePris = reader.GetInt32(2),Tidsstempel=reader.GetDateTime(3)});
                    Debug.WriteLine("" + reader.GetInt32(0) + "," + reader.GetString(1) + "," + reader.GetInt32(2));
                }
                dbConn.Close();
            }
            return varer;
        }
    }
}
