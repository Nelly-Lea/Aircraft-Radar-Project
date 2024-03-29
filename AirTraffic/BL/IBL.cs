﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<BE.FlightInfoPartial> GetFlightsBetweenTwoDates(DateTime dateFrom, DateTime dateTo);
        void DeleteFlight(BE.FlightInfoPartial flight);
        List<BE.Root> GetAllFlightRoot();
        BE.Root getRootFromFlightCode(string flightCode);
        #endregion
        #region Position
        List<BE.Trail> getTrail(BE.Root flight);

        Location GetOriginAirport(BE.Root flight);



        BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces);

        Location GetPosition(BE.Root flight);

        Location GetPosition(BE.Trail trail);
        Location GetBeforeLastPosition(List<BE.Trail> OrderedPlaces);
        
        #endregion

        #region Holidays
        bool IsBeforeHolidays(DateTime date);

        #endregion

        #region Weather
        ObservableCollection<BE.RootWeather> DisplayWeather(BE.Root flight);
        #endregion
        double Angle(double lat1, double long1, double lat2, double long2);
    }
}
