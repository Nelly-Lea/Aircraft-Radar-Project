using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.GadgetWindow
{
    public class GadgetWindowViewModel:INotifyPropertyChanged
    {
        public GadgetWindowModel gadgetWindowModel { get; set; }

        public GadgetWindowViewModel()
        {
            gadgetWindowModel = new GadgetWindowModel();

        }

        public void RaisePropertyChanged(string Name)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string v)
        {
            //throw new NotImplementedException();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        //public List<BE.Root> GVMGetAllFlightsRoot()
        //{
        //    return gadgetWindowModel.GMGetAllFlightsRoot();
        //}

        public BE.Root GVMgetRootFromFlightCode(string flightCode)
        {
            return gadgetWindowModel.GMgetRootFromFlightCode(flightCode);
        }
    }
}
