using AirTraffic.Commands;
using Microsoft.Maps.MapControl.WPF;
using PL.Commands;
using PL.FlightData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AirTraffic.Radar
{

    public class RadarViewModel:INotifyPropertyChanged
    {
        public RadarModel radarModel { get; set; }

        public RadarViewModel()
        {
            radarModel = new RadarModel();


        }
        public void RaisePropertyChanged(string Name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }

        private ObservableCollection<BE.FlightInfoPartial> flightIncoming;
        public ObservableCollection<BE.FlightInfoPartial> FlightIncoming
        {
            get
            {
                if (flightIncoming == null)
                    flightIncoming = new ObservableCollection<BE.FlightInfoPartial>();
                return flightIncoming;
            }
            set
            {
                flightIncoming= value;
                RaisePropertyChanged("FlightIncoming");
            }

        }
        //private Image image;
        //public Image imageYellow
        //{
        //    get 
        //    {
        //        image = new Image();

        //        // string s= "C:/Users/USER/Documents/project maarehot halonot/vrai projet git/AirTraffic/PL/images/airplaneYellow.png";
        //        //   Uri imageUri = new Uri(s, UriKind.Relative);
           
        //            var uriSource = new Uri(@"C:\Users\USER\Documents\project maarehot halonot\projet github\AirTraffic\PL\images\airplaneYellow.png", UriKind.Relative);
        //       // BitmapImage bitmapImage = new BitmapImage(imageUri);
        //        image.Source= new BitmapImage(uriSource);
        //        return image;
        //    }
        //    set
        //    {
        //        image = value;
        //        OnPropertyChanged("imageYellow");
        //    }
        //}

        //public string DisplayImage
        //{
        //    get { return @"~\..\images\airplaneYellow.png"; }
        //}

        private double anglerot;
        public double  AngleRot
        {
            get { return anglerot; }
            set
            {
                anglerot = value;
                OnPropertyChanged("AngleRot");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            //throw new NotImplementedException();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public double angleFromCoordinat(double lat1, double long1, double lat2, double long2)
        {
            double angle= radarModel.Angle(lat1, long1, lat2, long2);
            
            AngleRot =0;
            AngleRot = angle;
            return AngleRot;
            //return brng;
        }

        public string PlaneDirection(double angle)
        {
            if ((angle >= 0 && angle <= 22.5) || (angle > 337.5 && angle <= 360))
                return "north";
            if (angle > 22.5 && angle <= 67.5)
                return "northEast";
            if (angle > 67.5 && angle <= 112.5)
                return "east";
            if (angle > 112.5 && angle <= 157.5)
                return "eastSouth";
            if (angle > 157.5 && angle <= 202.5)
                return "south";
            if (angle > 202.5 && angle <= 247.5)
                return "southWest";
            if (angle > 247.5 && angle <= 292.5)
                return "west";
            if (angle > 292.5 && angle <= 337.5)
                return "westNorth";
            return "north";
        }

        private ObservableCollection<BE.FlightInfoPartial> flightOutgoing;
        public ObservableCollection<BE.FlightInfoPartial> FlightOutgoing
        {
            get
            {
                if (flightOutgoing == null)
                    flightOutgoing = new ObservableCollection<BE.FlightInfoPartial>();
                return flightOutgoing;
            }
            set
            {
                flightOutgoing = value;
                RaisePropertyChanged("FlightOutgoing");
            }
        }

        private ObservableCollection< BE.Root> currentFlight { get; set; }
        public ObservableCollection<BE.Root> CurrentFlight
        {
            get
            {
                if (currentFlight == null)
                    currentFlight = new ObservableCollection<BE.Root>();
                return currentFlight;
            }

            //set
            //{
                
            //        currentFlight = value;
            //        OnPropertyChanged("currentFlight");
            //}
        }
        public void DisplayCurrentFilghts()
        {

            FlightIncoming.Clear();
            flightIncoming.Clear();
            FlightOutgoing.Clear();
            flightOutgoing.Clear();
            var CurrentFlight = radarModel.RMDisplayCurrentFlight();
            foreach (var item in CurrentFlight["Incoming"])
            {
                FlightIncoming.Add(item);
            }
            foreach (var item in CurrentFlight["Outgoing"])
            {
                FlightOutgoing.Add(item);
            }

        }

        public BE.Root DisplayFlightData(string key)
        {
           BE.Root FlightData = new BE.Root();
            FlightData = radarModel.RMDisplayFlightData(key);
            if (CurrentFlight.Count != 0)
                CurrentFlight.Clear();
            CurrentFlight.Add(FlightData);
    
            return FlightData;
            


        }
        public void SaveFlightToDB(BE.FlightInfoPartial SelectedFlight)
        {
            radarModel.RMSaveFlightToDB(SelectedFlight);
        }
        public BE.Root ConvertFlightIPToFlighInfo(BE.FlightInfoPartial FlightIP)
        {
            BE.Root Flight = new BE.Root();
            string key = FlightIP.SourceId;
            Flight = radarModel.RMDisplayFlightData(key);
            return Flight;

        }

        public List<BE.Trail> RVMGetTrail(BE.Root flight)
        {
            return radarModel.RMGetTrail(flight);
        }

        public Location RVMGetOriginAirport(BE.Root flight)
        {
            return radarModel.RMGetOriginAirport(flight);
        }

        public BE.Trail RVMGetCurrentPosition(List<BE.Trail> OrderedPlaces)
        {
            return radarModel.RMGetCurrentPosition(OrderedPlaces);
        }

        public Location RVMGetPosition(BE.Root flight)
        {
            return radarModel.RMGetPosition(flight);
        }

        public Location RVMGetPosition(BE.Trail trail)
        {
            return radarModel.RMGetPosition(trail);
        }

        public Location RVMGetBeforeLastPosition(List<BE.Trail>OrderedPlaces)
        {
            return radarModel.RMGetBeforeLastPosition(OrderedPlaces);
        }
        public ObservableCollection<BE.RootWeather> RVMDisplayWeather(BE.Root flight)
        {
            return radarModel.RMDisplayWeather(flight);
        }
        public ICommand ReadAllCommand
        {
            get
            {
                return new ReadAllCommand(this);
            }
        }

        public List<BE.Root> RVMGetAllFlightsRoot()
        {
            return radarModel.RMGetAllFLightsRoot();
        }

        public BE.Root RVMGetRoot(string flightCode)
        {
            List<BE.Root> listRoot = radarModel.RMGetAllFLightsRoot();
           
            foreach(var item in listRoot)
            {
                string s = item.identification.number.@default;
                if (s == flightCode)
                {
                    return item;
                }
            }
            return null;
        }
    }
}

