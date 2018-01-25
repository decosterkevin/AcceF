using AcceF.ModelViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace AcceF
{
    public class BrushColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Debug.WriteLine((bool)value);
            if ((bool)value)
            {
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }
            return new SolidColorBrush(Colors.Red);
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public sealed partial class MyCustomContentDialogParty : ContentDialog
    {
        public Party result = null;
        public MyCustomContentDialogParty()
        {
            this.InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            dialog.Hide();

        }
        private void Item_Click(object sender, ItemClickEventArgs e)
        {
            Party item = (Party)e.ClickedItem;
            result = item;
            dialog.Hide();

        }
    }
}
