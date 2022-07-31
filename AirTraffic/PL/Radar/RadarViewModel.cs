using AirTraffic.Commands;
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
            FlightData=radarModel.RMDisplayFlightData(key);
            return FlightData;
        }
        public ICommand ReadAllCommand
        {
            get
            {
                return new ReadAllCommand(this);
            }
        }

    }
}

