using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Data;
using MySql.Data.MySqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Newtonsoft.Json;
using ViewModels;
using Frontend;


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frontend.Assets
{
    public sealed partial class Login : ContentDialog
    {
        public Login()
        {
            this.InitializeComponent();
        }

        public class BrugerLogin
        {
            public User Bruger { get; set; }
            public string Token { get; set; }
        }

        private async System.Threading.Tasks.Task LoginProcessAsync()
        {
            string password = Password.Text;
            string username = Brugernavn.Text;

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52856/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;
            response = await client.PostAsync("api/account/login?roomnr=" + username + "&password=" + password,null);
            if (response.IsSuccessStatusCode)
            {
                var buffer = await response.Content.ReadAsStringAsync();
                var buffer2 = JsonConvert.DeserializeObject<BrugerLogin>(buffer);
                
                App.loginToken = buffer2.Token;
            };
            UserContentViewModel UCVM = new UserContentViewModel();
            UCVM.LoadDrinkAsync();
            UCVM.LoadVarerAsync();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            LoginProcessAsync();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }
    }
}
