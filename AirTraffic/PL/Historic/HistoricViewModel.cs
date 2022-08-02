using PL.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PL.Historic
{
   
    public class HistoricViewModel
    {
        public HistoricModel historicModel { get; set; }
        public SearchHistoricCommand SHCommand { get; set; }
        public HistoricViewModel()
        {
           historicModel = new HistoricModel();
            SHCommand = new SearchHistoricCommand(this);


        }

     //   public ObservableCollection<BE.FlightInfoPartial> FlightBetweenTwoDates { get; set; }



        private ObservableCollection<BE.FlightInfoPartial> flightBetweenTwoDates;
        public ObservableCollection<BE.FlightInfoPartial> FlightBetweenTwoDates
        {
            get
            {
                if (flightBetweenTwoDates == null)
                    flightBetweenTwoDates = new ObservableCollection<BE.FlightInfoPartial>();
                return flightBetweenTwoDates;
            }

        }


        public void HVMDisplayFlightBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            ObservableCollection<BE.FlightInfoPartial> obs= new ObservableCollection<BE.FlightInfoPartial>();
            obs =historicModel.DisplayFlightBetweenTwoDates(dateFrom, dateTo);
            foreach(var item in obs)
            {
                FlightBetweenTwoDates.Add(item);
            }

        }

        public void HVMDeleteFlight(BE.FlightInfoPartial flight)
        {
            historicModel.HMDeleteFlight(flight);
            FlightBetweenTwoDates.Remove(flight);
        }

        public ICommand DeleteFlightCommand
        {
            get
            {
                return new DeleteFlightCommand(this);
            }
        }
        //public ICommand SearchHistoric
        //{
        //    get
        //    {
        //        return new SearchHistoricCommand(this);
        //    }
        //}
    }
}
