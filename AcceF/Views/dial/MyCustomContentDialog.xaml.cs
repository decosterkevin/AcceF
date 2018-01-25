using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AcceF
{
    public enum MyResult
    {
        Ok,
        Cancel,
        Delete,
        Nothing
    }

    public sealed partial class MyCustomContentDialog : ContentDialog
    {
        public MyResult Result { get; set; }

        public MyCustomContentDialog()
        {
            this.InitializeComponent();
            this.Result = MyResult.Nothing;
        }

        // Handle the button clicks from dialog
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.Ok;
            // Close the dialog
            dialog.Hide();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.Cancel;
            // Close the dialog
            dialog.Hide();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.Delete;
            // Close the dialog
            dialog.Hide();
        }
    }
}
