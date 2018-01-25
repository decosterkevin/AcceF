using AcceF.ModelViews;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class Employees : Page
    {
        int indexFilter = -1;
        public Employees()
        {
            this.InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new PartyContext())
            {
                MyEmployees.ItemsSource = db.employees.ToList();
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddEmployee));
        }
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.indexFilter = Filter.SelectedIndex;
        }
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)

        {
            List<Person> filteredList;
            string text = sender.Text.ToLower();

            using (var db = new PartyContext())
            {
                switch(indexFilter)
                {
                    case 0:
                        filteredList= db.employees.Where(t => t.Name.ToLower().Contains(text) ||  t.Age.ToString().Equals(text) || t.Surname.ToLower().Contains(text) || t.City.ToLower().Contains(text)).ToList();
                        break;
                    case 1:
                        filteredList = db.employees.Where(t => t.Name.ToLower().Contains(text) || t.Surname.ToLower().Contains(text)).ToList();

                        break;
                    case 2:
                        filteredList = db.employees.Where(t => t.Age.ToString().Equals(text)).ToList();

                        break;
                    case 3:
                        filteredList = db.employees.Where(t => t.Adress.ToLower().Contains(text) ||t.City.ToLower() .Contains(text)).ToList();

                        break;
                    default:
                        filteredList = new List<Person>();
                        break;
                }
                
            }
            
            MyEmployees.ItemsSource = filteredList;

        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            MyButton mb = (sender as MyButton);
            mb.State = (mb.State + 1) % 3;
            using (var db = new PartyContext())
            {

                if (mb.State == 0)
                {
                    MyEmployees.ItemsSource = db.employees.ToList();
                }
                else
                {
                    switch (mb.Tag)
                    {
                        case "ID":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.PersonId); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.PersonId); }
                            break;
                        case "Name":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.Name); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.Name); }
                            break;
                        case "Surname":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.Surname); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.Surname); }
                            break;
                        case "Age":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.Age); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.Age); }
                            break;
                        case "Phone":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.Phone_number1); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.Phone_number1); }
                            break;
                        case "Email":
                            if (mb.State == 1) { MyEmployees.ItemsSource = db.employees.ToList().OrderBy(x => x.Email); }
                            else if (mb.State == 2) { MyEmployees.ItemsSource = db.employees.ToList().OrderByDescending(x => x.Email); }
                            break;
                    }
                }
                ResetArrow((string)mb.Tag);




            }


        }

        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            var item = (Person)e.ClickedItem;
            if(item != null)
            {
                this.Frame.Navigate(typeof(AddEmployee), item);
            }
            
        }
        private void ResetArrow(String tag)
        {
            List<MyButton> buttons = new List<MyButton> { ID_Button, Name_Button, Surname_Button, Age_Button, Phone_Button, Email_Button };
            List<TextBlock> testArrow = new List<TextBlock> { ID_arrow, Name_arrow, Surname_arrow, Age_arrow, Phone_arrow, Email_arrow};
            for (int i = 0; i < testArrow.Count(); i++)
            {
                MyButton tmp = buttons[i];
                TextBlock arrow = testArrow[i];
                if ((string)tmp.Tag == tag)
                {
                    if (tmp.State == 0)
                    {
                        arrow.Text = "";
                    }
                    else if (tmp.State == 1)
                    {
                        arrow.Text = "5";
                    }
                    else
                    {
                        arrow.Text = "6";
                    }
                }
                else
                {
                    tmp.State = 0;
                    arrow.Text = "";

                }
            }

        }

    }

}