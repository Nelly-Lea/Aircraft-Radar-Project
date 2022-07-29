using AirTraffic.Radar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirTraffic.Commands
{
    class ReadAllCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            currentVM.DisplayCurrentFilghts();



        }
        public RadarViewModel currentVM { get; set; }

        public ReadAllCommand(RadarViewModel CurrentVM)
        {
            currentVM = CurrentVM;
        }
    }
}
