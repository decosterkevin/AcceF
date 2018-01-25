using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AcceF
{

    public sealed partial class MyCustomStands : ContentDialog
    {
        public Stand result = null;
        public Party party = null;
        public int currentMax = 0;
        public Tool currentTool = null;
        private bool isModified = false;
        public bool displayEmployees = false;
        public MyCustomStands()
        {
            this.InitializeComponent();
            
        }

        public bool Initialize(Stand stand, Party party)
        {
            if(stand == null)
            {
                this.result = new Stand();
                isModified = false;

            }
            else
            {
                this.result = stand;
                isModified = true;
            }
            this.party = party;
            listEmployee.ItemsSource = DatabaseHelper.GetEmployeesByID(result.EmployeesIDs);
            Main_Grid.DataContext = this.result;
            if (this.result.ToolIDs.Count() > 0)
            {
                listTools.ItemsSource = DatabaseHelper.GetToolsByID(this.result.ToolIDs);
            }

            return party.Date.Equals(new DateTime()) || party.ToDate.Equals(new DateTime()) || !party.Accepted;
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new PartyContext())
            {
                if (isModified)
                {
                    var standDb = db.stands.Find(this.result.StandId);
                    db.Entry(standDb).CurrentValues.SetValues(this.result);

                }
                else
                {
                    Debug.WriteLine("Add stand");
                    db.stands.Add(this.result);
                    
                }
                db.SaveChanges();
               // this.result = db.stands.Find(this.result.StandId);

            }
            Debug.WriteLine("ss: " + this.result.StandId.ToString());
                
            dialog.Hide();

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.result = null;
            dialog.Hide();
        }
        private async void Email_Employee(object sender, RoutedEventArgs e)
        {
            Person item = (sender as Button).Tag as Person;
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(@"mailto:" + item.Email));


        }
        private void Delete_employee(object sender, RoutedEventArgs e)
        {
            Person item = (sender as Button).Tag as Person;
            List<Person> employeesTmp = DatabaseHelper.GetEmployeesByID(this.result.EmployeesIDs);
            employeesTmp.Remove(employeesTmp.Find(u => u.PersonId == item.PersonId));
            this.result.EmployeesIDs = employeesTmp.Select(u => u.PersonId.ToString()).ToList();
            int indexRemove = listEmployee.Items.IndexOf(item);
            listEmployee.ItemsSource = null;

            listEmployee.ItemsSource = DatabaseHelper.GetEmployeesByID(this.result.EmployeesIDs);


        }
        //private void Item_Employee_Click(object sender, ItemClickEventArgs e)
        //{
        //    Person item = (Person)e.ClickedItem;
        //    if (item != null)
        //    {
        //        this.Frame.Navigate(typeof(AddEmployee), item);
        //    }
        //    listEmployee.ItemsSource = null;
        //    listEmployee.ItemsSource = DatabaseHelper.GetEmployeesByID(this.party.EmployeesIDs);


        //}
        private  void Add_EmployeeList(object sender, RoutedEventArgs e)
        {
            displayEmployees = true;
            dialog.Hide();

        }
        public void BlitEmployees()
        {
            listEmployee.ItemsSource = null;
            listEmployee.ItemsSource = DatabaseHelper.GetEmployeesByID(this.result.EmployeesIDs);
            displayEmployees = false;
        }

        private void Delete_Tool(object sender, RoutedEventArgs e)
        {
            Tool item = (sender as Button).Tag as Tool;
            List<string> toolTmp = this.result.ToolIDs;
            toolTmp.Remove(item.ToolId.ToString());
            this.result.ToolIDs = toolTmp;
            listTools.ItemsSource = null;
            if (this.result.ToolIDs.Count() > 0)
            {
                listTools.ItemsSource = DatabaseHelper.GetToolsByID(this.result.ToolIDs);
            }
            
            
        }
        private void Tool_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ((ComboBox)sender).SelectedIndex;
            if (index !=0)
            {

           
            using (var db = new PartyContext()) {
                
                
                
                
                if (index == 1)
                {
                    listTools2.ItemsSource = null;
                    listTools2.ItemsSource = db.products.ToList();
                }
                else if (index == 2)
                {
                    listTools2.ItemsSource = null;
                        listTools2.ItemsSource = db.machines.ToList();
                }
                else
                {
                    listTools2.ItemsSource = null;
                    listTools2.ItemsSource = db.structures.ToList();
                }
            }
            }

        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            int number = Convert.ToInt32(Number_quantity.Text);
            if (number > 0)
            {
                Number_quantity.Text = (number - 1).ToString();
            }
            
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            int number = Convert.ToInt32(Number_quantity.Text);
            if (number < currentMax)
            {
                Number_quantity.Text = (number + 1).ToString();
            }
        }
        private void Add_Tool_Click(object sender, RoutedEventArgs e)
        {
            int number = Convert.ToInt32(Number_quantity.Text);
            if (number > 0)
            {
                using (var db = new PartyContext())
                {
                   
                    List<String> currentTools = new List<String>(this.result.ToolIDs);

                    if (currentTools != null)
                    {
                        currentTools.AddRange(Enumerable.Repeat(this.currentTool.ToolId.ToString(), number));
                    }
                    this.result.ToolIDs = currentTools;
                    Debug.WriteLine(currentTools.Count());
                    listTools.ItemsSource = null;
                   listTools.ItemsSource = DatabaseHelper.GetToolsByID(this.result.ToolIDs);
                 



                }
            }
            Minus.IsEnabled = false;
            Plus.IsEnabled = false;
            Add_Tool.IsEnabled = false;
            Number_quantity.Text = "0";
        }
        private void Tool_Clicked(object sender, ItemClickEventArgs e)
        {
            currentTool= (Tool)e.ClickedItem;
            Minus.IsEnabled = true;
            Plus.IsEnabled = true;
            Add_Tool.IsEnabled = true;
            currentMax = currentTool.Number - DatabaseHelper.CountTool(currentTool, this.party, this.result);
                //

        }


    }

}