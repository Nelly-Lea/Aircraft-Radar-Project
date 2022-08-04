using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Microsoft.Maps.MapControl.WPF;

namespace AirTraffic.Radar
{
    public class RadarModel
    {
        IBL BL;
        public RadarModel()
        {
            BL = new BLImp();
        }
        public Dictionary<string, List<BE.FlightInfoPartial>> RMDisplayCurrentFlight()
        {
            return BL.GetCurrentFlights();
        }
        public BE.Root RMDisplayFlightData(string key)
        {
            return BL.GetFlightData(key);
        }
        public void RMSaveFlightToDB(BE.FlightInfoPartial SelectedFlight)
        {
            BL.SaveFlightToDataBase(SelectedFlight);
        }

        public List<BE.Trail> RMGetTrail(BE.Root flight)
        {
            return BL.getTrail(flight);
        }

        public BE.Trail RMGetOriginAirport(List<BE.Trail> OrderedPlaces)
        {
            return BL.GetOriginAirport(OrderedPlaces);
        }

        public BE.Trail RMGetCurrentPosition(List<BE.Trail> OrderedPlaces)
        {
            return BL.GetCurrentPosition(OrderedPlaces);
        }

        public Location RMGetPosition(BE.Root flight)
        {
            return BL.GetPosition(flight);
        }

        public Location RMGetPosition(BE.Trail trail)
        {
            return BL.GetPosition(trail);
        }
        public ObservableCollection<BE.RootWeather> RMDisplayWeather(BE.Root flight)
        {
            return BL.DisplayWeather(flight);
        }
    }
}
