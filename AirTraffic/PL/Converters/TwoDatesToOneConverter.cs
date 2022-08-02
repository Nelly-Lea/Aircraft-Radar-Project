using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;

namespace PL.Converters
{
    public class TwoDatesToOneConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var Result = string.Empty;
            ObservableCollection<string> ObsDate = new ObservableCollection<string>();

             if (values.Length > 1)
            {
                //Result = values[0].ToString() + "," + values[1].ToString();
                ObsDate.Add(values[0].ToString());
                ObsDate.Add(values[1].ToString());
            }


            return ObsDate;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
