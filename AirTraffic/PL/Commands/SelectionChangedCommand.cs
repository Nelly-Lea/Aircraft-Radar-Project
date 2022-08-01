using AirTraffic.Radar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PL.Commands
{
    class SelectionChangedCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { { CommandManager.RequerySuggested -= value; } }
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            BE.FlightInfoPartial SelectedFlight = new BE.FlightInfoPartial();
            SelectedFlight = parameter as BE.FlightInfoPartial;
            string key = SelectedFlight.SourceId;
            currentVM.DisplayFlightData(key);
            currentVM.SaveFlightToDB(SelectedFlight);
        }
        public RadarViewModel currentVM { get; set; }

        public SelectionChangedCommand(RadarViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }

    }
}
