using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Converter
{
    class ConverterSongRF : IValueConverter
    {       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value <- 111) return Color.FromHex("#ff9999");
            else if ((int)value >= -110 && (int)value <= -96) return Color.FromHex("#FFA500");
            else if ((int)value >= -95 && (int)value <= -81) return Color.FromHex("#008000");
            else return Color.FromHex("#0000ff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
