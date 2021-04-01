using CoordinateSharp;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGeographicLib;

namespace Demo.WindowsForms.Forms
{
    class PositionConverter
    {
        public static string DefaultScale = "Signed Degree";
        public static PointLatLng ParsePointFromString(string pos)
        {
            PointLatLng point = new PointLatLng(0, 0);
            Coordinate c;
            double lat, lon;
            int prec;
            if (Coordinate.TryParse(pos, out c))
            {
                point.Lat = c.Latitude.ToDouble();
                point.Lng = c.Longitude.ToDouble();
            }
            else
            {
                string[] arr = pos.Split(',');
                try
                {
                    Georef.Reverse(pos, out lat, out lon, out prec, true);
                    point.Lat = lat;
                    point.Lng = lon;
                }
                catch
                {
                    point.Lat = Convert.ToDouble(arr[0]);
                    point.Lng = Convert.ToDouble(arr[1]);
                }
            }
            return point;
        }
        public static string georef;
        public static string ParsePointToString(PointLatLng point, string scale)
        {
            Coordinate c = new Coordinate(point.Lat, point.Lng);
            if (scale == "Signed Degree")
            {
                return point.Lat.ToString(".000000") + ", " + point.Lng.ToString(".000000");
            }
            else if (scale == "Decimal Degree" || scale == "Lat/Lon d°")
            {
                c.FormatOptions.Format = CoordinateFormatType.Decimal_Degree;
                c.FormatOptions.Display_Leading_Zeros = true;
                c.FormatOptions.Round = 3;
                return c.ToString();
            }
            else if (scale == "Degree Decimal Minute")
            {
                c.FormatOptions.Format = CoordinateFormatType.Degree_Decimal_Minutes;
                c.FormatOptions.Display_Leading_Zeros = true;
                c.FormatOptions.Round = 3;
                return c.ToString();
            }
            else if (scale == "Degree Minute Seconds" || scale == "Lat/Lon dms")
            {
                c.FormatOptions.Format = CoordinateFormatType.Degree_Minutes_Seconds;
                c.FormatOptions.Display_Leading_Zeros = true;
                c.FormatOptions.Round = 3;
                return c.ToString();
            }
            else if (scale == "UTM")
            {
                return c.UTM.ToString();
            }
            else if (scale == "MGRS")
            {
                return c.MGRS.ToString();
            }
            else if (scale == "Cartesian")
            {
                return c.Cartesian.ToString();
            }
            else if (scale == "ECEF")
            {
                return c.ECEF.ToString();
            }
            else if (scale == "GEOREF")
            {
                for (int prec = -1; prec <= 5; ++prec)
                {
                    Georef.Forward(point.Lat,point.Lng, prec, out georef);
                    Console.WriteLine(string.Format("Precision: {0} Georef: {1}", prec, georef));
                }
                //GeoCoordinate s = new GeoCoordinate(point.Lat,point.Lng);
                return georef;//s.ToString();
            }
            return null;
        }
    }
}
