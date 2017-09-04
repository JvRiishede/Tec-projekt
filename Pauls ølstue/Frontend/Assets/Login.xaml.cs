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


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frontend.Assets
{
    public sealed partial class Login : ContentDialog
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void LoginProcess()
        {
            string password = Password.Text;
            string username = Brugernavn.Text;
            string passwordHash;

            var dbConn =DBConnection.Instance();
            if (dbConn.IsConnect())
            {
                string query = "Select KodeHash FROM Bruger where VærelseNr='"+username+"';";
                var cmd = new MySqlCommand(query, dbConn.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    passwordHash=reader.GetString(0);
                    //brugere.Add(reader.GetInt32(2).ToString() + ", " + reader.GetString(0) + " " + reader.GetString(1));
                }
                dbConn.Close();
            }

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            LoginProcess();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }
    }
}
