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
        
        
        public UserContentViewModel UCVM { get; set; }
        public UserContent()
        {
            this.InitializeComponent();
            UCVM = new UserContentViewModel();
            UCVM.Load();
            
            UCVM.vare = VareManager.Varer();
            UCVM.IndkobListe.Add(new UserContentViewModel.VareForList { });
            UCVM.IndkobListe.Add(new UserContentViewModel.VareForList { });// skal have fundet en løsning på det her!!!!
        }

        private void UpdateList()
        {
            UCVM.TempList = UCVM.IndkobListe.ToList();
            UCVM.IndkobListe.Clear();
            for (int i = 0; i < UCVM.TempList.Count; i++)
            {
                UCVM.IndkobListe.Add(UCVM.TempList[i]);
            }
            UCVM.TempList.Clear();
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
                }
            }
            if (found == false)
                UCVM.IndkobListe.Add(new UserContentViewModel.VareForList
                {
                    Name = ((Button)sender).Content.ToString(), Amount = 1, Id = Convert.ToInt32(((Button)sender).Name.ToString())
                });
            UpdateList();
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            string name="";
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
            UCVM.Buy();
            UCVM.IndkobListe.Clear();
            UCVM.IndkobListe.Add(new UserContentViewModel.VareForList { });
            UCVM.IndkobListe.Add(new UserContentViewModel.VareForList { });// skal have fundet en løsning på det her!!!!
        }
    }
}
