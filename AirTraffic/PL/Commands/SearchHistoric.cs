using PL.Historic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PL.Commands
{
    public class SearchHistoric : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public HistoricViewModel currentVM { get; set; }

        public SearchHistoric(HistoricViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }
    }
}
