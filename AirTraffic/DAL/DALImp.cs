using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BE;
using System.Data.Entity;
using Microsoft.Maps.MapControl.WPF;
//on recoit des BE.class, ds les fonctions on fait ctx=class.DB on le retourne
//on retourne List<BE.Class>
//on s'occupe de la data
namespace DAL
{
    public class DALImp : IDAL
    {
        public DALImp() { }
        //public class TrafficAdapter
        //{
        //    //ancien url
        //https://data-cloud.flightradar24.com/zones/fcgi/feed.js?faa=1&bounds=32.366%2C31.271%2C34.012%2C36.282&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1

        private const string AllURL = @" https://data-cloud.flightradar24.com/zones/fcgi/feed.js?faa=1&bounds=38.805%2C24.785%2C29.014%2C40.505&satellite=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=14400&gliders=1&stats=1";
        private const string FlightURL = @"https://data-live.flightradar24.com/clickhandler/?version=1.5&flight=";

        //public List< List<BE.FlightInfoPartial>> GetAllCurrentFlights()
        //{

        //    //Dictionary<string, List<BE.FlightInfoPartial>> Result = new Dictionary<string, List<BE.FlightInfoPartial>>();//belongs to BL
        //    JObject AllFlightData = null;
        //    //IList<string> Keys = null;  pas utilise
        //    //IList<Object> Values = null; pas utilise

        //    List<List<BE.FlightInfoPartial>> Result = new List<List<BE.FlightInfoPartial>>();
        //    List<BE.FlightInfoPartial> Incoming = new List<BE.FlightInfoPartial>();
        //    List<BE.FlightInfoPartial> Outgoing = new List<BE.FlightInfoPartial>();

        //    using (var webClient = new System.Net.WebClient())
        //    {
        //        var json = webClient.DownloadString(AllURL);

        //        BE.HelperClass Helper = new BE.HelperClass();
        //        AllFlightData = JObject.Parse(json);

        //        try
        //        {
        //            foreach (var item in AllFlightData)
        //            {
        //                var key = item.Key;
        //                if (key == "full_count") continue;
        //                if (key == "version") continue;
        //                // il a mis id=-1 parce qu'on doit rajouter le id et savoir le gerer
        //                if (item.Value[11].ToString() == "TLV") Outgoing.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
        //                if (item.Value[12].ToString() == "TLV") Incoming.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
        //                // la on rajoute a la list d'avion rentrant et sortant de TLV
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.Print(e.Message);
        //        }
        //    }
        //    Result.Add(Incoming);
        //    Result.Add(Outgoing);
        //    return Result;
        //    //Result.Add("Incoming", Incoming);
        //    //Result.Add("Outgoing", Outgoing);

        //    //return Result;
        //}
        public List<BE.FlightInfoPartial> GetAllCurrentFlights() //faudrait se debrouiller pr faire async 
        {
            List<BE.FlightInfoPartial> AllCurrentFlights = new List<BE.FlightInfoPartial>();
            JObject AllFlightData = null;

            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(AllURL);

                BE.HelperClass Helper = new BE.HelperClass();
                AllFlightData = JObject.Parse(json);

                try
                {
                    foreach (var item in AllFlightData)
                    {
                        var key = item.Key;
                        if (key == "full_count") continue;
                        if (key == "version") continue;
                        // il a mis id=-1 parce qu'on doit rajouter le id et savoir le gerer
                        if (item.Value[11].ToString() == "TLV" || item.Value[12].ToString() == "TLV")
                            if(item.Value[11].ToString()!=""&& item.Value[12].ToString()!=""&& item.Value[13].ToString()!="")
                                AllCurrentFlights.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
                        //  if (item.Value[12].ToString() == "TLV") Incoming.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
                        // la on rajoute a la list d'avion rentrant et sortant de TLV
                    }
                    
                }
                catch (Exception e)
                {
                    
                    Debug.Print(e.Message);
                }
                return AllCurrentFlights;
            }
        }
        //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        //change root name to FlightData
        public BE.Root GetFlight(string Key)
        {
            var CurrentUrl = FlightURL + Key;
            BE.Root CurrentFlight = null;
            //must use try-catch
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(CurrentUrl);
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    CurrentFlight = serializer.Deserialize<BE.Root>(json);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            }
            return CurrentFlight;
        }
        public void SaveFlightToDB(BE.FlightInfoPartial flight)
        {
            List<BE.FlightInfoPartial> listFlights = new List<FlightInfoPartial>();

            using (var ctx = new FlightContext())
            {
                listFlights=(from f in ctx.Flights
                             where f.SourceId==flight.SourceId
                             select f).ToList<BE.FlightInfoPartial>();
                if (listFlights.Count == 0)
                {
                    ctx.Flights.Add(flight);
                    ctx.SaveChanges();
                }
            }

        }

        public void DeleteFlight(BE.FlightInfoPartial flight)
        {
            List<BE.FlightInfoPartial> listFlights = new List<FlightInfoPartial>();

            using (var ctx = new FlightContext())
            {
                //collection Flights est dans BL
                listFlights = ( from f in ctx.Flights
                                where f.SourceId==flight.SourceId
                                select f).ToList<BE.FlightInfoPartial>();
                if (listFlights.Count != 0)
                {

                    ctx.Flights.Remove(listFlights.First());
                    ctx.SaveChanges();

                }
            }
          
        }

        public BE.Trail GetOriginAirport(List<BE.Trail> OrderedPlaces)
        {
            return OrderedPlaces.First<BE.Trail>();
        }

        public BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces)
        {
            return OrderedPlaces.Last<BE.Trail>();
        }

        public Location GetPosition(BE.Root flight)
        {
            Location loc = new Location();
            loc.Latitude = flight.airport.destination.position.latitude;
            loc.Longitude = flight.airport.destination.position.longitude;
            return loc;
        }

        public Location GetPosition(BE.Trail trail)
        {
            Location loc = new Location();
            loc.Latitude = trail.lat;
            loc.Longitude = trail.lng;
            return loc;

        }


        public List<BE.Trail> getTrail(BE.Root flight)
        {
            var OrderedPlaces = (from f in flight.trail    //ds DALImp
                                 orderby f.ts
                                 select f).ToList<BE.Trail>();
            return OrderedPlaces;
        }

        public List<BE.FlightInfoPartial> GetAllFlightInDB()
        {
            List<BE.FlightInfoPartial> Flights = new List<FlightInfoPartial>();
            using (var ctx = new FlightContext())
            {
                //collection Flights est dans BL
                 Flights = (from f in ctx.Flights
                               select f).ToList<BE.FlightInfoPartial>();
            }
            return Flights;
        }
        public class FlightContext : DbContext
        {
            public FlightContext() : base("FlightsDataBase")
            {

            }
            public DbSet<BE.FlightInfoPartial> Flights { get; set; }



        }
    }
    //}
}
