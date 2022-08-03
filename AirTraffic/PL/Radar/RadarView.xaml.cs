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
using System.Web.Script.Serialization;

namespace AirTraffic.Radar
{
    /// <summary>
    /// Interaction logic for RadarView.xaml
    /// </summary>
    public partial class RadarView : UserControl
    {
      
        public RadarViewModel radarViewModel;
        private const string AllURL = @" https://data-cloud.flightradar24.com/zones/fcgi/feed.js?faa=1&bounds=38.805%2C24.785%2C29.014%2C40.505&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string FlightURL = @"https://data-live.flightradar24.com/clickhandler/?version=1.5&flight=";

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


        private void Button_Click_1(object sender, RoutedEventArgs e) // pr rajouter l'onglet
        {
            FlightDataView fv = new FlightDataView();
            radarviewgrid.Children.Add(fv);


        }

        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BE.FlightInfoPartial SelectedFlight = null;
            SelectedFlight = e.AddedItems[0] as BE.FlightInfoPartial; //InFlightsListBox.SelectedItem as FlightInfoPartial;
            UpdateFlight(SelectedFlight);
            
            FlightDataView fv = new FlightDataView();
            BE.Root Flight = new BE.Root();
            Flight = radarViewModel.ConvertFlightIPToFlighInfo(SelectedFlight);
            fv.DetailsPanel.DataContext = Flight;
            myGrid.Children.Add(fv);
            Grid.SetColumn(fv, 1);
            Grid.SetRow(fv, 1);
            


        }

        private void UpdateFlight(BE.FlightInfoPartial selected)
        {


            // DetailsPanel.DataContext = Flight;
             BE.Root Flight = new BE.Root();
             Flight = radarViewModel.ConvertFlightIPToFlighInfo(selected);

            // Update map
            if (Flight != null)
            {

                var OrderedPlaces = radarViewModel.RVMGetTrail(Flight);

                addNewPolyLine(OrderedPlaces);

                //MessageBox.Show(Flight.airport.destination.code.iata);
                BE.Trail CurrentPlace = null;

                Pushpin PinCurrent = new Pushpin { ToolTip = selected.SourceId };
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

                CurrentPlace = radarViewModel.RVMGetCurrentPosition(OrderedPlaces);
                var PlaneLocation = radarViewModel.RVMGetPosition(CurrentPlace);
                PinCurrent.Location = PlaneLocation;


                var OriginPlace =radarViewModel.RVMGetOriginAirport(OrderedPlaces);
                PlaneLocation = radarViewModel.RVMGetPosition(OriginPlace);
                PinOrigin.Location = PlaneLocation;


                var AirportDst = radarViewModel.RVMGetPosition(Flight);              
                PinDestination.Location = AirportDst;
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

        //private void PushpinClick(object sender, RoutedEventArgs e)
        //{
        //    Pushpin p = sender as Pushpin;
        //   // string flight = p.Tag.ToString();
        //    //BE.Root Flight = new BE.Root();
        //    //Flight = GetFlight(flight);

        //    FlightDataView fv = new FlightDataView();
        //    radarviewgrid.Children.Add(fv);


        //}

        public BE.Root GetFlight(string Key)
        {
            var CurrentUrl = FlightURL + Key;
            BE.Root CurrentFlight = null;
            //must use try-catch
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(CurrentUrl);
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    CurrentFlight = serializer.Deserialize<BE.Root>(json);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                   // Debug.WriteLine(e.Message);
                }

            }
            return CurrentFlight;
        }
       
    }
   
}

