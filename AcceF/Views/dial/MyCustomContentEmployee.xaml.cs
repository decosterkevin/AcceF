using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AcceF
{

    public sealed partial class MyCustomContentDialogEmployee : ContentDialog
    {
        public List<Person> Result { get; set; }

        public MyCustomContentDialogEmployee()
        {
            this.InitializeComponent();
            this.Result = new List<Person>();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            dialog.Hide();

        }
        public void Initialize(List<Person> persons)
        {
            if(persons.Count() == 0)
            {
                txtAutoSuggestBox.Visibility = Visibility.Collapsed;
                listEmployee.Visibility = Visibility.Collapsed;
                NoEmployeeTextbox.Visibility = Visibility.Visible;
            }
            listEmployee.ItemsSource = persons;
        }
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)

        {

            List<Person> filteredList = DatabaseHelper.FilterPerson(sender.Text);
            listEmployee.ItemsSource = filteredList;

        }
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            Person item = (Person)e.ClickedItem;
            this.Result.Add(item);

        }
    }
}
