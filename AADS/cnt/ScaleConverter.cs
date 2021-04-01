using System;
using System.Collections.Generic;
using System.Text;

namespace NewRadarUX
{
    class ScaleConverter
    {
        public static double ConvertAltitude(double value, string scale, string returnedScale)
        {
            double ft = value;
            //convert to feet
            if (scale == "m")
            {
                ft = value / 0.3048;
            }
            else if (scale == "kft")
            {
                ft = value / 0.001;
            }
            else if (scale == "km")
            {
                ft = value / (0.3048 * 1000);
            }
            else if (scale == "FL")
            {
                ft = value * 100;
            }
            else if (scale == "NM")
            {
                ft = value * 0.00016457883;
            }
            //convert to expected scale
            if (returnedScale == "ft")
            {
                return ft;
            }
            else if (returnedScale == "m")
            {
                return ft * 0.3048;
            }
            else if (returnedScale == "kft")
            {
                return ft * 0.001;
            }
            else if (returnedScale == "km")
            {
                return (ft * 0.3048) / 1000;
            }
            else if (returnedScale == "FL")
            {
                return ft / 100;
            }
            else if(returnedScale == "NM")
            {
                return ft / 0.00016457883;
            }
            return 0;
        }
        public static double ConvertSpeed(double value, string scale, string returnedScale)
        {
            double kmph = value;
            //convert to kmph
            if (scale == "m/s")
            {
                kmph = value * 3.6;
            }
            else if (scale == "kts")
            {
                kmph = value / 0.539956803;
            }
            else if (scale == "mph")
            {
                kmph = value / 0.621371192;
            }
            //convert to expected scale
            if (returnedScale == "m/s")
            {
                return kmph / 3.6;
            }
            else if (returnedScale == "km/h")
            {
                return kmph;
            }
            else if (returnedScale == "kts")
            {
                return kmph * 0.539956803;
            }
            else if (returnedScale == "mph")
            {
                return kmph * 0.621371192;
            }
            return 0;
        }
        public static double ConvertBearing(double value, string scale, string returnedScale)
        {
            double deg = value;
            const double degToMilFactor = 0.05625;
            const double degToRadFactor = Math.PI / 180;
            //convert to degree
            if (scale == "rad")
            {
                deg = value / degToRadFactor;
            }
            else if (scale == "mil")
            {
                deg = value * degToMilFactor;
            }
            //convert to expected scale
            if (returnedScale == "rad")
            {
                return deg * degToRadFactor;
            }
            else if (returnedScale == "mil")
            {
                return deg / degToMilFactor;
            }
            else if (returnedScale == "deg")
            {
                return deg;
            }
            return 0;
        }
    }
}
