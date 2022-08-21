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
using System.Windows.Shapes;

namespace PL.GadgetWindow
{
    /// <summary>
    /// Interaction logic for GadgetWindow.xaml
    /// </summary>
    public partial class GadgetWindowView : Window
    {
        //public ObservableCollection<BE.Root> listRoot = null;
        GadgetWindowViewModel gadgetViewModel = new GadgetWindowViewModel();
        BE.Root flight = new BE.Root();
        public GadgetWindowView()
        {
            InitializeComponent();
            //listRoot=new ObservableCollection<BE.Root>( gadgetViewModel.GVMGetAllFlightsRoot());
            //cbListFlights.ItemsSource = listRoot;
            //cbListFlights.DisplayMemberPath = "identification.number.@default";
            //cbListFlights.DataContext = listRoot;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void cbListFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    flight = cbListFlights.SelectedItem as BE.Root;
        //}
        
        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            // your event handler here
            e.Handled = true;
            flight = gadgetViewModel.GVMgetRootFromFlightCode(tbFlight.Text);
            if (flight != null)
                MessageBox.Show("good");
            else
                MessageBox.Show("bad");
        }
    }
}
