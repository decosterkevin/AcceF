using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Popups;
using Windows.Storage;
using AcceF.ModelViews;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddParty : Page
    {
        List<TextBox> boxs;
        Party party;
        List<StorageFile> FilesToBeAdded;
        List<StorageFile> FilesAlreadyIn;
        List<int> months = new List<int> { 1, 3, 6 };
        bool isModified = false;
        public AddParty()
        {
            this.InitializeComponent();
            boxs = new List<TextBox> { partyName, partyPhone, partyEmail, partyAdress, partyCity, partyZip };
            party = new Party();
            FilesToBeAdded = new List<StorageFile>();
            FilesAlreadyIn = new List<StorageFile>();
            Party_Grid.DataContext = party;


        }

        private async void Accepted_Click(object sender, RoutedEventArgs e)
        {
            bool state = ((CheckBox)sender).IsChecked == true;
            if (state)
            {
                //it was false and is now true=> do nothing
            }
            else
            {
                MessageDialog showDialog = new MessageDialog("Ne plus accepter l'event supprimera tout les stands déjà présent, voulez vous continuer?");
                showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
                showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
                showDialog.DefaultCommandIndex = 0;
                showDialog.CancelCommandIndex = 1;
                var result = await showDialog.ShowAsync();
                if ((int)result.Id == 0)
                {
                    DatabaseHelper.RemoveStands(this.party);
                    listStands.ItemsSource = null;
                }
                else
                {
                    ((CheckBox)sender).IsChecked = true;
                }

            }
            Debug.WriteLine(state);

        }
        private async void Add_click(object sender, RoutedEventArgs e)
        {
            if (!isModified)
            {
                using (var db = new PartyContext())
                {
                    db.parties.Add(this.party);
                    db.SaveChanges();
                }
            }

            await Save_File_LocalAsync();
            using (var db = new PartyContext())
            {

                if (ValidateEntry())
                {


                    if (isModified)
                    {
                        MessageDialog showDialog = new MessageDialog("Voulez vous vraiment modifier ces informations?");
                        showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
                        showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
                        showDialog.DefaultCommandIndex = 0;
                        showDialog.CancelCommandIndex = 1;
                        var result = await showDialog.ShowAsync();
                        if ((int)result.Id == 0)
                        {
                            Debug.WriteLine(this.party.PartyId);
                            var partyDb = db.parties.Find(this.party.PartyId);
                            db.Entry(partyDb).CurrentValues.SetValues(this.party);
                        }
                    }
                    else
                    {
                        var partyDb = db.parties.Find(this.party.PartyId);
                        db.Entry(partyDb).CurrentValues.SetValues(this.party);
                    }

                    db.SaveChanges();
                    this.Frame.GoBack();
                }

            }
        }
        private Boolean ValidateEntry()
        {
            RegexUtilities util = new RegexUtilities();
            Boolean passed = true;
            string msg = "";

            Boolean anyEmptyBox = false;
            foreach (TextBox entry in boxs)
            {

                if (entry.Text == null || entry.Text == String.Empty)
                {
                    entry.BorderBrush = new SolidColorBrush(Colors.Red);
                    passed = false;
                    anyEmptyBox = true;

                }
            }
            if (anyEmptyBox) msg += "Veuillez renseigner les champs obligatoires vides \n";

            if (partyEmail.Text != null && partyEmail.Text != String.Empty && !util.IsValidEmail(partyEmail.Text))
            {
                partyEmail.BorderBrush = new SolidColorBrush(Colors.Red);
                passed = false;
                msg += "Email invalide \n";
            }
            if (partyFromDate.Date == null || partyToDate.Date == null)
            {
                partyFromDate.BorderBrush = new SolidColorBrush(Colors.Red);
                partyToDate.BorderBrush = new SolidColorBrush(Colors.Red);
                passed = false;
                msg += "Veuillez renseigner les dates de la fête \n";
            }
            if (partyFirstWrote.Date == null)
            {
                partyFirstWrote.BorderBrush = new SolidColorBrush(Colors.Red);
                passed = false;
                msg += "Veuillez renseigner la date de premier contact \n";
            }

            if (!passed)
            {
                Utilities.ShowDial(msg);
            }
            return passed;
        }



        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper.DeleteStands(this.party, this.party.StandsIDs);
            this.Frame.GoBack();
        }
        private void Calendar_Datepicker(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs e)
        {

            partyToDate.MinDate = partyFromDate.Date ?? new DateTimeOffset();

        }
        private void Calendar_Datepicker2(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs e)
        {

            partyLastWrote.Date = partyFirstWrote.Date ?? new DateTimeOffset();
            partyLastWrote.MinDate = partyFirstWrote.Date ?? new DateTimeOffset();


        }

        private async void Add_StandList(object sender, RoutedEventArgs e)
        {

            MyCustomStands dial = new MyCustomStands();
            if (dial.Initialize(null, this.party))
            {
                Utilities.ShowDial("Veuillez spécifier les dates et accepter l'event avant de procéder au stand");
            }
            else
            {
                await dial.ShowAsync();
                if (dial.result != null)
                {
                    while (dial.displayEmployees)
                    {
                        using (var db = new PartyContext())
                        {
                            List<Person> employees = db.employees.ToList<Person>();
                            MyCustomContentDialogEmployee dialEmployee = new MyCustomContentDialogEmployee();
                            dialEmployee.Initialize(employees);
                            await dialEmployee.ShowAsync();
                            HashSet<String> toBeAdded = new HashSet<String>(dialEmployee.Result.Select(u => u.PersonId.ToString()).ToList());
                            HashSet<String> currentEmployees = new HashSet<String>(dial.result.EmployeesIDs);

                            if (currentEmployees != null)
                            {
                                toBeAdded.UnionWith(currentEmployees);
                            }
                            dial.result.EmployeesIDs = toBeAdded.ToList();
                            dial.BlitEmployees();
                            await dial.ShowAsync();

                        }

                    }
                    HashSet<String> currentStands = new HashSet<String>(this.party.StandsIDs);
                    currentStands.Add(dial.result.StandId.ToString());
                    this.party.StandsIDs = currentStands.ToList();

                    listStands.ItemsSource = null;
                    Debug.WriteLine("sss " + dial.result.StandId.ToString());
                    listStands.ItemsSource = DatabaseHelper.GetStandsByID(currentStands.ToList());
                }
            }



        }
        private async void Add_OrgaList(object sender, RoutedEventArgs e)
        {

            Organisateur orga = new Organisateur();
            MyCustomContentDialogOrga dial = new MyCustomContentDialogOrga();
            Grid myGrid = ((Grid)dial.FindName("Grid_Orga"));
            myGrid.DataContext = orga;
            await dial.ShowAsync();

            if (dial.Result == MyResult.Ok)
            {
                using (var db = new PartyContext())
                {
                    db.organisateurs.Add(orga);
                    db.SaveChanges();
                }

                HashSet<String> currentOrga = new HashSet<String>(this.party.OrgaIDs);
                currentOrga.Add(orga.OrganisateurId.ToString());

                this.party.OrgaIDs = currentOrga.ToList();

                listOrga.ItemsSource = null;
                listOrga.ItemsSource = DatabaseHelper.GetOrgasByID(currentOrga.ToList());


            }





        }
        private async void Add_FileList(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.FilesToBeAdded.Add(file);
                listFiles.ItemsSource = null;
                listFiles.ItemsSource = this.FilesToBeAdded.Concat(this.FilesAlreadyIn);
            }


        }

        private async void Item_Orga_Click(object sender, ItemClickEventArgs e)
        {
            Organisateur item = (Organisateur)e.ClickedItem;
            MyCustomContentDialogOrga dial = new MyCustomContentDialogOrga();
            Grid myGrid = ((Grid)dial.FindName("Grid_Orga"));
            myGrid.DataContext = item;
            await dial.ShowAsync();
            listOrga.ItemsSource = null;
            listOrga.ItemsSource = DatabaseHelper.GetOrgasByID(this.party.OrgaIDs);

        }

        private async void Item_Stand_Click(object sender, ItemClickEventArgs e)
        {
            Stand item = (Stand)e.ClickedItem;
            if (item != null)
            {
                MyCustomStands dial = new MyCustomStands();
                if (dial.Initialize(item, this.party))
                {
                    Utilities.ShowDial("Veuillez spécifier les dates avant de procéder au stand");
                }
                else
                {
                    await dial.ShowAsync();
                    while (dial.displayEmployees)
                    {
                        using (var db = new PartyContext())
                        {
                            List<Person> employees = db.employees.ToList<Person>();
                            MyCustomContentDialogEmployee dialEmployee = new MyCustomContentDialogEmployee();
                            dialEmployee.Initialize(employees);
                            await dialEmployee.ShowAsync();
                            HashSet<String> toBeAdded = new HashSet<String>(dialEmployee.Result.Select(u => u.PersonId.ToString()).ToList());
                            HashSet<String> currentEmployees = new HashSet<String>(dial.result.EmployeesIDs);

                            if (currentEmployees != null)
                            {
                                toBeAdded.UnionWith(currentEmployees);
                            }
                            dial.result.EmployeesIDs = toBeAdded.ToList();
                            dial.BlitEmployees();
                            await dial.ShowAsync();

                        }

                    }
                    listStands.ItemsSource = null;
                    listStands.ItemsSource = DatabaseHelper.GetStandsByID(this.party.StandsIDs);
                }

            }



        }

        private async System.Threading.Tasks.Task Save_File_LocalAsync()
        {
            List<File> newList = new List<File>();
            StorageFolder rootFolder = ApplicationData.Current.LocalFolder;
            StorageFolder filesFolder = await rootFolder.CreateFolderAsync("Files", CreationCollisionOption.OpenIfExists);


            var projectFolderName = this.party.Name + "_" + this.party.PartyId;
            StorageFolder projectFolder = projectFolder = await filesFolder.CreateFolderAsync(projectFolderName, CreationCollisionOption.OpenIfExists);
            using (var db = new PartyContext())
            {
                foreach (StorageFile file in this.FilesToBeAdded)
                {
                    if (file != null)
                    {
                        StorageFile newFile = await file.CopyAsync(projectFolder);
                        File myFile = new File { Name = newFile.Name, Path = newFile.Path };
                        newList.Add(myFile);
                        db.files.Add(myFile);

                    }
                }
                List<File> tmp = DatabaseHelper.GetFilesByID(this.party.FilesIDs);
                if (tmp != null)
                {
                    newList = newList.Concat(tmp).ToList();
                }
                db.SaveChanges();
            }

            this.party.FilesIDs = newList.Select(u => u.FileId.ToString()).ToList();

        }

        private void Delete_stand(object sender, RoutedEventArgs e)
        {
            Stand item = (sender as Button).Tag as Stand;
            List<Stand> standsTmp = DatabaseHelper.GetStandsByID(this.party.StandsIDs);
            standsTmp.Remove(standsTmp.Find(u => u.StandId == item.StandId));
            this.party.StandsIDs = standsTmp.Select(u => u.StandId.ToString()).ToList();
            int indexRemove = listStands.Items.IndexOf(item);
            listStands.ItemsSource = null;

            listStands.ItemsSource = DatabaseHelper.GetStandsByID(this.party.StandsIDs);


        }
        private void Delete_Orga(object sender, RoutedEventArgs e)
        {
            Organisateur item = (sender as Button).Tag as Organisateur;
            List<Organisateur> OrgaTmp = DatabaseHelper.GetOrgasByID(this.party.OrgaIDs);
            OrgaTmp.Remove(OrgaTmp.Find(u => u.OrganisateurId == item.OrganisateurId));
            this.party.OrgaIDs = OrgaTmp.Select(u => u.OrganisateurId.ToString()).ToList();

            int indexRemove = listOrga.Items.IndexOf(item);
            listOrga.ItemsSource = null;

            listOrga.ItemsSource = DatabaseHelper.GetOrgasByID(this.party.OrgaIDs);


        }
        private async void Email_Orga(object sender, RoutedEventArgs e)
        {
            Organisateur item = (sender as Button).Tag as Organisateur;
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(@"mailto:" + item.Email));


        }

        private void Delete_File(object sender, RoutedEventArgs e)
        {

            File item = (sender as Button).Tag as File;
            List<File> fileTmp = DatabaseHelper.GetFilesByID(this.party.FilesIDs);
            fileTmp.Remove(fileTmp.Find(u => u.FileId == item.FileId));

            this.party.FilesIDs = fileTmp.Select(u => u.FileId.ToString()).ToList();

            int indexRemove = listFiles.Items.IndexOf(item);
            listFiles.ItemsSource = null;

            listFiles.ItemsSource = DatabaseHelper.GetFilesByID(this.party.FilesIDs);


        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Party tmp = e.Parameter as Party;
            if (tmp != null)
            {
                isModified = true;
                Party_Grid.DataContext = tmp;
                listStands.ItemsSource = null;
                listStands.ItemsSource = DatabaseHelper.GetStandsByID(tmp.StandsIDs);
                listFiles.ItemsSource = null;
                listFiles.ItemsSource = DatabaseHelper.GetFilesByID(tmp.FilesIDs);
                listOrga.ItemsSource = null;
                listOrga.ItemsSource = DatabaseHelper.GetOrgasByID(tmp.OrgaIDs);
                Ok_button.Content = "Modifier";
                this.party = tmp;
                foreach (File file in DatabaseHelper.GetFilesByID(this.party.FilesIDs))
                {
                    try
                    {
                        StorageFile fileTmp = await StorageFile.GetFileFromPathAsync(file.Path);
                        FilesAlreadyIn.Add(fileTmp);
                    }
                    catch (Exception exc)
                    {

                    }

                }


            }
        }
        private void LastWrote_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = lastWrite.IsChecked == true ? true : false;
            if (isChecked)
            {
                this.party.Lastwrote = DateTime.Today;
            }


        }
    }
}