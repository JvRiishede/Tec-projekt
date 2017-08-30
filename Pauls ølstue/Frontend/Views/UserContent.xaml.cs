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
        private List<Vare> vare;
        public List<int> location;
        public ObservableCollection<Test2> DataList2;
        public UserContentViewModel UCVM { get; set; }
        public int DataList2Location;
        public UserContent()
        {
            this.InitializeComponent();
            UCVM = new UserContentViewModel();
            UCVM.Load();

            vare = VareManager.Varer();
            DataList2 = new ObservableCollection<Test2>();
            location = new List<int>();
            
            DataList2Location = 0;
            location.Add(DataList2Location);
        }
        
        public class Test2
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            DataList2.Add(new Test2 { Name = ((Button)sender).Content.ToString(), Amount = DataList2Location });
            DataList2Location++;
            location.Add(DataList2Location);
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            string name = ((HyperlinkButton)sender).Name.ToString();
            //DataList2Location--;
            for (int i = 0; i < DataList2.Count; i++)
            {
                for (int j = 0; j < location.Count; j++)
                {
                    if (DataList2[i].Amount == location[j])
                        DataList2.RemoveAt(location[j]);
                }
            }
            Debug.WriteLine(name+" "+ DataList2Location);
        }
    }
}
