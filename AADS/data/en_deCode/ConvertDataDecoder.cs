using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using NewRadarUX;

namespace Demo.WindowsForms.Forms
{
    class ConvertDataDecoder
    {
        public static string Convertcode(PointLatLng data, double bearing, double altitude, double speed, int id, char statu, DataDecoder decoder)
        {
            //track
            char status = statu;
            int posX;
            int posY;
            string radarkey = "";
            double min = 99999;
            
            Dictionary<string, RecievedRadarData> radars = decoder.Radars;
            foreach (string key in radars.Keys)
            {
                RecievedRadarData radar = radars[key];
                PointLatLng point = new PointLatLng(radar.Lat, radar.Lng);
                double distance = CPC.GetDistance(data, point);
                if (distance < min)
                {
                    min = distance;
                    radarkey = key;
                }
            }
            posY = Convert.ToInt16(((double)data.Lat - radars[radarkey].Lat) * 480);
            posX = Convert.ToInt16(((double)data.Lng - radars[radarkey].Lng) * 480);
            int speedX;
            int speedY;
            speedX = (int)Convert.ToInt16(speed * Math.Sin(bearing)) * 8;
            speedY = (int)Convert.ToInt16(speed * Math.Cos(bearing)) * 8;

            string codedata = "0201" + checkcode(posX) + checkcode(posY) + checkcode(speedX)
                + checkcode(speedY) + id.ToString("0000") + (int)status + radars[radarkey].Number.ToString("00");
            string blocklength = checkcode((codedata.Length / 2) + 4);
            return "0202" + blocklength + codedata.ToUpper();
        }

        public static string checkcode(int BlockLength)
        {
            string hex = Convert.ToString(BlockLength, 16);
            string expanded = "";
            for (int i = 0; i < 4 - hex.Length; i++)
            {
                expanded += "0";
            }
            expanded += hex;
            return (expanded);
        }
    }
}
