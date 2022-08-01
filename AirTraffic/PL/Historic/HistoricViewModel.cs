using PL.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PL.Historic
{
   
    public class HistoricViewModel
    {
        public HistoricModel historicModel { get; set; }
        public HistoricViewModel()
        {
           historicModel = new HistoricModel();


        }
        public ICommand SearchHistoric
        {
            get
            {
                return new SearchHistoric(this);
            }
        }
    }
}
