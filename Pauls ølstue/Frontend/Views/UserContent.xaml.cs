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
using System.Diagnostics;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frontend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserContent : Page
    {
        private List<Data.Vare> vare;
        public ObservableCollection<Vare> IndkobListe;
        public UserContentViewModel UCVM { get; set; }
        public UserContent()
        {
            this.InitializeComponent();
            UCVM = new UserContentViewModel();
            UCVM.Load();

            vare = VareManager.Varer();
            IndkobListe = new ObservableCollection<Vare>();
        }
        
        public class Vare
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Content.ToString();
            bool found = false;
            for (int i = 0; i < IndkobListe.Count; i++)
            {
                if (IndkobListe[i].Name == name)
                {
                    IndkobListe[i].Amount += 1;
                    found = true;
                }
            }
            if(found==false)
                IndkobListe.Add(new Vare { Name = ((Button)sender).Content.ToString(), Amount = 1 });
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            string name = ((HyperlinkButton)sender).Content.ToString();
            
            for (int i = 0; i < IndkobListe.Count; i++)
            {
                if (IndkobListe[i].Name == name)
                {
                    IndkobListe[i].Amount -= 1;
                    if (IndkobListe[i].Amount == 0)
                        IndkobListe.RemoveAt(i);
                }
            }
        }
        private void Buy(object sender, RoutedEventArgs e)
        {
            UCVM.Buy();
            IndkobListe.Clear();
        }
    }
}
