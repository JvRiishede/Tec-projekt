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
using ViewModels;
using Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frontend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserContent : Page
    {
        public UserContentViewModel UCVM { get; set; }
        public UserContent()
        {
            this.InitializeComponent();
            UCVM = new UserContentViewModel();

            UCVM.LoadAsync();
            UCVM.LoadDrinkAsync();
            UCVM.LoadVarerAsync();

            Products.IsEnabled = false;
            Køb.IsEnabled = false;
            Cancel.IsEnabled = false;
        }

        private void UpdateList()
        {
            UCVM.TempList = UCVM.IndkobListe.ToList();
            UCVM.IndkobListe.Clear();
            for (int i = 0; i < UCVM.TempList.Count; i++)
            {
                UCVM.IndkobListe.Add(UCVM.TempList[i]);
            }
            if (UCVM.IndkobListe.Count > 0)
            {
                Køb.IsEnabled = true;
                Cancel.IsEnabled = true;
            }
            else
            {
                Køb.IsEnabled = false;
                Cancel.IsEnabled = false;
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Content.ToString();
            bool found = false;
            for (int i = 0; i < UCVM.IndkobListe.Count; i++)
            {
                if (UCVM.IndkobListe[i].Name == name)
                {
                    UCVM.IndkobListe[i].Amount += 1;
                    UCVM.IndkobListe[i].Combine();
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                bool isDrink = false;
                foreach (var item in UCVM.vareItem)
                {
                    if (name == item.Navn)
                        isDrink = item.ErDrink;
                }
                UCVM.IndkobListe.Add(new OrderItem
                {
                    Name = name,
                    Amount = 1,
                    Id = Convert.ToInt32(((Button)sender).Name.ToString()),
                    ErDrink = isDrink
                });
                UCVM.IndkobListe[UCVM.IndkobListe.Count - 1].Combine();
            }
            UpdateList();
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            string name = "";
            try
            {
                name = ((HyperlinkButton)sender).Content.ToString();
            }
            catch { }

            try
            {
                name = ((TextBlock)sender).Text.ToString();
                name = name.Split(',')[0];
            }
            catch { }

            try
            {
                name = ((Button)sender).Content.ToString();
                name = name.Split(',')[0];
            }
            catch { }

            for (int i = 0; i < UCVM.IndkobListe.Count; i++)
            {
                if (UCVM.IndkobListe[i].Name == name)
                {
                    UCVM.IndkobListe[i].Amount -= 1;
                    UCVM.IndkobListe[i].Combine();
                    if (UCVM.IndkobListe[i].Amount == 0)
                        UCVM.IndkobListe.RemoveAt(i);
                }
            }
            UpdateList();
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            UCVM.BuyAsync();
            Find_bruger.SelectedIndex = -1;
            Products.IsEnabled = false;
            Køb.IsEnabled = false;
            Cancel.IsEnabled = false;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            UCVM.IndkobListe.Clear();
            Find_bruger.SelectedIndex = -1;
            Products.IsEnabled = false;
            Køb.IsEnabled = false;
            Cancel.IsEnabled = false;
        }

        private void Find_bruger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UCVM.IndkobListe.Clear();//Hvis brugeren bbliver skiftet undervejs nulstilles indkøbslisten.
            Køb.IsEnabled = false;
            Cancel.IsEnabled = false;
            if (Find_bruger == null) return;
            if (Find_bruger != null) { Products.IsEnabled = true; }
            if (App.first)
            {
                UCVM.LoadDrinkAsync();
                UCVM.LoadVarerAsync();
                App.first = false;
            }
            var combo = (ComboBox)sender;
            try
            {
                string[] buffer = combo.SelectedItem.ToString().Split(' ');
                Debug.WriteLine(buffer[0]);
                foreach (var item in UCVM.brugere)
                {
                    if (buffer[0] == item.VærelseNr.ToString())
                        UCVM.BrugerId = item.Id;
                }

            }
            catch (Exception f)
            {
                UCVM.BrugerId = -1;
            }
        }
    }
}
