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
        public ObservableCollection<VareForList> IndkobListe;
        public ObservableCollection<string> brugere;
        public List<VareForList> TempList;
        public List<Data.Vare> vare;
        UserContentModel UCM;

        public UserContentViewModel()
        {
            UCM = new UserContentModel();
            IndkobListe = new ObservableCollection<VareForList>();
            brugere = new ObservableCollection<string>();
        }

        public class VareForList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Amount { get; set; }
            public bool ErDrink { get; set; }
            public string Full { get; set; }
            public void Combine()
            {
                Full = Name + "," + Amount.ToString();
            }
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
                }
                dbConn.Close();
            }
        }
        
        public void Buy()
        {
            int FuldPris = 0;
            for (int i = 0; i < IndkobListe.Count; i++)
            {
                for (int j = 0; j < vare.Count; j++)
                {
                    if(vare[j].ErDrink && IndkobListe[i].Name==vare[j].VareNavn)
                    {
                        IndkobListe[i].ErDrink = true;
                    }
                    if(IndkobListe[i].Id==vare[j].VareId)
                    {
                        FuldPris += IndkobListe[i].Amount * vare[j].VarePris;
                    }
                }
            }
            var dbconn = DBConnection.Instance();
            if (dbconn.IsConnect())
            {
                string query = "Insert into Ordre (BrugerId, Pris) values (4,+"+FuldPris+")";// OBS mangler korrekt brugerId!!!!
                var cmd = new MySqlCommand(query, dbconn.Connection);
                int succeed =cmd.ExecuteNonQuery();
                if(succeed>0)
                {
                    cmd = new MySqlCommand("Select MAX(Id) from Ordre", dbconn.Connection);
                    var reader = cmd.ExecuteReader();
                    int OrdreId = 0;
                    while (reader.Read())
                    { OrdreId = reader.GetInt32(0); }
                    reader.Close();
                    query = "Insert into Ordre_Drink_Vare (OrdreId, DrinkId, VareId) values ";
                    for (int i = 0; i < IndkobListe.Count; i++)
                    {
                        for (int j = 0; j < IndkobListe[i].Amount; j++)
                        {
                            int DrinkId = 0, VareId = 0;
                            if (IndkobListe[i].ErDrink)
                            {
                                DrinkId = IndkobListe[i].Id;
                            }
                            else
                                VareId = IndkobListe[i].Id;

                            query += "(" + OrdreId + ", " + DrinkId + ", " + VareId + "), ";
                        }
                    }
                    query = query.Substring(0, query.Length - 2);
                    query += ";";
                    cmd = new MySqlCommand(query, dbconn.Connection);
                    cmd.ExecuteNonQuery();
                }
                dbconn.Close();
            }
        }
    }
}
