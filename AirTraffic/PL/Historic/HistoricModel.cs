using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Historic
{
   
    public class HistoricModel
    {
        IBL BL;
        public HistoricModel()
        {
            BL = new BLImp();
        }
    }
}
