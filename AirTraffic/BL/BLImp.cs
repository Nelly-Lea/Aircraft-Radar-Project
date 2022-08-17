using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Maps.MapControl.WPF;

namespace BL
{
    //les fonctions vont recevoir des object de type BE.class et retourn BE.class
    // on retourn dal.fonction
    public class BLImp : IBL
    {
        public IDAL dal { get; set; }
        public BLImp()
        {
            dal = new DALImp();
        }
        public Dictionary<string, List<BE.FlightInfoPartial>> GetCurrentFlights()
        {
            Dictionary<string, List<BE.FlightInfoPartial>> Result = new Dictionary<string, List<BE.FlightInfoPartial>>();
            dal.GetAllCurrentFlight1();
            List<BE.FlightInfoPartial> AllFlights = dal.GetAllCurrentFlightsL();
            List<BE.FlightInfoPartial> Incoming = new List<BE.FlightInfoPartial>();
            List<BE.FlightInfoPartial> Outgoing = new List<BE.FlightInfoPartial>();

            foreach (var item in AllFlights)
            {
                BE.Root CurrentFlightBL = dal.GetFlightRoot(item.SourceId).Result;
                if (item.Source == "TLV" && CurrentFlightBL != null)
                    Outgoing.Add(item);
                else
                {
                    if (item.Destination == "TLV" && CurrentFlightBL != null)
                        Incoming.Add(item);
                }
            }
            Result.Add("Incoming", Incoming);
            Result.Add("Outgoing", Outgoing);

            return Result;

        }

        public BE.Root GetFlightData(string Key)
        {
            return dal.GetFlightRoot(Key).Result;
        }

        public void SaveFlightToDataBase(BE.FlightInfoPartial flight)
        {
            dal.SaveFlightToDB(flight);

        }

        public ObservableCollection<BE.FlightInfoPartial> GetFlightsBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            List<BE.FlightInfoPartial> ListOfFlightInDB = dal.GetAllFlightInDB();
            List<BE.FlightInfoPartial> ListPred = ListOfFlightInDB.FindAll(p => p.DateAndTime.Date <= dateTo.Date && p.DateAndTime.Date >= dateFrom.Date);

            ObservableCollection<BE.FlightInfoPartial> Obs = new ObservableCollection<BE.FlightInfoPartial>(ListPred);
            return Obs;
        }

        public void DeleteFlight(BE.FlightInfoPartial flight)
        {
            dal.DeleteFlight(flight);
        }

        public List<BE.Trail> getTrail(BE.Root flight)
        {
            return dal.getTrail(flight);

        }



        public BE.Trail GetOriginAirport(List<BE.Trail> OrderedPlaces)
        {
            return dal.GetOriginAirport(OrderedPlaces);
        }

        public BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces)
        {
            return dal.GetCurrentPosition(OrderedPlaces);
        }

        public Location GetPosition(BE.Root flight)
        {
            return dal.GetPosition(flight);
        }

        public Location GetPosition(BE.Trail trail)
        {
            return dal.GetPosition(trail);
        }

        public Location GetBeforeLastPosition(List<BE.Trail> OrderedPlaces)
        {
            return dal.GetBeforeLastPosition(OrderedPlaces);
        }

        #region Holidays
        public bool IsBeforeHolidays(DateTime date)
        {

            dal.IsBeforeHolidayAsync1(date);
            ObservableCollection<bool> obsHol = new ObservableCollection<bool>();
            obsHol = dal.GetObsHolidays();
            foreach (var item in obsHol)
            {
                if (item)
                    return true;
            }
            return false;


        }
        #endregion

        public ObservableCollection<BE.RootWeather> DisplayWeather(BE.Root flight)
        {
            Double latOrigin = flight.airport.origin.position.latitude;
            Double lngOrigin = flight.airport.origin.position.longitude;
            Double latDestination = flight.airport.destination.position.latitude;
            Double lngDestination = flight.airport.destination.position.longitude;
            ObservableCollection<BE.RootWeather> obsWeather = new ObservableCollection<BE.RootWeather>();

            obsWeather.Add(dal.GetWeatherOfAirport(latOrigin, lngOrigin));
            obsWeather.Add(dal.GetWeatherOfAirport(latDestination, lngDestination));
            obsWeather[0].main.temp -= 273.15;
            obsWeather[1].main.temp -= 273.15;
            return obsWeather;
        }

        public double Angle(double lat1, double long1, double lat2, double long2)
        {
            double dLon = (long2 - long1);

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
                    * Math.Cos(lat2) * Math.Cos(dLon);


            double brng = Math.Atan2(y, x);

            brng = brng * 57.296;
            brng = (brng + 360) % 360;
            brng = 360 - brng; // count degrees counter-clockwise - remove to make clockwise
            return brng;
        }
        public List<BE.Root> GetAllFlightRoot()
        {
            List<BE.Root> listFlights = new List<BE.Root>();
            dal.GetAllCurrentFlight1();
            List<BE.FlightInfoPartial> AllFlights = dal.GetAllCurrentFlightsL();
            BE.Root flight = new BE.Root();
            foreach(var item in AllFlights)
            {
                flight = dal.GetFlightRoot(item.SourceId).Result;
                if(flight!=null)
                    listFlights.Add(flight);
            }
            return listFlights;
        }
    }

  
}
