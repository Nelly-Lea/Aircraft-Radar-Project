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

namespace AirTraffic.Radar
{

    public class RadarViewModel
    {
        public RadarModel radarModel { get; set; }

        public RadarViewModel()
        {
            radarModel = new RadarModel();


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
         
        }

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

        public void angleFromCoordinat(double lat1, double long1, double lat2, double long2)
        {

            AngleRot = radarModel.Angle(lat1, long1, lat2, long2);
            //return brng;
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

        public BE.Trail RVMGetOriginAirport(List<BE.Trail> OrderedPlaces)
        {
            return radarModel.RMGetOriginAirport(OrderedPlaces);
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
        //public ICommand HolidaysCommand
        //{
        //    get
        //    {
        //        return new HolidaysCommand(this);
        //    }
        //}
    }
}

