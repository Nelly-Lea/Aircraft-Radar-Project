using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BE;

namespace AirTraffic.Converters
{
    public class TickToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Result = string.Empty;
            var IsOk = int.TryParse(value.ToString(), out int Ticks);

            HelperClass Helper = new HelperClass();

            if (IsOk)
            {
                Result = Helper.GetDateTimeFromEpoch(Ticks).ToString("dd-MM-yyyy HH:mm");
            }

            return Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


        //object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

