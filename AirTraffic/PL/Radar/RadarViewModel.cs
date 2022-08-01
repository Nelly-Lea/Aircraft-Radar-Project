using AirTraffic.Commands;
using PL.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private bool _isMyUserControlVisible;
        public bool IsMyUserControlVisible
        {
            get { return _isMyUserControlVisible; }
            set
            {
                _isMyUserControlVisible = value;
                OnPropertyChanged("IsMyUserControlVisible");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            //throw new NotImplementedException();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
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

        public void DisplayFlightData(string key)
        {
           BE.Root FlightData = new BE.Root();
            FlightData = radarModel.RMDisplayFlightData(key);
            if (CurrentFlight.Count != 0)
                CurrentFlight.Clear();
            CurrentFlight.Add(FlightData);
            IsMyUserControlVisible = true;


        }
        public ICommand ReadAllCommand
        {
            get
            {
                return new ReadAllCommand(this);
            }
        }
        public ICommand SelectionChangedCommand
        {
            get
            {
                return new SelectionChangedCommand(this);
            }
        }
    }
}

