using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Holidays
{
    public class HolidaysModel
    {

        IBL BL;
        public HolidaysModel()
        {
            BL = new BLImp();
        }

    }
}
