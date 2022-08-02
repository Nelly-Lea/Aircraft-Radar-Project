﻿using System;
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

            List<BE.FlightInfoPartial> AllFlights = dal.GetAllCurrentFlights();
            List<BE.FlightInfoPartial> Incoming = new List<BE.FlightInfoPartial>();
            List<BE.FlightInfoPartial> Outgoing = new List<BE.FlightInfoPartial>();
            foreach (var item in AllFlights)
            {
                if (item.Source == "TLV")
                    Outgoing.Add(item);
                else //if item.Destination== TLV
                    Incoming.Add(item);
            }
            Result.Add("Incoming", Incoming);
            Result.Add("Outgoing", Outgoing);

            return Result;

        }

        public BE.Root GetFlightData(string Key)
        {
            return dal.GetFlight(Key);
        }

        public void SaveFlightToDataBase(BE.FlightInfoPartial flight)
        {
            dal.SaveFlightToDB(flight);
        
        }

        public ObservableCollection<BE.FlightInfoPartial> GetFlightsBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            List<BE.FlightInfoPartial> ListOfFlightInDB = dal.GetAllFlightInDB();
            List<BE.FlightInfoPartial> ListPred=ListOfFlightInDB.FindAll(p => p.DateAndTime.Date <= dateTo.Date && p.DateAndTime.Date >= dateFrom.Date);
            ObservableCollection<BE.FlightInfoPartial> Obs = new ObservableCollection<BE.FlightInfoPartial>(ListOfFlightInDB);
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

    }
}
