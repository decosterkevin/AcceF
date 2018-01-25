using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
using Windows.UI.Popups;
using AcceF.ModelViews;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEmployee : Page
    {
        private List<TextBox> boxs;
        Boolean isModifiedPage = false;
        Person employee;
        int personID = -1;
        public AddEmployee()
        {
            this.InitializeComponent();
            boxs = new List<TextBox> { employeeName, employeeSurName, employeeAdress, employeeCity, employeeZip, employeeEmail, employeePhone1 };
            employee = new Person();
            Grid_Employee.DataContext = employee;

        }
        private async void Add_click(object sender, RoutedEventArgs e)
        {
            
            using (var db = new PartyContext())
            {
                DateTimeOffset sourceTime = employeeBirthDate.Date ?? new DateTimeOffset();
                if (ValidateEntry())
                {
                    //var employee = new Person { Name = employeeName.Text, Surname = employeeSurName.Text, BirthDate = sourceTime.DateTime, Email = employeeEmail.Text, Adress = employeeAdress.Text, ZIP= employeeZip.Text, City =  employeeCity.Text, IBAN = employeeIBAN.Text, AVS = employeeAVS.Text, Phone_number1 = employeePhone1.Text, Phone_number2 = employeePhone2.Text };
                    if (!isModifiedPage)
                    {
                        db.employees.Add(employee);
                    }
                    else
                    {
                        MessageDialog showDialog = new MessageDialog("Voulez vous vraiment modifier ces informations?");
                        showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
                        showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
                        showDialog.DefaultCommandIndex = 0;
                        showDialog.CancelCommandIndex = 1;
                        var result = await showDialog.ShowAsync();
                        if ((int)result.Id == 0)
                        {
                            employee.PersonId = personID;
                            var employeeDb = db.employees.Find(employee.PersonId);
                            db.Entry(employeeDb).CurrentValues.SetValues(employee);
                        }
                    }
                    db.SaveChanges();
                    this.Frame.GoBack();
                }
                
            }
        }
        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
        private async void  Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Voulez vous vraiment supprimer cet employé?");
            showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                using (var db = new PartyContext())
                {
                    DatabaseHelper.DeleteEmployeeEntry(db.employees.Find(personID));
                }
                this.Frame.GoBack();

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

            if (employeeEmail.Text != null && employeeEmail.Text != String.Empty && !util.IsValidEmail(employeeEmail.Text))
            {
                employeeEmail.BorderBrush = new SolidColorBrush(Colors.Red);
                passed = false;
                msg += "Email invalide \n";
            }
            if (employeeBirthDate.Date == null)
            {
                employeeBirthDate.BorderBrush = new SolidColorBrush(Colors.Red);
                passed = false;
                msg += "Veuillez renseigner la date de naissance\n";
            }
            if (!passed)
            {
                Utilities.ShowDial(msg);
            }
            return passed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            employee = e.Parameter as Person;

            if (employee != null)
            {
                //We modify an employee as we pass a Person parameter
                isModifiedPage = true;
                Delete_button.Visibility = Visibility;
                personID = employee.PersonId;
                addEmployeeButton.Content = "Modifier employé";
                listSkills.ItemsSource = DatabaseHelper.GetSkillsByID(employee.ToolIDs);


                //var fieldnames = new[] { "Name", "Surname", "Phone_number1", "Phone_number2", "Adress", "City", "ZIP", "Email", "AVS" };
                //var tb = new List<TextBox> { employeeName, employeeSurName, employeePhone1, employeePhone2, employeeAdress, employeeCity, employeeZip, employeeEmail, employeeAVS };
                //for (int i = 0; i < fieldnames.Length; i++)
                //{
                //    Object tmp = employee.GetType().GetProperty(fieldnames[i]).GetValue(employee, null);
                //    if (tmp != null)
                //    {
                //        tb[i].Text =tmp.ToString();
                //    }


                //}
                //if (employee.BirthDate != null)
                //{
                //    try
                //    {
                //        employeeBirthDate.Date = employee.BirthDate;
                //    }
                //    catch (System.ArgumentOutOfRangeException)
                //    {

                //    }

                //}


            }
            else
            {
                employee = new Person();

            }
            Grid_Employee.DataContext = employee;


        }
        private async void Email_Employee(object sender, RoutedEventArgs e)
        {
            Person item = (sender as Button).Tag as Person;
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(@"mailto:" + item.Email));


        }

        private async void Add_SkillList(object sender, RoutedEventArgs e)
        {
            using (var db = new PartyContext())
            {
                List<Skill> skills = db.skills.ToList<Skill>();
                MyCustomContentDialogSkill dial = new MyCustomContentDialogSkill();
                dial.Initialize(skills);
                await dial.ShowAsync();
                HashSet<String> toBeAdded = new HashSet<String>(dial.Result.Select(u => u.ToolId.ToString()).ToList());
                HashSet<String> currentSkills = new HashSet<String>(this.employee.ToolIDs);

                if (currentSkills != null)
                {
                    toBeAdded.UnionWith(currentSkills);
                }
                this.employee.ToolIDs = toBeAdded.ToList();
                listSkills.ItemsSource = null;
                listSkills.ItemsSource = DatabaseHelper.GetSkillsByID(toBeAdded.ToList());


            }

        }
        private void Delete_skills(object sender, RoutedEventArgs e)
        {
            Skill item = (sender as Button).Tag as Skill;
            List<Skill> skillTmp = DatabaseHelper.GetSkillsByID(this.employee.ToolIDs);
            skillTmp.Remove(skillTmp.Find(u => u.ToolId == item.ToolId));
            this.employee.ToolIDs = skillTmp.Select(u => u.ToolId.ToString()).ToList();
            int indexRemove = listSkills.Items.IndexOf(item);
            listSkills.ItemsSource = null;

            listSkills.ItemsSource = DatabaseHelper.GetSkillsByID(this.employee.ToolIDs);


        }


    }
}