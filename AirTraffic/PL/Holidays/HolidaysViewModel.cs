using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.Holidays
{
    public class HolidaysViewModel
    {
        public HolidaysModel holidaysModel { get; set; }

        public HolidaysViewModel()
        {
            holidaysModel = new HolidaysModel();
        }
    }
}
