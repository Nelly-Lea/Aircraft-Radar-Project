using Newtonsoft.Json;
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
using System.Collections.ObjectModel;
//using Newtonsoft.Json;
//on recoit des BE.class, ds les fonctions on fait ctx=class.DB on le retourne
//on retourne List<BE.Class>
//on s'occupe de la data
namespace DAL
{
    public class DALImp : IDAL
    {
        public DALImp() { }
        public ObservableCollection<bool> obsHolidays = new ObservableCollection<bool>();
        public List<BE.FlightInfoPartial> AllCurrentFlights = new List<BE.FlightInfoPartial>();
        public BE.Root CurrentFlight = null;
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


        public List<BE.FlightInfoPartial> GetAllCurrentFlightsL()
        {
            return AllCurrentFlights;
        }
        public async Task GetAllCurrentFlight1()
        {
            AllCurrentFlights.Clear();
            BE.HelperClass Helper = new BE.HelperClass();
            JObject AllFlightData = await GetAllCurrentFlights();
            try
            {
                foreach (var item in AllFlightData)
                {
                    var key = item.Key;
                    if (key == "full_count") continue;
                    if (key == "version") continue;
                    // il a mis id=-1 parce qu'on doit rajouter le id et savoir le gerer
                    if (item.Value[11].ToString() == "TLV" || item.Value[12].ToString() == "TLV")
                        if (item.Value[11].ToString() != "" && item.Value[12].ToString() != "" && item.Value[13].ToString() != "")
                            AllCurrentFlights.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
                    //  if (item.Value[12].ToString() == "TLV") Incoming.Add(new BE.FlightInfoPartial { Id = -1, Source = item.Value[11].ToString(), Destination = item.Value[12].ToString(), SourceId = key, Long = Convert.ToDouble(item.Value[1]), Lat = Convert.ToDouble(item.Value[2]), DateAndTime = Helper.GetDateTimeFromEpoch(Convert.ToDouble(item.Value[10])), FlightCode = item.Value[13].ToString() });
                    // la on rajoute a la list d'avion rentrant et sortant de TLV
                }

            }
            catch (Exception e)
            {

                Debug.Print(e.Message);
            }
        //    return AllCurrentFlights;
        }


    
    public Task<JObject> GetAllCurrentFlights() //faudrait se debrouiller pr faire async 
    {
        //List<BE.FlightInfoPartial> AllCurrentFlights = new List<BE.FlightInfoPartial>();
        JObject AllFlightData = null;

        using (var webClient = new System.Net.WebClient())
        {
            var json = webClient.DownloadString(AllURL);

           
            AllFlightData = JObject.Parse(json);

        }
        return Task.FromResult(AllFlightData);
    }
        //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        //change root name to FlightData
        public async Task<BE.Root> GetFlightRoot(string Key)
        {
            await GetFlightAsync(Key);
            return CurrentFlight;
        }
        //public async Task GetFlightAsync(string key)
        //{
        //    await GetFlight(key);
        //}


        public Task<BE.Root> GetFlightAsync(string Key)
        {
            var CurrentUrl = FlightURL + Key;
            //BE.Root CurrentFlight = null;
            //must use try-catch
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(CurrentUrl);
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    CurrentFlight = null;
                    CurrentFlight = serializer.Deserialize<BE.Root>(json);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    CurrentFlight = null;
                }

            }
            return Task.FromResult(CurrentFlight);
        }
        public void SaveFlightToDB(BE.FlightInfoPartial flight)
        {
            List<BE.FlightInfoPartial> listFlights = new List<FlightInfoPartial>();

            using (var ctx = new FlightContext())
            {
                listFlights = (from f in ctx.Flights
                               where f.SourceId == flight.SourceId
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
                listFlights = (from f in ctx.Flights
                               where f.SourceId == flight.SourceId
                               select f).ToList<BE.FlightInfoPartial>();
                if (listFlights.Count != 0)
                {

                    ctx.Flights.Remove(listFlights.First());
                    ctx.SaveChanges();

                }
            }

        }

        public Location GetOriginAirport(BE.Root flight)
        {
            Location loc = new Location();
            loc.Latitude = flight.airport.origin.position.latitude;
            loc.Longitude = flight.airport.origin.position.longitude;

            return loc;
        }

        public BE.Trail GetCurrentPosition(List<BE.Trail> OrderedPlaces)
        {
            return OrderedPlaces.Last<BE.Trail>();
        }

        public Location GetBeforeLastPosition(List<BE.Trail> OrderedPlaces)
        {
            Location loc = new Location();
            int size = OrderedPlaces.Count();
            int BeforeLast = size - 2;
            loc.Latitude = OrderedPlaces[BeforeLast].lat;
            loc.Longitude = OrderedPlaces[BeforeLast].lng;
            return loc;
        }

        public Location GetPosition(BE.Root flight)
        {
            try
            {
                Location loc = new Location();
                loc.Latitude = flight.airport.destination.position.latitude;
                loc.Longitude = flight.airport.destination.position.longitude;
                return loc;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
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
        //public bool IsBeforeHolidayAsync1(string json)
        //{
        //    ObsHoliday.Add(json);

        //    foreach (var item in ObsHoliday)
        //    {
        //        //RootCal Data = JsonConvert.DeserializeObject<RootCal>(item);
        //        //if (Data.hd == 24 && Data.hm == "Kislev")
        //        //    return true;

        //        //foreach (var item1 in Data.events)
        //        //{
        //        //    if (item1.Contains("Erev"))
        //        //    {
        //        //        return true;
        //        //    }
        //        //}

        //    }
        //    //return false;
        //}
        public ObservableCollection<bool> GetObsHolidays()
        {
            return obsHolidays;
        }
        public async Task IsBeforeHolidayAsync1(DateTime date)
        {
            obsHolidays.Clear();
            DateTime dateIn6days = date.AddDays(6);
            for (DateTime i = date; i <= dateIn6days; i = i.AddDays(1))
            {
                bool b = await IsBeforeHolidayAsync(i);
                obsHolidays.Add(b);
            }

        }

        public Task<bool> IsBeforeHolidayAsync(DateTime date)
        {

            using (var webClient = new System.Net.WebClient())
            {

                var yyyy = date.ToString("yyyy");
                var mm = date.ToString("MM");
                var dd = date.ToString("dd");
                string URL = $"https://www.hebcal.com/converter?cfg=json&date={yyyy}-{mm}-{dd}&g2h=1&strict=1";
                var json = webClient.DownloadString(URL);
                RootCal Data = JsonConvert.DeserializeObject<RootCal>(json);
                if (Data.hd == 24 && Data.hm == "Kislev")
                    return Task.FromResult(true);

                foreach (var item in Data.events)
                {
                    if (item.Contains("Erev"))
                    {
                        return Task.FromResult(true);
                    }
                }
                return Task.FromResult(false);

            }

        }

        public RootWeather GetWeatherOfAirport(double latitude, double longitude)
        {

            using (var webClient = new System.Net.WebClient())
            {
                string key = "085845fad636a5a47a3f041f98275e2c";
                var lat = latitude.ToString();
                var lon = longitude.ToString();
                string URL = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={key}";
                var json = webClient.DownloadString(URL);
                RootWeather Data = JsonConvert.DeserializeObject<RootWeather>(json);
                return Data;

            }


        }

        

        
    }
}
