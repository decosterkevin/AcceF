using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class PartyViewer : Page
    {
        int LeftPanelIndex = 0;
        List<Party> partyArrayLeft = new List<Party> { };
        Party rightParty;

        public PartyViewer()
        {
            this.InitializeComponent();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();

        }
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Voulez vous vraiment sauvegarder tout les changements?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                using (var db = new PartyContext())
                {
                    var partyDb = db.parties.Find(rightParty.PartyId);
                    db.Entry(partyDb).CurrentValues.SetValues(rightParty);
                    foreach (Party tmp in partyArrayLeft)
                    {
                        var partyTmp = db.parties.Find(tmp.PartyId);
                        Debug.WriteLine(partyTmp.PartyId);
                        db.Entry(partyTmp).CurrentValues.SetValues(tmp);
                    }
                    db.SaveChanges();
                }
            }
        }

        private void Right_Arrow_Click(object sender, RoutedEventArgs e)
        {
            if(LeftPanelIndex == -1)
            {
                Enable_Grid(Left_Grid);
            }
            LeftPanelIndex += 1;
            Update_LeftPanel(partyArrayLeft[LeftPanelIndex]);
            Check_arrow();
        }
        private void Left_Arrow_Click(object sender, RoutedEventArgs e)
        {
            LeftPanelIndex -= 1;
            Update_LeftPanel(partyArrayLeft[LeftPanelIndex]);
            Check_arrow();
        }

        private void Advance_Click_l(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddParty), partyArrayLeft[LeftPanelIndex]);
        }

        private void Advance_Click_r(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddParty), rightParty);
        }
        private async void Delete_Click_l(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Voulez vous vraiment supprimer cette évenement?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                DatabaseHelper.DeletePartyEntry(partyArrayLeft[LeftPanelIndex]);
                partyArrayLeft.RemoveAt(LeftPanelIndex);
                LeftPanelIndex -= 1;

                if (LeftPanelIndex < 0)
                {
                    if (partyArrayLeft.Count > 0)
                    {
                        LeftPanelIndex = 0;
                        Update_LeftPanel(partyArrayLeft[LeftPanelIndex]);

                    }
                    else
                    {
                        Update_LeftPanel(new Party());
                    }
                }
                else
                {
                    Update_LeftPanel(partyArrayLeft[LeftPanelIndex]);
                }
                Check_arrow();
            }
        }
        private async void Delete_Click_r(object sender, RoutedEventArgs e)
        {

            MessageDialog showDialog = new MessageDialog("Voulez vous vraiment supprimer cette évenement?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                DatabaseHelper.DeletePartyEntry(rightParty);
                rightParty = null;
                Update_RightPanel(new Party());
                Disable_Grid(Right_Grid);
            }
            

            
        }

        private void Disable_Grid(Grid grid)
        {
            foreach (TextBox tmp in grid.Children.OfType<TextBox>().ToList())
            {
                tmp.IsEnabled = false;
            }
            foreach (Button tmp in grid.Children.OfType<Button>().ToList())
            {
                tmp.IsEnabled = false;
            }
            foreach (CalendarDatePicker tmp in grid.Children.OfType<CalendarDatePicker>().ToList())
            {
                tmp.IsEnabled = false;
            }
            foreach (CheckBox tmp in grid.Children.OfType<CheckBox>().ToList())
            {
                tmp.IsEnabled = false;
            }
        }

        private void Enable_Grid(Grid grid)
        {
            foreach (TextBox tmp in grid.Children.OfType<TextBox>().ToList())
            {
                tmp.IsEnabled = true;
            }
            foreach (Button tmp in grid.Children.OfType<Button>().ToList())
            {
                tmp.IsEnabled = true;
            }
            foreach (CalendarDatePicker tmp in grid.Children.OfType<CalendarDatePicker>().ToList())
            {
                tmp.IsEnabled = true;
            }
            foreach (CheckBox tmp in grid.Children.OfType<CheckBox>().ToList())
            {
                tmp.IsEnabled = true;
            }
        }

        private void Update_LeftPanel(Party party)
        {
            Left_Grid.DataContext = party;
        }
        private void Update_RightPanel(Party party)
        {
            Right_Grid.DataContext = party;
        }
        private void Check_arrow()
        {
            if (LeftPanelIndex - 1 < 0)
            {
                left_button.IsEnabled = false;
            }
            else
            {
                left_button.IsEnabled = true;
            }
            if(LeftPanelIndex + 1 >= partyArrayLeft.Count)
            {
                right_button.IsEnabled = false;
            }
            else
            {
                right_button.IsEnabled = true;

            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Party party = e.Parameter as Party;

            if (party != null)
            {
               Title_name.Text = party.Name;
                partyArrayLeft = DatabaseHelper.SearchPartiesByName(party);
                LeftPanelIndex = partyArrayLeft.FindIndex(x => x.PartyId == party.PartyId) - 1;
                rightParty = party;
                partyArrayLeft.Remove(partyArrayLeft[LeftPanelIndex + 1]);
                if (LeftPanelIndex < 0)
                {
                    Update_LeftPanel(new Party());
                    Update_RightPanel(rightParty);
                    Disable_Grid(Left_Grid);
                }
                else
                {
                    Update_LeftPanel(partyArrayLeft[LeftPanelIndex]);
                    Update_RightPanel(rightParty);
                }
                Check_arrow();


            }
        }


    }
}