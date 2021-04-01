using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    public enum TrackDataSource
    {
        TRML, AADS
    }
    public class TrackData
    {
        public TrackDataSource Source { get; set; }
        public bool Faker { get; set; }
        public string Key { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public string Identification { get; set; }
        public PointLatLng Position { get; set; }
        public double Height { get; set; }
        public double Speed { get; set; }
        public double Bearing { get; set; }
        public DateTime LastUpdated { get; set; }
        public override string ToString()
        {
            return string.Format("[Position={0}, Speed={2}, Bearing={3}]", Position.ToString(), Height, Speed, Bearing);
        }
    }
}
