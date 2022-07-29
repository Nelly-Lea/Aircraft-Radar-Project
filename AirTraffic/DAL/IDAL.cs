using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// on met le nom des fonctions qui sont ds le DALimp
namespace DAL
{
    public interface IDAL
    {
        #region Flight
        List<BE.FlightInfoPartial> GetAllCurrentFlights();

        BE.Root GetFlight(string Key);
        void SaveFlightToDB(BE.FlightInfoPartial flight);
        #endregion
    }
}
