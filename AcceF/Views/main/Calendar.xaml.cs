using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
   
public sealed partial class Calendar : Page
    {
        
        public Calendar()
        {
            this.InitializeComponent();
        }
       private async void myCal_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTime selectedDate = args.AddedDates[0].Date;
            
            List<Party> parties = DatabaseHelper.FindPartyByDate(selectedDate);
            MyCustomContentDialogParty dial = new MyCustomContentDialogParty();
            ListView myGrid = ((ListView)dial.FindName("listParty"));
            myGrid.ItemsSource = parties;
            await dial.ShowAsync();
            
            if (dial.result != null)
            {
                this.Frame.Navigate(typeof(PartyViewer), dial.result);
            }
            


        }
        private void OnCalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            // Render basic day items.


            if (args.Phase == 0)
            
            {
                if (DatabaseHelper.FindPartyByDate(args.Item.Date).Count() == 0)
                {
                    args.Item.IsBlackout = true;
                    args.Item.IsEnabled = false;
                }
                else
                {


                    // Register callback for next phase.
                    List<Party> parties = DatabaseHelper.FindPartyByDate(args.Item.Date);
                    List<Color> colors = new List<Color>();
                    foreach (Party party in parties)
                    {
                        if (party.Accepted)
                        {
                            colors.Add(Colors.Green);
                        }
                        else
                        {
                            colors.Add(Colors.Red);
                        }
                    }
                    //args.Item.DataContext = DatabaseHelper.FindPartyByDate(args.Item.Date);
                    args.Item.SetDensityColors(colors);
                }

            }
        }


    }

   

}