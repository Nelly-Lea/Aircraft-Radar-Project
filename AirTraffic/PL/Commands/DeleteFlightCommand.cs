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
    class DeleteFlightCommand:ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter) // pas oublie de verifier que date apres
        {
            return true;
            
        }

        public void Execute(object parameter)
        {
            currentVM.HVMDeleteFlight(parameter as BE.FlightInfoPartial);
        }



        public HistoricViewModel currentVM { get; set; }

        public DeleteFlightCommand(HistoricViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }
    }
}

