using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class FlightInfoPartial : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string SourceId { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Source { get; set; }
        public string Destination; //{ get; set; }
        public string FlightCode { get; set; }

        public string destination
        {
            get
            {
                return this.Destination;
            }
            set
            {
                if (this.Destination != value)
                {
                    this.Destination = value;
                    this.NotifyPropertyChanged("destination");
                }
            }
        }
        public string source
        {
            get
            {
                return this.Source;
            }
            set
            {
                if (this.Source != value)
                {
                    this.Source = value;
                    this.NotifyPropertyChanged("source");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }
}
