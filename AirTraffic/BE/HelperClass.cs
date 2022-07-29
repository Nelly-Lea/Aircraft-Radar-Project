using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HelperClass
    {
        public HelperClass()
        {

        }
        //helper function to convert from unix epoch time Human DateTime

        public DateTime GetDateTimeFromEpoch(double EpochTimeStamp)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0); //from start epoch time
            start = start.AddSeconds(EpochTimeStamp);//add the seconds to thet start DateTime
            return start;
        }
    }
}
