using BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Historic
{

    public class HistoricModel
    {
        IBL BL;
        public HistoricModel()
        {
            BL = new BLImp();
        }

        public ObservableCollection<BE.FlightInfoPartial> DisplayFlightBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
           return BL.GetFlightsBetweenTwoDates(dateFrom, dateTo);
        }

        public void HMDeleteFlight(BE.FlightInfoPartial flight)
        {
            BL.DeleteFlight(flight);
        }
    }
}
