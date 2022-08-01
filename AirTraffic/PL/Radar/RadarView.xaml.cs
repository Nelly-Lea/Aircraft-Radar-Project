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
using PL.FlightData;

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
            //FlightDataView fv = new FlightDataView();
            //radarviewgrid.Children.Add(fv);
          

        }
        //changer cette fonction
        private void Pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pin = e.OriginalSource as Pushpin;
            if (pin != null)
                MessageBox.Show(pin.ToolTip.ToString());
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FlightDataView fv = new FlightDataView();
            radarviewgrid.Children.Add(fv);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myMap.Children.Clear();
            if (OutFlightsListBox.SelectedItem==null&& InFlightsListBox.SelectedItem==null)
            {
                MessageBox.Show("you didn't click on item");
                return;
            }
            BE.FlightInfoPartial FlightIP = new BE.FlightInfoPartial();
            bool inflight=true;
            if (InFlightsListBox.SelectedItem != null)
                inflight = true;
            if (OutFlightsListBox.SelectedItem != null)
                inflight = false;
            BE.Root Flight = new BE.Root();
            if (inflight == true)
            {
                FlightIP = InFlightsListBox.SelectedItem as BE.FlightInfoPartial;
                InFlightsListBox.SelectedItem = null;
                Flight = radarViewModel.ConvertFlightIPToFlighInfo(FlightIP);
                    }
            else
            {
                FlightIP = OutFlightsListBox.SelectedItem as BE.FlightInfoPartial;
                OutFlightsListBox.SelectedItem = null;
                Flight = radarViewModel.ConvertFlightIPToFlighInfo(FlightIP);
            }
                

            if (Flight != null)
            {
                var OrderedPlaces = (from f in Flight.trail
                                     orderby f.ts
                                     select f).ToList<BE.Trail>();

                addNewPolyLine(OrderedPlaces);

                //MessageBox.Show(Flight.airport.destination.code.iata);
                BE.Trail CurrentPlace = null;

                Pushpin PinCurrent = new Pushpin { ToolTip = FlightIP.Id };
                Pushpin PinOrigin = new Pushpin { ToolTip = Flight.airport.origin.name };
                Pushpin PinDestination = new Pushpin { ToolTip = Flight.airport.destination.name };


                PositionOrigin origin = new PositionOrigin { X = 0.4, Y = 0.4 };
                MapLayer.SetPositionOrigin(PinCurrent, origin);

               

                //Better to use RenderTransform
                if (Flight.airport.destination.code.iata == "TLV")
                {
                    PinCurrent.Style = (Style)Resources["ToIsrael"];
                }
                else
                {
                    PinCurrent.Style = (Style)Resources["FromIsrael"];
                }

                CurrentPlace = OrderedPlaces.Last<BE.Trail>();
                var PlaneLocation = new Location { Latitude = CurrentPlace.lat, Longitude = CurrentPlace.lng };
                PinCurrent.Location = PlaneLocation;


                CurrentPlace = OrderedPlaces.First<BE.Trail>();
                PlaneLocation = new Location { Latitude = CurrentPlace.lat, Longitude = CurrentPlace.lng };
                PinOrigin.Location = PlaneLocation;

                var AirportLocation = new Location { Latitude = Flight.airport.destination.position.latitude, Longitude = Flight.airport.destination.position.longitude };
                PinDestination.Location = AirportLocation;

                //PinCurrent.MouseDown += Pin_MouseDown;

                myMap.Children.Add(PinOrigin);
                myMap.Children.Add(PinCurrent);
                myMap.Children.Add(PinDestination);

            }

        }
        void addNewPolyLine(List<BE.Trail> Route)
        {
            MapPolyline polyline = new MapPolyline();
            //polyline.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            polyline.StrokeThickness = 1;
            polyline.Opacity = 0.7;
            polyline.Locations = new LocationCollection();
            foreach (var item in Route)
            {
                polyline.Locations.Add(new Location(item.lat, item.lng));
            }

            myMap.Children.Clear();
            myMap.Children.Add(polyline);
        }
    }
   
}

