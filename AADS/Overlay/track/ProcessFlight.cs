using Demo.WindowsForms;
using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    class ProcessFlight
    {
        public static Dictionary<int, GMapMarkerPlane> AllflightMarkers = new Dictionary<int, GMapMarkerPlane>();
        public static Dictionary<int, GMapMarker> AllBordersTrack = new Dictionary<int, GMapMarker>();
        //public static List<FlightRadarData> flightSimulation = new List<FlightRadarData>();
        public static Dictionary<int, GMapMarkerPlane> AllflightFakers = new Dictionary<int, GMapMarkerPlane>();
        public static Dictionary<int, GMapMarkerPlane> CloneflightFakers()
        {
            Dictionary<int, GMapMarkerPlane> temp = new Dictionary<int, GMapMarkerPlane>(AllflightFakers);
            return temp;
        }
        public static Dictionary<int, GMapMarkerPlane> CloneflightAll()
        {
            Dictionary<int, GMapMarkerPlane> temp = new Dictionary<int, GMapMarkerPlane>(AllflightMarkers);
            return temp;
        }
        public static void testDoWork()
        {
            /*
            for (int i = 0; i < flightSimulation.Count; i++)
            {
                FlightRadarData fd = flightSimulation[i];
                if (fd.auto)
                {
                    DateTime now = DateTime.Now;
                    double intervalSecond = (fd.time != null) ? now.Subtract(fd.time.Value).TotalMilliseconds / 1000 : 0;
                    double distance = getDistanceFrom(ScaleConverter.ConvertSpeed(fd.speed, "kts", "km/h"), intervalSecond);
                    PointLatLng p = FindPointAtDistanceFrom(fd.point, DegreesToRadians(fd.bearing), distance);
                    FlightRadarData tempfd = new FlightRadarData(fd, p, now);
                    flightSimulation[i] = tempfd;
                }
            }
            */
            Track track = Track.GetInstance();
            foreach (int faker in track.Fakers)
            {
                FlightRadarData fd = track.GetFaker(faker);
                if (fd.auto)
                {
                    DateTime now = DateTime.Now;
                    double intervalSecond = (fd.time != null) ? now.Subtract(fd.time.Value).TotalMilliseconds / 1000 : 0;
                    double distance = getDistanceFrom(ScaleConverter.ConvertSpeed(fd.speed, "kts", "km/h"), intervalSecond);
                    PointLatLng p = FindPointAtDistanceFrom(fd.point, DegreesToRadians(fd.bearing), distance);
                    fd.lastPoint = fd.point;
                    fd.point = p;
                    fd.time = now;
                    if (DataSettings.UseDatabase)
                    {
                        MySqlConnection conn = DataSettings.GetConnection();
                        using (MySqlCommand cmd = new MySqlCommand(null, conn))
                        {
                            cmd.CommandText = "INSERT INTO faker_track_point (point_lat, point_lng, faker_id) VALUES (@lat, @lng, @id)";
                            cmd.Parameters.AddWithValue("lat", fd.lastPoint.Lat);
                            cmd.Parameters.AddWithValue("lng", fd.lastPoint.Lng);
                            cmd.Parameters.AddWithValue("id", fd.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static double getDistanceFrom(double speedKMperHour, double timeisSecond)
        {
            double timeHour = timeisSecond / (60 * 60.0);
            Debug.WriteLine("time in second: " + timeHour);
            return speedKMperHour * timeHour;
        }

        public static PointLatLng FindPointAtDistanceFrom(PointLatLng startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var distRatio = distanceKilometres / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint.Lat);
            var startLonRad = DegreesToRadians(startPoint.Lng);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));

            var endLonRads = startLonRad
                + Math.Atan2(
                    Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                    distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new PointLatLng
            {
                Lat = RadiansToDegrees(endLatRads),
                Lng = RadiansToDegrees(endLonRads)
            };
        }

        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}
