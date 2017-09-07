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
using Frontend.Models;
using ViewModels;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Frontend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpContent : Page
    {
        public HelpViewModel HVM;
        public HelpContent()
        {
            HVM = new HelpViewModel();
            
            this.InitializeComponent();
        }

        private void HelpList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HVM.HelpList.Clear();
            HVM.LoadAsync();
        }

        private void SetHelp(object sender, RoutedEventArgs e)
        {
            foreach (var item in HVM.HelpList)
            {
                if(item.Navn== ((Button)sender).Content.ToString())
                {
                    HelpView.Blocks.Clear();
                    Run run = new Run();
                    run.Text = item.Text;
                    Paragraph paragraph = new Paragraph();
                    paragraph.Inlines.Add(run);
                    HelpView.Blocks.Add(paragraph);
                }
            }
            
        }
    }
}
