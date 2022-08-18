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
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.IO;

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
        public string ImagePath = "C:\\Projet aircraft radar\\AirTraffic\\PL\\images\\";
        //private const string AllURL = @" https://data-cloud.flightradar24.com/zones/fcgi/feed.js?faa=1&bounds=38.805%2C24.785%2C29.014%2C40.505&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        //private const string FlightURL = @"https://data-live.flightradar24.com/clickhandler/?version=1.5&flight=";
        public List<BE.Root> ListAllFLightsRoot = new List<BE.Root>();
        DispatcherTimer dispatcherTimerAllFlights = new DispatcherTimer();
        DispatcherTimer dispatcherTimerOneFlight = new DispatcherTimer();
        

        public RadarView()
        {
            radarViewModel = new RadarViewModel();
            InitializeComponent();

            this.DataContext = radarViewModel;
            Init();
            //FlightDataView fv = new FlightDataView();
            //radarviewgrid.Children.Add(fv);
            dispatcherTimerAllFlights.Tick += DispatcherTimer_TickAllFlights;
            dispatcherTimerAllFlights.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimerAllFlights.Start();

        }

        private void Init()
        {
            ListAllFLightsRoot = radarViewModel.RVMGetAllFlightsRoot();
            foreach (var item in ListAllFLightsRoot)
            {
                UpdateFlight2(item);
                
            }

            
           
        }

        
        //double click sur vol
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

            string pathImagePlane = "/images/"+Flight.airline.name.ToString()+ ".jpg";

            Uri resourceImPlane = new Uri(pathImagePlane, UriKind.Relative);
           
            fv.ImagePlane.Source = new BitmapImage(resourceImPlane);
            string s = "C:/Users/USER/Documents/project maarehot halonot/projet github/AirTraffic/PL";
             FileInfo file = new FileInfo(s+pathImagePlane);
            if (file.Exists.Equals(false))
            {
                string pathImagePlaneDefault = "/images/Default Image.jpg";
                Uri resourcePlaneDefault = new Uri(pathImagePlaneDefault, UriKind.Relative);
                fv.ImagePlane.Source = new BitmapImage(resourcePlaneDefault);
            }

            
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
            try
            {
                SelectedFlight = e.AddedItems[0] as BE.FlightInfoPartial; //InFlightsListBox.SelectedItem as FlightInfoPartial;
                UpdateFlight(SelectedFlight);
                Flight = radarViewModel.ConvertFlightIPToFlighInfo(SelectedFlight);
                radarViewModel.SaveFlightToDB(SelectedFlight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }



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


                PinCurrent.Style = (Style)Resources["IsraelYellow"];

                //Better to use RenderTransform
                //if (Flight.airport.destination.code.iata == "TLV")
                //{
                //    PinCurrent.Style = (Style)Resources["ToIsrael"];
                //}
                //else
                //{
                //    PinCurrent.Style = (Style)Resources["FromIsrael"];
                //}

                CurrentPlace = radarViewModel.RVMGetCurrentPosition(OrderedPlaces);
                var PlaneLocation = radarViewModel.RVMGetPosition(CurrentPlace);
                PinCurrent.Location = PlaneLocation;

                Location BeforeLastPosition = radarViewModel.RVMGetBeforeLastPosition(OrderedPlaces);
                
                radarViewModel.angleFromCoordinat(BeforeLastPosition.Latitude, BeforeLastPosition.Longitude, PinCurrent.Location.Latitude, PinCurrent.Location.Longitude);
               

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
            pushpin.Style = (Style)Resources["IsraelRed"];
            
           
            pushpin.MouseLeave += Pushpin_MouseLeave;
        }

        private void PinCurrent_MouseEnter2(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            if(pushpin.Style== (Style)Resources["IsraelYellowNorth"])
                pushpin.Style = (Style)Resources["IsraelRedNorth"];
            if (pushpin.Style == (Style)Resources["IsraelYellowNorthEast"])
                pushpin.Style = (Style)Resources["IsraelRedNorthEast"];
            if (pushpin.Style == (Style)Resources["IsraelYellowEast"])
                pushpin.Style = (Style)Resources["IsraelRedEast"];
            if (pushpin.Style == (Style)Resources["IsraelYellowEastSouth"])
                pushpin.Style = (Style)Resources["IsraelRedEastSouth"];
            if (pushpin.Style== (Style)Resources["IsraelYellowSouth"])
                pushpin.Style = (Style)Resources["IsraelRedSouth"];
            if (pushpin.Style == (Style)Resources["IsraelYellowSouthWest"])
                pushpin.Style = (Style)Resources["IsraelRedSouthWest"];
            if (pushpin.Style == (Style)Resources["IsraelYellowWest"])
                pushpin.Style = (Style)Resources["IsraelRedWest"];
            if (pushpin.Style == (Style)Resources["IsraelYellowWestNorth"])
                pushpin.Style = (Style)Resources["IsraelRedWestNorth"];



            pushpin.MouseLeave += Pushpin_MouseLeave2;
        }
        private void Pushpin_MouseLeave(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            pushpin.Style = (Style)Resources["IsraelYellow"];
      
        }
        private void Pushpin_MouseLeave2(object sender, MouseEventArgs e)
        {
            Pushpin pushpin = sender as Pushpin;
            if (pushpin.Style == (Style)Resources["IsraelRedNorth"])
                pushpin.Style = (Style)Resources["IsraelYellowNorth"];
            if (pushpin.Style == (Style)Resources["IsraelRedNorthEast"])
                pushpin.Style = (Style)Resources["IsraelYellowNorthEast"];
            if (pushpin.Style == (Style)Resources["IsraelRedEast"])
                pushpin.Style = (Style)Resources["IsraelYellowEast"];
            if (pushpin.Style == (Style)Resources["IsraelRedEastSouth"])
                pushpin.Style = (Style)Resources["IsraelYellowEastSouth"];

            if (pushpin.Style == (Style)Resources["IsraelRedSouth"])
                pushpin.Style = (Style)Resources["IsraelYellowSouth"];
            if (pushpin.Style == (Style)Resources["IsraelRedSouthWest"])
                pushpin.Style = (Style)Resources["IsraelYellowSouthWest"];

            if (pushpin.Style == (Style)Resources["IsraelRedWest"])
                pushpin.Style = (Style)Resources["IsraelYellowWest"];
            if (pushpin.Style == (Style)Resources["IsraelRedWestNorth"])
                pushpin.Style = (Style)Resources["IsraelYellowWestNorth"];
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

        void addNewPolyLineAllFlights(List<BE.Trail> Route)
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

            //myMap.Children.Clear();
            myMap.Children.Add(polyline);
        }

        private void ButtonCounterOneFlightClick(object sender, RoutedEventArgs e)
        {
            dispatcherTimerOneFlight.Tick += DispatcherTimer_TickOneFlight;
            dispatcherTimerOneFlight.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimerOneFlight.Start();

        }

        private void DispatcherTimer_TickOneFlight(object sender, EventArgs e)
        {
            UpdateFlight(SelectedFlight);
            CounterOneFlight.Text = (Convert.ToInt32(CounterOneFlight.Text) + 1).ToString();
        }

    

        private void DispatcherTimer_TickAllFlights(object sender, EventArgs e)
        {

            ListAllFLightsRoot = radarViewModel.RVMGetAllFlightsRoot();
            myMap.Children.Clear();
            foreach (var item in ListAllFLightsRoot)
            {
                UpdateFlight2(item);
            }
            
        }

        private void FocusOneFlightButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimerAllFlights.Stop();
            myMap.Children.Clear();
            LabelOut.Visibility = Visibility.Visible;
            labelIncom.Visibility = Visibility.Visible;
            InFlightsListBox.Visibility= Visibility.Visible;
            OutFlightsListBox.Visibility = Visibility.Visible;
            ButtonReadAll.Visibility = Visibility.Visible;
            ButtonCounterOneFlight.Visibility = Visibility.Visible;
            CounterOneFlight.Visibility = Visibility.Visible;
            DisplayAllFlightsButton.Visibility = Visibility.Visible;
            FocusOneFlightButton.Visibility = Visibility.Hidden;

        }

        private void UpdateFlight2(BE.Root flight)
        {


            // DetailsPanel.DataContext = Flight;
            BE.Root Flight = new BE.Root();
            Flight = flight;

            // Update map
            if (Flight != null)
            {

                var OrderedPlaces = radarViewModel.RVMGetTrail(Flight);
                //MessageBox.Show(Flight.airport.destination.code.iata);
                //addNewPolyLineAllFlights(OrderedPlaces);
                BE.Trail CurrentPlace = null;

                Pushpin PinCurrent = new Pushpin { ToolTip = flight.identification.number.@default};
                Pushpin PinOrigin = new Pushpin { ToolTip = Flight.airport.origin.name };
                Pushpin PinDestination = new Pushpin { ToolTip = Flight.airport.destination.name };
                //PinCurrent.MouseDoubleClick += Pin_MouseDown;
                PinCurrent.MouseEnter += PinCurrent_MouseEnter2;

           
                PositionOrigin origin = new PositionOrigin { X = 0.4, Y = 0.4 };
                MapLayer.SetPositionOrigin(PinCurrent, origin);

              

                CurrentPlace = radarViewModel.RVMGetCurrentPosition(OrderedPlaces);
                var PlaneLocation = radarViewModel.RVMGetPosition(CurrentPlace);
                PinCurrent.Location = PlaneLocation;

                Location BeforeLastPosition = radarViewModel.RVMGetBeforeLastPosition(OrderedPlaces);

                //calculate AngleRot
                double angleRot= radarViewModel.angleFromCoordinat(BeforeLastPosition.Latitude, BeforeLastPosition.Longitude, PinCurrent.Location.Latitude, PinCurrent.Location.Longitude);


                var OriginPlace = radarViewModel.RVMGetOriginAirport(OrderedPlaces);
                PlaneLocation = radarViewModel.RVMGetPosition(OriginPlace);
                PinOrigin.Location = PlaneLocation;


                var AirportDst = radarViewModel.RVMGetPosition(Flight);
                PinDestination.Location = AirportDst;
                string planeDirection=radarViewModel.PlaneDirection(angleRot);
                

                if(planeDirection=="north")
                    PinCurrent.Style = (Style)Resources["IsraelYellowNorth"];
                if(planeDirection=="northEast")
                    PinCurrent.Style = (Style)Resources["IsraelYellowNorthEast"];
                if (planeDirection=="east")
                    PinCurrent.Style = (Style)Resources["IsraelYellowEast"];
                if (planeDirection == "eastSouth")
                    PinCurrent.Style = (Style)Resources["IsraelYellowEastSouth"];
                if (planeDirection == "south")
                    PinCurrent.Style = (Style)Resources["IsraelYellowSouth"];
                if (planeDirection == "southWest")
                    PinCurrent.Style = (Style)Resources["IsraelYellowSouthWest"];
                if (planeDirection=="west")
                    PinCurrent.Style = (Style)Resources["IsraelYellowWest"];
                if (planeDirection == "westNorth")
                    PinCurrent.Style = (Style)Resources["IsraelYellowWestNorth"];




                myMap.Children.Add(PinOrigin);
                myMap.Children.Add(PinCurrent);
                myMap.Children.Add(PinDestination);

            }
        }

        private void DisplayAllFlightsButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimerOneFlight.Stop();
            myMap.Children.Clear();
            LabelOut.Visibility = Visibility.Hidden;
            labelIncom.Visibility = Visibility.Hidden;
            InFlightsListBox.Visibility = Visibility.Hidden;
            OutFlightsListBox.Visibility = Visibility.Hidden;
            ButtonReadAll.Visibility = Visibility.Hidden;
            ButtonCounterOneFlight.Visibility = Visibility.Hidden;
            CounterOneFlight.Visibility = Visibility.Hidden;
            DisplayAllFlightsButton.Visibility = Visibility.Hidden;
            FocusOneFlightButton.Visibility = Visibility.Visible;
            Init();
            var items = myGrid.Children.Cast<UIElement>().Where(i => Grid.GetRow(i) == 1 && Grid.GetColumn(i) == 1);
            foreach(var item in items.ToList())
            {
                if (item is PL.FlightData.FlightDataView)
                    myGrid.Children.Remove(item);
            }
            
            radarViewModel.FlightOutgoing.Clear();
            radarViewModel.FlightIncoming.Clear();
            dispatcherTimerAllFlights.Start();

        }
    }

}

