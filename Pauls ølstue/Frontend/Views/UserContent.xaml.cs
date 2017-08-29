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
        public List<Test2> DataList { get; set; }
        public UserContent()
        {
            this.InitializeComponent();
            UCVM = new UserContentViewModel();
            UCVM.Load();

            vare = VareManager.Varer();
            Debug.WriteLine(vare[0].VareNavn);

            DataList = new List<Test2>
            {
                new Test2
                {
                    Name = "Name",
                    Amount = 2
                },
                new Test2
                {
                    Name = "Name2",
                    Amount = 4
                }
            };
        }
        public UserContentViewModel UCVM { get; set; }
        public class Test2
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }

        private void Products_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            //DataList.Add(new Test2 { Name=e.ClickedItem.ToString()})
            Debug.WriteLine(" ");
        }
    }
}
