using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AcceF
{


    public class MyCalendarDatePicker : CalendarDatePicker
    {
        public DateTime SelectedDate
        {
            get
            {
                return (DateTime)GetValue(SelectedDateProperty);
            }

            set { SetValue(SelectedDateProperty, value); }

        }
        public static readonly DependencyProperty SelectedDateProperty = 
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(MyCalendarDatePicker), new PropertyMetadata(null, (sender, e) =>
                {
                    Debug.WriteLine(e.NewValue);
                    if (e.NewValue != null && (DateTime)e.NewValue != new DateTime())
                    {
                        ((MyCalendarDatePicker)sender).Date =
                (DateTime)e.NewValue;

                    }
                    else
                    {
                        ((MyCalendarDatePicker)sender).Date = null;
                    }
                }));

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DateChanged += MyCalendarDatePicker_DateChanged;

        }
        private void MyCalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (args.NewDate != args.OldDate)
            {
                if (args.NewDate != null && args.NewDate.HasValue)
                { 
                    SelectedDate = args.NewDate.Value.Date;
                }
                else if (args.OldDate != null && args.OldDate.HasValue)
                {
                    SelectedDate = args.OldDate.Value.Date;
                    Date = args.OldDate;
                }
                else
                {
                    SelectedDate = DateTimeOffset.Now.Date;
                }
            }
        }
    }
}