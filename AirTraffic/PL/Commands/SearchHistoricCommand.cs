using PL.Historic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PL.Commands
{
    public class SearchHistoricCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter) // pas oublie de verifier que date apres
        {
            ObservableCollection<string> ObsDate = new ObservableCollection<string>();
            DateTime DTFrom = new DateTime();
            ObsDate = parameter as ObservableCollection<string>;
            DTFrom = DateTime.Parse(ObsDate[0]);
            DateTime DTTo = new DateTime();
            DTTo = DateTime.Parse(ObsDate[1]);
            if (DTTo.Date < DTFrom.Date)
                return false;
            else
                return true;

        }

        public void Execute(object parameter)
        {
            //  throw new NotImplementedException();
            ObservableCollection<string> ObsDate = new ObservableCollection<string>();
            DateTime DTFrom = new DateTime();
            ObsDate = parameter as ObservableCollection<string>;
            DTFrom = DateTime.Parse(ObsDate[0]);
            DateTime DTTo = new DateTime();
            DTTo = DateTime.Parse(ObsDate[1]);
            currentVM.HVMDisplayFlightBetweenTwoDates(DTFrom, DTTo);
        }
    

    
        public HistoricViewModel currentVM { get; set; }

        public SearchHistoricCommand(HistoricViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }
    }
}
