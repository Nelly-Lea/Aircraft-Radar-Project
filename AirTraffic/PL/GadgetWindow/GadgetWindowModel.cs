using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.GadgetWindow
{
    public class GadgetWindowModel
    {

        IBL BL;
        public GadgetWindowModel()
        {
            BL = new BLImp();
        }

        public List<BE.Root> GMGetAllFlightsRoot()
        {
            return BL.GetAllFlightRoot();
        }

        public BE.Root GMgetRootFromFlightCode(string flightCode)
        {
            return BL.getRootFromFlightCode(flightCode);
        }
    }
}
