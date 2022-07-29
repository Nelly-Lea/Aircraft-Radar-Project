using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Juste les noms des fonctions qui sont ds BLImp 
namespace BL
{
    public interface IBL
    {
        #region Flights
        Dictionary<string, List<BE.FlightInfoPartial>> GetCurrentFlights();
        BE.Root GetFlightData(string Key);

        void SaveFlightToDataBase(BE.FlightInfoPartial flight);

        #endregion
    }
}
