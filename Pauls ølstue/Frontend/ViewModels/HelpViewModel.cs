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
using Frontend;
using Frontend.Assets;

namespace ViewModels
{
    public class HelpViewModel
    {
        public ObservableCollection<Help> HelpList;
        public HelpViewModel()
        {
            HelpList = new ObservableCollection<Help>();
        }
        public async Task LoadAsync()
        {
            //API connection
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + App.loginToken);

            HttpResponseMessage response;
            response = await client.GetAsync("api/help/GetAllHelp");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var test = await response.Content.ReadAsStringAsync();
                    var midt = JsonConvert.DeserializeObject<List<Help>>(test);
                    foreach (var item in midt)
                    {
                        HelpList.Add(item);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Frontend");
                    Debug.WriteLine(e.Message);
                }
            }
            else
            {
                Login ny = new Login();
                await ny.ShowAsync();
            }
        }
    }
}
