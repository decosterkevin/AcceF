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

    public sealed partial class MyCustomContentDialogOrga : ContentDialog
    {
        public MyResult Result { get; set; }

        public MyCustomContentDialogOrga()
        {
            this.InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Result = MyResult.Ok;
            dialog.Hide();

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MyResult.Cancel;
            dialog.Hide();

        }

    }
}
