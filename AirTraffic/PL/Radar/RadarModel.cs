using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

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
    }
}
