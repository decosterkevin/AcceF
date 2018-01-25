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

    public sealed partial class MyCustomContentDialogSkill : ContentDialog
    {
        public List<Skill> Result { get; set; }

        public MyCustomContentDialogSkill()
        {
            this.InitializeComponent();
            this.Result = new List<Skill>();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            dialog.Hide();

        }
        public void Initialize(List<Skill> tools)
        {
            if(tools.Count() == 0)
            {
                listSkills.Visibility = Visibility.Collapsed;
                NoSkillsTextbox.Visibility = Visibility.Visible;
            }
            listSkills.ItemsSource = tools;
        }
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            Skill item = (Skill)e.ClickedItem;
            this.Result.Add(item);

        }
    }
}
