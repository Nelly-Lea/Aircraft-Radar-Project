using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.Holidays
{
    /// <summary>
    /// Interaction logic for HolidaysView.xaml
    /// </summary>
    public partial class HolidaysView : UserControl
    {
        public HolidaysViewModel holidaysViewModel;
        public HolidaysView()
        {
            InitializeComponent();
            holidaysViewModel = new HolidaysViewModel();
        }
    }
}
