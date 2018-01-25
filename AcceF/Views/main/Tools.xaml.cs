using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public sealed partial class Tools : Page
    {
        bool notTrigger = false;
        public Tools()
        {
            this.InitializeComponent();
            Load_lists();
            notTrigger = false;
                
        }
        private void Load_lists()
        {
            notTrigger = true;
            using (var db = new PartyContext())
            {
                if (db.products.Count() > 0)
                {
                    Products_list.ItemsSource = db.products;
                }
                else
                {
                    Products_list.IsEnabled = false;
                }
                if (db.structures.Count() > 0)
                {
                    Structures_list.ItemsSource = db.structures;
                    
                }
                else
                {
                    Structures_list.IsEnabled = false;
                }
                if (db.skills.Count() > 0)
                {
                    Skills_list.ItemsSource = db.skills;
                }
                else
                {
                    Skills_list.IsEnabled = false;
                }
                if (db.machines.Count() > 0)
                {
                    Machines_list.ItemsSource = db.machines;
                }
                else
                {
                    Machines_list.IsEnabled = false;
                }
            }
        }
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            Button tmp = (Button)sender;
            MyCustomContentDialog dialog = new MyCustomContentDialog();
            dialog.Title = "Ajouter";
            ((Button)dialog.FindName("Delete")).Visibility = Visibility.Collapsed;
            Grid myGrid = ((Grid)dialog.FindName("Grid_Dial"));
            if ((string)tmp.Tag == "0" || (string)tmp.Tag == "1")
            {
                myGrid.RowDefinitions[1].Height = new GridLength(0);
            }
            Tool toolToAdd = null;

            switch (tmp.Tag)
            {
                case "0":
                    toolToAdd = new Skill();
                    myGrid.DataContext = toolToAdd;
                    break;
                case "1":
                    toolToAdd = new Product();
                    myGrid.DataContext = toolToAdd;
                    break;
                case "2":
                    toolToAdd = new Structure();
                    myGrid.DataContext = toolToAdd;
                    break;
                case "3":
                    toolToAdd = new Machine();
                    myGrid.DataContext = toolToAdd;

                    break;
            }


            await dialog.ShowAsync();
            if (dialog.Result == MyResult.Ok)
            {
                //save 
                using (var db = new PartyContext())
                {
                    switch (tmp.Tag)
                    {
                        case "0":
                            //Add to skills
                            db.skills.Add((Skill)toolToAdd);
                            break;
                        case "1":
                            //Add to produtcst
                            db.products.Add((Product)toolToAdd);
                            break;
                        case "2":
                            //Add to structures
                            db.structures.Add((Structure)toolToAdd);
                            break;
                        case "3":
                            //Add to machine
                            db.machines.Add((Machine)toolToAdd);

                            break;
                    }
                    db.SaveChanges();


                }
                Load_lists();
            }



        }

        private async void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!notTrigger)
            { 
                ComboBox tmp = (ComboBox)sender;


                Tool toolToAdd = (Tool)tmp.SelectedItem;
                MyCustomContentDialog dialog = new MyCustomContentDialog();
                dialog.Title = "Modifier";

                Grid myGrid = ((Grid)dialog.FindName("Grid_Dial"));
                if ((string)tmp.Tag == "0" || (string)tmp.Tag == "1")
                {
                    myGrid.RowDefinitions[1].Height = new GridLength(0);
                }
                myGrid.DataContext = toolToAdd;
                await dialog.ShowAsync();
                if (dialog.Result == MyResult.Ok)
                {
                    //save 
                    using (var db = new PartyContext())
                    {
                        switch (tmp.Tag)
                        {
                            case "0":
                                //Add to skills
                                var skillDb = db.skills.Find(toolToAdd.ToolId);
                                db.Entry(skillDb).CurrentValues.SetValues(toolToAdd);
                                break;
                            case "1":
                                //Add to produtcst
                                var productDb = db.products.Find(toolToAdd.ToolId);
                                db.Entry(productDb).CurrentValues.SetValues(toolToAdd);
                                break;
                            case "2":
                                //Add to structures
                                var structureDb = db.structures.Find(toolToAdd.ToolId);
                                db.Entry(structureDb).CurrentValues.SetValues(toolToAdd);
                                break;
                            case "3":
                                //Add to machine
                                var machineDb = db.machines.Find(toolToAdd.ToolId);
                                db.Entry(machineDb).CurrentValues.SetValues(toolToAdd);

                                break;
                        }
                        db.SaveChanges();


                    }
                }
                else if (dialog.Result == MyResult.Delete)
                {
                    MessageDialog showDialog = new MessageDialog("Voulez vous vraiment supprimer cette entrée?");
                    showDialog.Commands.Add(new UICommand("Oui") { Id = 0 });
                    showDialog.Commands.Add(new UICommand("Non") { Id = 1 });
                    showDialog.DefaultCommandIndex = 0;
                    showDialog.CancelCommandIndex = 1;
                    var result = await showDialog.ShowAsync();
                    if ((int)result.Id == 0)
                    {
                        using (var db = new PartyContext())
                        {
                            switch (tmp.Tag)
                            {
                                case "0":
                                    //Add to skills
                                    DatabaseHelper.DeleteToolEntry(db.skills.Find(toolToAdd.ToolId));
                                    break;
                                case "1":
                                    //Add to produtcst
                                    DatabaseHelper.DeleteToolEntry(db.products.Find(toolToAdd.ToolId));
                                    break;
                                case "2":
                                    //Add to structures
                                    DatabaseHelper.DeleteToolEntry(db.structures.Find(toolToAdd.ToolId));
                                    break;
                                case "3":
                                    //Add to machine
                                    DatabaseHelper.DeleteToolEntry(db.machines.Find(toolToAdd.ToolId));

                                    break;
                            }
                            db.SaveChanges();

                        }
                    }
                }
                dialog.Hide();
                Load_lists();
            }
            notTrigger = false;


        }

        

    }
    

}