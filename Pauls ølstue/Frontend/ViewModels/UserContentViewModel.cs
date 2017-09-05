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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ViewModels
{
    public class UserContentViewModel:BaseViewmodel
    {
        
        public ObservableCollection<VareForList> IndkobListe;
        public ObservableCollection<User> brugere;
        public List<VareForList> TempList;
        public ObservableCollection<Vare> vare;
        public ObservableCollection<string> brugereCombo;
        public int BrugerId;
        UserContentModel UCM;

        public UserContentViewModel()
        {
            UCM = new UserContentModel();
            IndkobListe = new ObservableCollection<VareForList>();
            brugere = new ObservableCollection<User>();
            vare = new ObservableCollection<Vare>();
            brugereCombo = new ObservableCollection<string>();
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
                Full = Name + "   " + "Antal: " + Amount.ToString();
            }
        }

        public async Task LoadVarerAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            response = await client.GetAsync("api/varer/getproducts");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                try
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var midt = JsonConvert.DeserializeObject<List<Vare>>(test);
                    foreach (var item in midt)
                    {
                        vare.Add(item);
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            };
        }

        public async Task LoadAsync()
        {
            await LoadVarerAsync();
            //API connection
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            response = await client.GetAsync("api/users/GetUsers");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                try
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var midt = JsonConvert.DeserializeObject<List<User>>(test);
                    foreach (var item in midt)
                    {
                        brugere.Add(item);
                    }
                    for (int i = 0; i < brugere.Count; i++)
                    {
                        brugereCombo.Add(brugere[i].VærelseNr.ToString()+" "+brugere[i].Fornavn+" "+brugere[i].Efternavn);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            };
        }
        
        public void Buy()
        {
            int FuldPris = 0;
            for (int i = 0; i < IndkobListe.Count; i++)
            {
                for (int j = 0; j < vare.Count; j++)
                {
                    //if(vare[j].ErDrink && IndkobListe[i].Name==vare[j].VareNavn)
                    //{
                    //    IndkobListe[i].ErDrink = true;
                    //}
                    //if(IndkobListe[i].Id==vare[j].VareId)
                    //{
                    //    FuldPris += IndkobListe[i].Amount * vare[j].VarePris;
                    //}
                }
            }
            var dbconn = DBConnection.Instance();
            if (dbconn.IsConnect())
            {
                string query = "Insert into Ordre (BrugerId, Pris) values ("+BrugerId+",+"+FuldPris+")";// OBS mangler korrekt brugerId!!!!
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
