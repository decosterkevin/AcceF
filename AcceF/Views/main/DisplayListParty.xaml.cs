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
using System.Diagnostics;
using AcceF.ModelViews;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AcceF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    class MyButton : Button
    {

        //0 for neutral, 1 for s
        private int m_State = 0;

        public int State
        {
            get { return m_State; }
            set { m_State = value; }
        }

    }
    public sealed partial class DisplayListParty: Page
    {
        int indexFilter = -1;

        public DisplayListParty()
        {
            this.InitializeComponent();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddParty));
        }
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.indexFilter = Filter.SelectedIndex;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)

        {

            List<Party> filteredList = DatabaseHelper.FilterParty(this.indexFilter,sender.Text);
            Parties.ItemsSource = filteredList;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new PartyContext())
            {
                Parties.ItemsSource = db.parties.ToList();
            }
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            MyButton mb = (sender as MyButton);
            mb.State = (mb.State + 1 )% 3;
            Debug.WriteLine(mb.State);
            using (var db = new PartyContext())
            {

                if (mb.State == 0)
                {
                    Parties.ItemsSource = db.parties.ToList();
                }
                else
                {
                    switch (mb.Tag)
                    {
                        case "ID":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.PartyId); }
                            else if(mb.State==2){ Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.PartyId); }
                            break;
                        case "From":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.Date); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.Date); }
                            break;
                        case "To":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.ToDate); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.ToDate); }
                            break;
                        case "LastWrote":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.Lastwrote); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.Lastwrote); }
                            break;
                        case "FirstWrote":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.Firstwrote); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.Firstwrote); }
                            break;
                        case "Name":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.Name); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.Name); }
                            break;
                        case "City":
                            if (mb.State == 1) { Parties.ItemsSource = db.parties.ToList().OrderBy(x => x.City); }
                            else if (mb.State == 2) { Parties.ItemsSource = db.parties.ToList().OrderByDescending(x => x.City); }
                            break;

                    }
                }
                ResetArrow((string)mb.Tag);




            }
           

        }
        private void ResetArrow(String tag)
        {
            List<MyButton> buttons = new List<MyButton> { ID_Button, Name_Button, From_Button, To_Button, LastWrote_Button, FirstWrote_Button, City_Button};
            List<TextBlock> testArrow = new List<TextBlock> { ID_arrow, Name_arrow, From_arrow, To_arrow, LastWrote_arrow, FirstWrote_arrow, City_arrow};
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
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            var item = (Party)e.ClickedItem;
            if (item != null)
            {
                this.Frame.Navigate(typeof(PartyViewer), item);
            }

        }
    }

}