using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackEncoder
{
    class Radar
    {
        private PointLatLng _Position;
        public Radar(int ID, PointLatLng Position)
        {
            this.ID = ID;
            this._Position = Position;
        }
        public int ID { get; set; }
        public PointLatLng Position
        {
            get { return _Position; }
            set
            {
                _Position.Lat = value.Lat;
                _Position.Lng = value.Lng;
            }
        }
    }
}
