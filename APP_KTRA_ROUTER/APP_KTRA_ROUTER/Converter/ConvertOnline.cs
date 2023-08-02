using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Converter
{
    //chuyển đỗi trạng thái online thành chữ màu xanh và offline thành chữ màu đỏ
    public class ConvertOnline<T> : IValueConverter
    {
        public T NullObject { set; get; } //giá trị trả về nếu đúng 
        public T TrueObject { set; get; } //giá trị trả về nếu đúng

        public T FalseObject { set; get; } //trả về giá trị nếu k online
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null) return NullObject;
                if ((string )value != "0" && (string)value != "") return TrueObject;
                else return FalseObject;
            }
            catch (Exception)
            {
                return FalseObject;
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
