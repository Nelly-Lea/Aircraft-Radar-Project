using PL.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PL.Historic
{

    public class HistoricViewModel:INotifyPropertyChanged
    {
        public HistoricModel historicModel { get; set; }
        public SearchHistoricCommand SHCommand { get; set; }
        public HolidaysCommand HCommand{get;set;}
        public event PropertyChangedEventHandler PropertyChanged;

        public HistoricViewModel()
        {
           historicModel = new HistoricModel();
            SHCommand = new SearchHistoricCommand(this);
            HCommand = new HolidaysCommand(this);

        }

        //   public ObservableCollection<BE.FlightInfoPartial> FlightBetweenTwoDates { get; set; }

        public void RaisePropertyChanged(string Name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }


        private ObservableCollection<BE.FlightInfoPartial> flightBetweenTwoDates;

        

        public ObservableCollection<BE.FlightInfoPartial> FlightBetweenTwoDates
        {
            get
            {
                if (flightBetweenTwoDates == null)
                    flightBetweenTwoDates = new ObservableCollection<BE.FlightInfoPartial>();
                return flightBetweenTwoDates;
            }
            set
            {
                flightBetweenTwoDates = value;
                RaisePropertyChanged("FlightBetweenTwoDates");
            }

        }
        private string holidaysMessage { get; set; }
        public void OnPropertyChanged(string Name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        public string HolidaysMessage
        {
            get
            {
                return holidaysMessage;
            }
            set
            {
                holidaysMessage = value;
                OnPropertyChanged("HolidaysMessage");
           }
        }
        
        public void HVMDisplayHolidayMessage(DateTime from, DateTime To)
        {
             bool FromBool = historicModel.HMDisplayHolidaysMessage(from);
            bool ToBool = historicModel.HMDisplayHolidaysMessage(To);
            
           if (FromBool && ToBool)
            {
                HolidaysMessage = "התאריכים בשבוע לפני חג";
            
                return;
            }
            if (FromBool)
            {
                HolidaysMessage = "התאריך הראשון בשבוע לפני חג";
                
                return;
            }
            if (ToBool)
            {
                HolidaysMessage = "התאריך השני בשבוע חג";
                
                return;
            }
            if(!FromBool && !ToBool)
            {
                HolidaysMessage = "אין חג קרוב לתאריכים שבחרת";
               
                return;
            }
            
        }

       

        public void HVMDisplayFlightBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            ObservableCollection<BE.FlightInfoPartial> obs= new ObservableCollection<BE.FlightInfoPartial>();
            obs =historicModel.DisplayFlightBetweenTwoDates(dateFrom, dateTo);
            flightBetweenTwoDates.Clear();
            FlightBetweenTwoDates.Clear();
            
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
