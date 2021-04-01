using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackEncoder
{
    class EncodedData
    {
        private int pointer = 0;
        private byte[] data = new byte[1024];
        public EncodedData()
        {
            
        }
        public void InitialRadar()
        {
            AppendData("0202001401");
        }
        public void InitialTrack()
        {
            AppendData("0202001202");
        }
        public void AppendData(params string[] hex)
        {
            foreach (string s in hex)
            {
                int k = 0;
                var output = s.ToLookup(c => Math.Floor((double)k++ / 2)).Select(e => new string(e.ToArray()));
                foreach (string ss in output)
                {
                    data[pointer] = Convert.ToByte(ss, 16);
                    pointer++;
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pointer; i++)
            {
                sb.AppendFormat("{0:x2}", data[i]);
            }
            return sb.ToString().ToUpper();
        }
    }
}
