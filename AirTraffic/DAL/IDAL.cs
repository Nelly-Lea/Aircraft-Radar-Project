using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
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
        // List<BE.FlightInfoPartial> GetAllCurrentFlights();
         Task GetAllCurrentFlight1();
        Task<JObject> GetAllCurrentFlights();
        List<BE.FlightInfoPartial> GetAllCurrentFlightsL();

        Task<BE.Root> GetFlightRoot(string Key);
        Task<BE.Root> GetFlightAsync(string Key);
        void SaveFlightToDB(BE.FlightInfoPartial flight);

        List<BE.FlightInfoPartial> GetAllFlightInDB();

        void DeleteFlight(BE.FlightInfoPartial flight);
        #endregion
        #region Position
        List<BE.Trail> getTrail(BE.Root flight);
   
        BE.Trail GetOriginAirport(List<BE.Trail> OrderedPlaces);

        BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces);


        Location GetPosition(BE.Root flight);

        Location GetPosition(BE.Trail trail);

        Location GetBeforeLastPosition(List<BE.Trail> OrderedPlaces);
        #endregion




        #region HebCal

        // bool IsBeforeHolidayAsync1(string jason);
        ObservableCollection<bool> GetObsHolidays();
        Task IsBeforeHolidayAsync1(DateTime date);
        Task<bool> IsBeforeHolidayAsync(DateTime date);


        #endregion
        #region Weather
        BE.RootWeather GetWeatherOfAirport(double latitude, double longitude);
        #endregion
    }
}
