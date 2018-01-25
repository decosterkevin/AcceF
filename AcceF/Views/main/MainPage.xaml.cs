using AcceF.ModelViews;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Viewer.Navigate(typeof(DisplayListParty));
            listParties.ItemsSource = DatabaseHelper.GetToBeNotifiedParties();

        }
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            Party item = (Party)e.ClickedItem;
            this.Frame.Navigate(typeof(PartyViewer), item);

        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        private void PartyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Viewer.Navigate(typeof(DisplayListParty));
        }
        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Viewer.Navigate(typeof(Calendar));

        }
        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Viewer.Navigate(typeof(Employees));
        }
        private void ToolsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Viewer.Navigate(typeof(Tools));
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Viewer.Navigate(typeof(Settings));
        }
    }
}