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
        public List<BE.FlightInfoPartial> ListOutFlight = null;
        public List<BE.FlightInfoPartial> ListInFlight = null;
        private ObservableCollection<BE.FlightInfoPartial> listInFlight = new ObservableCollection<BE.FlightInfoPartial>();

        public RadarModel radarModel { get; set; }

        public RadarViewModel()
        {
            radarModel = new RadarModel();

        }
        public void DisplayCurrentFilghts()
        {
            var CurrentFlight = radarModel.RMDisplayCurrentFlight();
            listInFlight = new ObservableCollection<BE.FlightInfoPartial>(CurrentFlight["Incoming"]);
            ListOutFlight = CurrentFlight["Outgoing"];

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

