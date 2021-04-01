using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewRadarUX
{
    public class FlightRadarData
    {
        //main data
        public int Id;
        public char identification;
        public PointLatLng point;
        public double bearing; //deg
        public double speed; //kts
        public double altitude; //ft
        /*
        public DateTime startTime;
        public DateTime lastTime;
        public int initialType;
        public int aircraftType;
        public double cumulativeRange;
        public double averageDirection;
        public double maxSpeed;
        public double maxAltitude;
        public int transactionId;
        */
        public char identity;

        //simulate data
        public PointLatLng lastPoint; //ใช้สำหรับ simulation
        public DateTime? time;

        //function data
        public bool auto;

        public FlightRadarData(TrackType type)
        {
            this.identity = type == TrackType.Real ? 'A' : 'X';
        }
        public string name
        {
            get
            {
                Track track = Track.GetInstance();
                return track.getStatus(Id) + " " + Id.ToString("000") + identity;
            }
        }

        public FlightRadarData(FlightRadarData oldfd, PointLatLng point, DateTime time)
        {
            //copy from old data
            this.Id = oldfd.Id;
            this.lastPoint = oldfd.point;
            this.bearing = oldfd.bearing;
            this.altitude = oldfd.altitude;
            this.speed = oldfd.speed;
            this.identification = oldfd.identification;
            /*
            this.startTime = oldfd.startTime;
            this.lastTime = oldfd.lastTime;
            this.initialType = oldfd.initialType;
            this.aircraftType = oldfd.aircraftType;
            this.cumulativeRange = oldfd.cumulativeRange;
            this.averageDirection = oldfd.averageDirection;
            this.maxSpeed = oldfd.maxSpeed;
            this.maxAltitude = oldfd.maxAltitude;
            this.transactionId = oldfd.transactionId;
            */
            //function data
            this.auto = oldfd.auto;
            //set new data
            this.point = point;
            this.time = time;
        }
    }
}
