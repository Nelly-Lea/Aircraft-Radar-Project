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
using System.Windows.Threading;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;

namespace AirTraffic.Radar
{
    /// <summary>
    /// Interaction logic for RadarView.xaml
    /// </summary>
    public partial class RadarView : UserControl
    {
        
        BE.FlightInfoPartial SelectedFlight = null;
        public BE.Root Flight = new BE.Root();
        public RadarViewModel radarViewModel;
        public string ImagePath= "C:\\Projet aircraft radar\\AirTraffic\\PL\\images\\";
        //private const string AllURL = @" https://data-cloud.flightradar24.com/zones/fcgi/feed.js?faa=1&bounds=38.805%2C24.785%2C29.014%2C40.505&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        //private const string FlightURL = @"https://data-live.flightradar24.com/clickhandler/?version=1.5&flight=";

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
            //var pin = e.OriginalSource as Pushpin;
            //if (pin != null)
            //    MessageBox.Show(pin.ToolTip.ToString());
            FlightDataView fv = new FlightDataView();


            fv.DetailsPanel.DataContext = Flight;
            ObservableCollection<BE.RootWeather> obsWeather = new ObservableCollection<BE.RootWeather>();
            obsWeather = radarViewModel.RVMDisplayWeather(Flight);
            
            fv.DesWeatherOrigin.Content = obsWeather[0].weather[0].description;
            fv.TempWeatherOrigin.Content = obsWeather[0].main.temp.ToString();
            fv.DesWeatherDestination.Content = obsWeather[1].weather[0].description;
            fv.TempWeatherDestination.Content = obsWeather[1].main.temp.ToString();
            string pathOri = "/images/" + obsWeather[0].weather[0].icon.ToString() + ".png";
            Uri resourceUri = new Uri(pathOri, UriKind.Relative);
            fv.IconWeatherOrigin.Source = new BitmapImage(resourceUri);
            string pathDest = "/images/" + obsWeather[1].weather[0].icon.ToString() + ".png";
            Uri resourceUridest = new Uri(pathDest, UriKind.Relative);
            fv.IconWeatherDestination.Source = new BitmapImage(resourceUridest);
            // fv.IconWeatherOrigin.Source= new BitmapImage(new Uri(ImagePath + obsWeather[0].weather[0].icon.ToString() + ".png"));
            //fv.IconWeatherDestination.Source= new BitmapImage(new Uri(ImagePath + obsWeather[1].weather[0].icon.ToString()+".png"));
            myGrid.Children.Add(fv);
            Grid.SetColumn(fv, 1);
            Grid.SetRow(fv, 1);
        }


        //private void Button_Click_1(object sender, RoutedEventArgs e) // pr rajouter l'onglet
        //{
        //    FlightDataView fv = new FlightDataView();
        //    radarviewgrid.Children.Add(fv);


        //}

        private void FlightsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            SelectedFlight = e.AddedItems[0] as BE.FlightInfoPartial; //InFlightsListBox.SelectedItem as FlightInfoPartial;
            UpdateFlight(SelectedFlight);
            Flight = radarViewModel.ConvertFlightIPToFlighInfo(SelectedFlight);
            radarViewModel.SaveFlightToDB(SelectedFlight);




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
                
                Pushpin PinCurrent = new Pushpin { ToolTip = selected.FlightCode };
                Pushpin PinOrigin = new Pushpin { ToolTip = Flight.airport.origin.name };
                Pushpin PinDestination = new Pushpin { ToolTip = Flight.airport.destination.name };
                PinCurrent.MouseDoubleClick += Pin_MouseDown;
                PinCurrent.MouseEnter += PinCurrent_MouseEnter;
                

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

        private void PinCurrent_MouseEnter(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            
            if (Flight.airport.destination.code.iata == "TLV")
            {
                pushpin.Style = (Style)Resources["ToIsraelRed"];
            }
            else
            {
                pushpin.Style = (Style)Resources["FromIsraelRed"];
            }


            pushpin.MouseLeave += Pushpin_MouseLeave;
        }

        private void Pushpin_MouseLeave(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            
            if (Flight.airport.destination.code.iata == "TLV")
            {
                pushpin.Style = (Style)Resources["ToIsrael"];
            }
            else
            {
                pushpin.Style = (Style)Resources["FromIsrael"];
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

        private void ButtonCounter(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimer.Start();

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateFlight(SelectedFlight);
            Counter.Text = (Convert.ToInt32(Counter.Text) + 1).ToString();
        }


        //private double angleFromCoordinate(double lat1, double long1, double lat2,
        //double long2)
        //{

        //    double dLon = (long2 - long1);

        //    double y = Math.Sin(dLon) * Math.Cos(lat2);
        //    double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
        //            * Math.Cos(lat2) * Math.Cos(dLon);

        //    double brng = Math.Atan2(y, x);

        //    brng = brng*57.296;
        //    brng = (brng + 360) % 360;
        //    brng = 360 - brng; // count degrees counter-clockwise - remove to make clockwise

        //    return brng;
        //}

        //private void PushpinClick(object sender, RoutedEventArgs e)
        //{
        //    Pushpin p = sender as Pushpin;
        //   // string flight = p.Tag.ToString();
        //    //BE.Root Flight = new BE.Root();
        //    //Flight = GetFlight(flight);

        //    FlightDataView fv = new FlightDataView();
        //    radarviewgrid.Children.Add(fv);


        //}

        //public BE.Root GetFlight(string Key)
        //{
        //    var CurrentUrl = FlightURL + Key;
        //    BE.Root CurrentFlight = null;
        //    //must use try-catch
        //    using (var webClient = new System.Net.WebClient())
        //    {
        //        var json = webClient.DownloadString(CurrentUrl);
        //        try
        //        {
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            CurrentFlight = serializer.Deserialize<BE.Root>(json);
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //           // Debug.WriteLine(e.Message);
        //        }

        //    }
        //    return CurrentFlight;
        //}

    }

}

