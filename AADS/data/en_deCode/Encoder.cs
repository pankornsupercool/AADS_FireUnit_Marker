using NewRadarUX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackEncoder
{
    class Encoder
    {
        public Encoder()
        {

        }
        private int[] ddToDms(double dd)
        {
            int[] dms = new int[3];
            int d = (int)dd;
            int m = (int)((dd - d) * 60);
            int s = (int)((dd - d - (double)m / 60) * 3600);
            dms[0] = d;
            dms[1] = m;
            dms[2] = s;
            return dms;
        }
        private string dmsToHex(int[] dms)
        {
            StringBuilder sb = new StringBuilder();
            string dbin = Convert.ToString(dms[0], 2).PadLeft(16, '0');
            string mbin = Convert.ToString(dms[1], 2).PadLeft(6, '0');
            string sbin = Convert.ToString(dms[2], 2).PadLeft(6, '0');
            sb.Append(mbin);
            sb.Append(sbin);
            sb.Append("0000");
            sb.Append(dbin);
            int val = Convert.ToInt32(sb.ToString(), 2);
            return Convert.ToString(val, 16);
        }

        public string encodeRadar(Radar radar)
        {
            EncodedData data = new EncodedData();

            //Radar encoding
            data.InitialRadar();
            data.AppendData("0000000000");
            data.AppendData(Convert.ToString(radar.ID, 16), "00");
            data.AppendData(dmsToHex(ddToDms(radar.Position.Lat)));
            data.AppendData(dmsToHex(ddToDms(radar.Position.Lng)));
            return data.ToString();
        }
        public string headTrack(int length)
        {
            return "0202"+Convert.ToString((length/2)+4,16).PadLeft(4,'0');
        }

        public string Encode(FlightRadarData fd, Radar radar,int cnt)
        {
            EncodedData data = new EncodedData();

            //Radar encoding
            /*data.InitialRadar();
            data.AppendData("0000000000");
            data.AppendData(Convert.ToString(radar.ID, 16), "00");
            data.AppendData(dmsToHex(ddToDms(radar.Position.Lat)));
            data.AppendData(dmsToHex(ddToDms(radar.Position.Lng)));*/
            
            data.AppendData(cnt.ToString());
            short positionX = (short)((fd.point.Lng - radar.Position.Lng) * 480);
            short positionY = (short)((fd.point.Lat - radar.Position.Lat) * 480);
            double speed = fd.speed;
            double bearing = fd.bearing;
            short speedX = (short)(speed * Math.Sin(bearing * Math.PI / 180));
            short speedY = (short)(speed * Math.Cos(bearing * Math.PI / 180));
            data.AppendData(Convert.ToString(positionX, 16).PadLeft(4, '0'));
            data.AppendData(Convert.ToString(positionY, 16).PadLeft(4, '0'));
            data.AppendData(Convert.ToString(speedX, 16).PadLeft(4, '0'));
            data.AppendData(Convert.ToString(speedY, 16).PadLeft(4, '0'));
            data.AppendData(Convert.ToString(fd.Id, 16).PadLeft(4, '0'));
            data.AppendData(Convert.ToString(fd.identification, 16).PadLeft(2, '0'));
            data.AppendData(Convert.ToString(radar.ID, 16));
            return "02"+data.ToString();
        }
    }
}
