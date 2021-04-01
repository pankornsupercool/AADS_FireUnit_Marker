using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    enum IntersectionResult
    {
        Success, Impossible, Infinity, Ambiguous, Empty
    }
    class CPC
    {
        public static float Rotate(float bearing, FlightRadarData fd1, FlightRadarData fd2)
        {
            bearing = (bearing + 1) % 360;
            return bearing;
        }
        public static IntersectionResult Intersection(FlightRadarData fd1, FlightRadarData fd2, out PointLatLng intersectPoint, out double s, out double b, out double t)
        {
            IntersectionResult result = IntersectionResult.Empty;
            PointLatLng point1 = fd1.point;
            PointLatLng point2 = fd2.point;
            double initDistance = GetDistance(point1, point2);
            float bearing = (float)fd1.bearing;
            intersectPoint = new PointLatLng();
            int attempt = 0;
            s = 0;
            b = 0;
            t = 0;
            while (result != IntersectionResult.Success && attempt < 360)
            {
                var φ1 = DegreesToRadians(point1.Lat);
                var φ2 = DegreesToRadians(point2.Lat);
                var λ1 = DegreesToRadians(point1.Lng);
                var λ2 = DegreesToRadians(point2.Lng);
                var Δφ = φ2 - φ1;
                var Δλ = λ2 - λ1;
                var θ13 = DegreesToRadians(bearing);
                var θ23 = DegreesToRadians(fd2.bearing);

                var δ12 = 2 * Math.Asin(Math.Sqrt(Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) + Math.Cos(φ1) * Math.Cos(φ2) * Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2)));

                var cosθa = (Math.Sin(φ2) - Math.Sin(φ1) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ1));
                var cosθb = (Math.Sin(φ1) - Math.Sin(φ2) * Math.Cos(δ12)) / (Math.Sin(δ12) * Math.Cos(φ2));
                var θa = Math.Acos(Math.Min(Math.Max(cosθa, -1), 1)); // protect against rounding errors
                var θb = Math.Acos(Math.Min(Math.Max(cosθb, -1), 1)); // protect against rounding errors

                var θ12 = Math.Sin(λ2 - λ1) > 0 ? θa : 2 * Math.PI - θa;
                var θ21 = Math.Sin(λ2 - λ1) > 0 ? 2 * Math.PI - θb : θb;

                var α1 = θ13 - θ12; // angle 2-1-3
                var α2 = θ21 - θ23; // angle 1-2-3

                if (Math.Sin(α1) == 0 && Math.Sin(α2) == 0)
                {
                    bearing = Rotate(bearing, fd1, fd2);
                    result = IntersectionResult.Infinity;
                }
                else if (Math.Sin(α1) * Math.Sin(α2) < 0)
                {
                    bearing = Rotate(bearing, fd1, fd2);
                    result = IntersectionResult.Ambiguous;
                }
                else
                {
                    var cosα3 = -Math.Cos(α1) * Math.Cos(α2) + Math.Sin(α1) * Math.Sin(α2) * Math.Cos(δ12);

                    var δ13 = Math.Atan2(Math.Sin(δ12) * Math.Sin(α1) * Math.Sin(α2), Math.Cos(α2) + Math.Cos(α1) * cosα3);

                    var φ3 = Math.Asin(Math.Sin(φ1) * Math.Cos(δ13) + Math.Cos(φ1) * Math.Sin(δ13) * Math.Cos(θ13));

                    var Δλ13 = Math.Atan2(Math.Sin(θ13) * Math.Sin(δ13) * Math.Cos(φ1), Math.Cos(δ13) - Math.Sin(φ1) * Math.Sin(φ3));
                    var λ3 = λ1 + Δλ13;

                    var lat = RadiansToDegrees(φ3);
                    var lng = RadiansToDegrees(λ3);
                    intersectPoint = new PointLatLng(lat, lng);

                    var speed2InKm = ScaleConverter.ConvertSpeed(fd2.speed, "kts", "km/h");
                    var distance2 = GetDistance(fd2.point, intersectPoint);
                    var time = distance2 / speed2InKm;

                    var distance1 = GetDistance(fd1.point, intersectPoint);
                    var speed1 = distance1 / time;
                    if (distance1 > initDistance * 2.5)
                    {
                        bearing = Rotate(bearing, fd1, fd2);
                        result = IntersectionResult.Ambiguous;
                    }
                    else if (speed1 > DataSettings.Track["MaxSpeed"])
                    {
                        bearing = Rotate(bearing, fd1, fd2);
                        attempt++;
                        result = IntersectionResult.Impossible;
                    }
                    else
                    {
                        s = speed1;
                        b = bearing;
                        t = time;
                        result = IntersectionResult.Success;
                    }
                }
            }
            return result;
        }

        public static double DegRecommend(PointLatLng PointFighters,PointLatLng PointRecommend)
        {
            return rhumbBearingTo(PointFighters, PointRecommend);
        }

        public static double FindDegFighters(double DegFighters, double DegRecommend)
        {
            var Deg = 0.0;
            if(DegFighters > DegRecommend)
            {
                Deg = DegFighters - DegRecommend;
                Deg = Deg - 360;
            }
            else if(DegFighters < DegRecommend)
            {
                Deg = DegRecommend - DegFighters;
            }
            
            return Deg;
        }

        public static List<double> findSpeedAndTime(double distanceFighters, double SpeedTarget, double distanceTarget)
        {
            List<double> result = new List<double>();
            double V_Fighters, T_Fighters, T_Target;

            T_Target = distanceTarget/ SpeedTarget;
            V_Fighters = distanceFighters/ T_Target;

            T_Fighters = distanceFighters/ V_Fighters;

            result[0] = V_Fighters;
            result[1] = T_Fighters;

            return result;
        }

        public static string HourToMinutesAndSeconds(double TimeHour)
        {
            int min, sec;
            double minDou;

            min = (int)(TimeHour * (double)60);
            minDou = TimeHour * 60.0;
            sec = (int)(60 * ((double)minDou - min));

            return min.ToString() + ":" + sec.ToString();
        }

        public static double GetDistance(PointLatLng p1, PointLatLng p2)
        {
            var R = 6371d; // Radius of the earth in km
            var dLat = DegreesToRadians(p2.Lat - p1.Lat);  // deg2rad below
            var dLon = DegreesToRadians(p2.Lng - p1.Lng);
            var a =
            Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) +
            Math.Cos(DegreesToRadians(p1.Lat)) * Math.Cos(DegreesToRadians(p2.Lat)) *
            Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d);
            var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
            var d = R * c; // Distance in km
            return d;
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

        public static PointLatLng destinationPoint(PointLatLng point,double bearing, double distance)
        {
            double radius = 6371e3;

            var δ = distance / radius; // angular distance in radians
            var θ = DegreesToRadians(bearing);

            var φ1 = DegreesToRadians(point.Lat);
            var λ1 = DegreesToRadians(point.Lng);

            var sinφ2 = Math.Sin(φ1) * Math.Cos(δ) + Math.Cos(φ1) * Math.Sin(δ) * Math.Cos(θ);
            var φ2 = Math.Asin(sinφ2);
            var y = Math.Sin(θ) * Math.Sin(δ) * Math.Cos(φ1);
            var x = Math.Cos(δ) - Math.Sin(φ1) * sinφ2;
            var λ2 = λ1 + Math.Atan2(y, x);

            var lat = RadiansToDegrees(φ2);
            var lon = RadiansToDegrees(λ2);

            return new PointLatLng(lat,lon);
        }

        public static double rhumbBearingTo(PointLatLng point1,PointLatLng point2)
        {
            var φ1 = DegreesToRadians(point1.Lat);
            var φ2 = DegreesToRadians(point2.Lat);
            var Δλ = DegreesToRadians(point2.Lng - point1.Lng);

            // if dLon over 180° take shorter rhumb line across the anti-meridian:
            if (Math.Abs(Δλ) > Math.PI) Δλ = Δλ > 0 ? -(2 * Math.PI - Δλ) : (2 * Math.PI + Δλ);

            var Δψ = Math.Log(Math.Tan(φ2 / 2 + Math.PI / 4) / Math.Tan(φ1 / 2 + Math.PI / 4));

            var θ = Math.Atan2(Δλ, Δψ);

            var bearing = RadiansToDegrees(θ);

            return clampAngle(bearing);
        }

        public static PointLatLng midpoint(PointLatLng start, PointLatLng end)
        {
            var φ1 = DegreesToRadians(start.Lat);
            var λ1 = DegreesToRadians(start.Lng);
            var φ2 = DegreesToRadians(end.Lat);
            var Δλ = DegreesToRadians(end.Lng - start.Lng);

            var Bx = Math.Cos(φ2) * Math.Cos(Δλ);
            var By = Math.Cos(φ2) * Math.Sin(Δλ);
            var φ3 = Math.Atan2(Math.Sin(φ1) + Math.Sin(φ2),
                                Math.Sqrt((Math.Cos(φ1) + Bx) * (Math.Cos(φ1) + Bx) + By * By));
            var λ3 = λ1 + Math.Atan2(By, Math.Cos(φ1) + Bx);
            PointLatLng point = new PointLatLng(RadiansToDegrees(φ3), RadiansToDegrees(λ3));
            return point;
        }

        public static float initialBearingTo(PointLatLng start, PointLatLng end)
        {
            var φ1 = DegreesToRadians(start.Lat);
            var φ2 = DegreesToRadians(end.Lat);
            var Δλ = DegreesToRadians(end.Lng - start.Lng);

            var x = Math.Cos(φ1) * Math.Sin(φ2) - Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(Δλ);
            var y = Math.Sin(Δλ) * Math.Cos(φ2);
            var θ = Math.Atan2(y, x);

            var bearing = (float)RadiansToDegrees(θ);

            return wrap360(bearing);
        }

        public static float finalBearingTo(PointLatLng start, PointLatLng end)
        {

            // get initial bearing from destination point to this point & reverse it by adding 180°

            var bearing = (float)initialBearingTo(start,end) + 180;

            return wrap360(bearing);
        }

        public static float wrap360(float degrees)
        {
            if (0 <= degrees && degrees < 360) return degrees; // avoid rounding due to arithmetic ops if within range
            return (degrees % 360 + 360) % 360; // sawtooth wave p:360, a:360
        }

        public static double clampAngle(double angle)
        {
            return (angle % 360) + (angle < 0 ? 360 : 0);
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
