using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Maps.MapControl.WPF;
using System.Windows.Interactivity;

namespace AirTraffic.Radar
{
    /// <summary>
    /// Interaction logic for RadarView.xaml
    /// </summary>
    public partial class RadarView : UserControl
    {
      
        public RadarViewModel radarViewModel;
        public RadarView()
        {
            radarViewModel = new RadarViewModel();
            InitializeComponent();
            
            this.DataContext = radarViewModel; 
          

        }
        //changer cette fonction
        private void Pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pin = e.OriginalSource as Pushpin;
            if (pin != null)
                MessageBox.Show(pin.ToolTip.ToString());
        }

        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
