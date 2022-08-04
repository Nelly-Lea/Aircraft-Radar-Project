using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// on met le nom des fonctions qui sont ds le DALimp
namespace DAL
{
    public interface IDAL
    {
       // static bool Holiday;

        #region Flight
        List<BE.FlightInfoPartial> GetAllCurrentFlights();

        BE.Root GetFlight(string Key);
        void SaveFlightToDB(BE.FlightInfoPartial flight);

        List<BE.FlightInfoPartial> GetAllFlightInDB();

        void DeleteFlight(BE.FlightInfoPartial flight);
        List<BE.Trail> getTrail(BE.Root flight);
        BE.Trail GetOriginAirport(List<BE.Trail> OrderedPlaces);

        BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces);

        Location GetPosition(BE.Root flight);

        Location GetPosition(BE.Trail trail);



        #endregion

        #region HebCal
       
        bool IsBeforeHolidayAsync(DateTime date);
        // bool IsBeforeHolidayAsync1();
        #endregion
        #region Weahter
        BE.RootWeather GetWeatherOfAirport(double latitude, double longitude);
        #endregion
    }
}
