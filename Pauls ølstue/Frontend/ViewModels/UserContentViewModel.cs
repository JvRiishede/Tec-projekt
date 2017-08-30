using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Data;
using Frontend.ViewModels;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace ViewModels
{
    public class UserContentViewModel:BaseViewmodel
    {
        public ObservableCollection<string> brugere = new ObservableCollection<string>();
        UserContentModel UCM;

        public UserContentViewModel()
        {
            UCM = new UserContentModel();
            //Load();
        }
        
        public void Load()
        {
            var dbConn = DBConnection.Instance();
            if(dbConn.IsConnect())
            {
                string query = "Select Fornavn, Efternavn, VærelseNr FROM Bruger;";
                var cmd = new MySqlCommand(query, dbConn.Connection);
                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    brugere.Add(reader.GetInt32(2).ToString() + ", " + reader.GetString(0) + " " + reader.GetString(1));
                    Debug.WriteLine(reader.GetInt32(2).ToString() + ", " + reader.GetString(0) + " " + reader.GetString(1));
                }
                dbConn.Close();
            }
        }

        public void Create()
        {
            var dbconn = DBConnection.Instance();
            //dbconn.DatabaseName = "PaulsData";
            if(dbconn.IsConnect())
            {
                string query = "Insert into Bruger (Fornavn) values ('Smiley')";
                var cmd = new MySqlCommand(query, dbconn.Connection);
                cmd.ExecuteNonQuery();
                dbconn.Close();
            }            
        }
        public void Farve()
        {
            UCM.Farve();
            Debug.WriteLine("Hello UCVM");
        }
    }
}
