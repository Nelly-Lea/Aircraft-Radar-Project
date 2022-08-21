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

        public Location RMGetOriginAirport(BE.Root flight)
        {
            return BL.GetOriginAirport(flight);
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

        public Location RMGetBeforeLastPosition(List<BE.Trail>OrderedPlaces)
        {
            return BL.GetBeforeLastPosition(OrderedPlaces);
        }
        public ObservableCollection<BE.RootWeather> RMDisplayWeather(BE.Root flight)
        {
            return BL.DisplayWeather(flight);
        }
        public double Angle(double lat1, double long1, double lat2, double long2)
        {
            return BL.Angle(lat1, long1, lat2, long2);
        }
        public List<BE.Root> RMGetAllFLightsRoot()
        {
            return BL.GetAllFlightRoot();
        }
    }
}
