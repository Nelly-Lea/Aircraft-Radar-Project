using AirTraffic.Radar;
using PL.Historic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PL.Commands
{
    public class HolidaysCommand : ICommand
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


            ObservableCollection<string> ObsDate = new ObservableCollection<string>();
            DateTime DTFrom = new DateTime();
            ObsDate = parameter as ObservableCollection<string>;
            DTFrom = DateTime.Parse(ObsDate[0]);
            DateTime DTTo = new DateTime();
            DTTo = DateTime.Parse(ObsDate[1]);
            currentVM.HVMDisplayHolidayMessage(DTFrom, DTTo);
        }
        public HistoricViewModel currentVM { get; set; }

        public HolidaysCommand(HistoricViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }

    }
}
