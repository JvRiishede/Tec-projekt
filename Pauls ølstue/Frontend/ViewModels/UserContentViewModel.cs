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
    public class UserContentViewModel : BaseViewmodel
    {

        public ObservableCollection<OrderItem> IndkobListe;
        public ObservableCollection<User> brugere;
        public List<OrderItem> TempList;
        public ObservableCollection<Vare> vare;
        public ObservableCollection<Drink> drink;
        public ObservableCollection<VareItem> vareItem;
        public ObservableCollection<string> brugereCombo;
        public int BrugerId;
        UserContentModel UCM;

        public UserContentViewModel()
        {
            UCM = new UserContentModel();
            IndkobListe = new ObservableCollection<OrderItem>();
            brugere = new ObservableCollection<User>();
            vare = new ObservableCollection<Vare>();
            brugereCombo = new ObservableCollection<string>();
            drink = new ObservableCollection<Drink>();
            vareItem = new ObservableCollection<VareItem>();
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
                        vareItem.Add(new VareItem { Id = item.Id, Navn = item.Navn, ErDrink = false });
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            };
        }

        public async Task LoadDrinkAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            response = await client.GetAsync("api/drink/getproducts");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var midt = JsonConvert.DeserializeObject<List<Drink>>(test);
                    foreach (var item in midt)
                    {
                        drink.Add(item);
                        vareItem.Add(new VareItem { Id = item.Id, Navn = item.Navn, ErDrink = true });
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            };
        }

        public async Task LoadAsync()
        {
            //API connection
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            response = await client.GetAsync("api/users/GetUsers");
            if (response.IsSuccessStatusCode)
            {
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
                        brugereCombo.Add(brugere[i].VærelseNr.ToString() + " " + brugere[i].Fornavn + " " + brugere[i].Efternavn);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            };
        }

        public async Task BuyAsync()
        {
            decimal FuldPris = 0;

            foreach (var item in IndkobListe)
            {
                if (item.ErDrink)
                {
                    for (int i = 0; i < drink.Count; i++)
                    {
                        if (drink[i].Id == item.Id)
                        {
                            for (int j = 0; j < drink[i].Ingrediense.Count; j++)
                            {
                                for (int k = 0; k < vare.Count; k++)
                                {
                                    if (drink[i].Ingrediense[j].Id == vare[k].Id)
                                    {
                                        FuldPris += vare[k].Pris*item.Amount;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vare.Count; i++)
                    {
                        if (vare[i].Id == item.Id)
                        {
                            FuldPris += vare[i].Pris*item.Amount;
                            break;
                        }
                    }
                }
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:52856/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                FullOrder fo = new FullOrder { Brugerid = BrugerId, FuldPris = FuldPris, OrderList = IndkobListe };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Order/placeorder", fo);
                if (response.IsSuccessStatusCode)
                {
                    bool result = await response.Content.ReadAsAsync<bool>();
                    if (result)
                        Debug.WriteLine("Order Submitted");
                    else
                        Debug.WriteLine("An error has occurred");
                }
            }
        }
    }
}
