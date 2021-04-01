using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using NewRadarUX;
using System;
using System.Runtime.Serialization;
using Demo.WindowsForms.CustomMarkers;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AADS;

namespace Demo.WindowsForms.Forms
{
    public class RadarOverlay
    {
        public string Name;
        private GMapOverlay Overlay;
        public GMapMarker Marker;
        public double Radius;
        public Dictionary<double, GMapPolygon> Polygons;
        public List<GMapPolygon> Lines = new List<GMapPolygon>();
        private const double degToRadFactor = Math.PI / 180;
        public static Color DefaultLineColor = Color.CornflowerBlue;
        public static Color SpecialLineColor = Color.FromArgb(255, 126, 0);

        public RadarOverlay(string Name, GMapOverlay Overlay)
        {
            this.Name = Name;
            this.Overlay = Overlay;
        }

        public void UpdateLineColor()
        {
            MainForm main = MainForm.GetInstance();
            GMapControl map = main.GetmainMap();
            bool provider = map.NegativeMode;
            Color color = Color.Empty;
            if (provider == false)
            {
                color = Color.CornflowerBlue;
            }
            else
            {
                color = Color.Green;
            }
                foreach (GMapPolygon line in Lines)
            {
                line.Stroke = new Pen(color);
            }
            foreach (double radius in Polygons.Keys)
            {
                GMapPolygon polygon = Polygons[radius];
                polygon.Stroke = new Pen(color);
            }
        }

        public PointLatLng Position
        {
            get
            {
                return Marker.Position;
            }
            set
            {
                MoveRadar(value);
            }
        }
        public void InitialRadar(PointLatLng point, double radius, int interval)
        {
            Bitmap pic = (Bitmap) Image.FromFile("Img/point/RS.png");
            GMapMarker marker = new GMarkerGoogle(point, pic)
            {
                ToolTipText = this.Name,
                ToolTipMode = MarkerTooltipMode.Never,
                Tag = "RadarPinPoint"
            };
            marker.IsHitTestVisible = false;
            Overlay.Markers.Add(marker);
            this.Marker = marker;
            GMapPolygon poly = CreateRadius(radius);
            Dictionary<double, GMapPolygon> polys = new Dictionary<double, GMapPolygon>();
            polys.Add(radius, poly);

            int intervalTest = 0;
            if (DataSettings.EnableRadarInterval)
            {
                GMapPolygon Inradius1 = CreateRadius(DataSettings.RadarInterval["Radius1"]);
                polys.Add(DataSettings.RadarInterval["Radius1"], Inradius1);
                Overlay.Polygons.Add(Inradius1);
                GMapPolygon Inradius2 = CreateRadius(DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"]);
                polys.Add((double)DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"], Inradius2);
                Overlay.Polygons.Add(Inradius2);
                GMapPolygon Inradius3 = CreateRadius(DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"] + DataSettings.RadarInterval["Radius3"]);
                polys.Add((double)DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"] + DataSettings.RadarInterval["Radius3"], Inradius3);
                Overlay.Polygons.Add(Inradius3);
                GMapPolygon Inradius4 = CreateRadius(DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"] + DataSettings.RadarInterval["Radius3"] + DataSettings.RadarInterval["Radius4"]);
                polys.Add((double)DataSettings.RadarInterval["Radius1"] + DataSettings.RadarInterval["Radius2"] + DataSettings.RadarInterval["Radius3"] + DataSettings.RadarInterval["Radius4"], Inradius4);
                Overlay.Polygons.Add(Inradius4);
                intervalTest = (int)DataSettings.RadarInterval["Radius1"] + (int)DataSettings.RadarInterval["Radius2"] + (int)DataSettings.RadarInterval["Radius3"] + (int)DataSettings.RadarInterval["Radius4"] + interval;
            }
            for (int i = intervalTest; i < radius; i += interval)
            {
                GMapPolygon inside = CreateRadius(i);
                polys.Add(i, inside);
                Overlay.Polygons.Add(inside);
            }
            Overlay.Polygons.Add(poly);
            this.Polygons = polys;
            this.Radius = radius;
            UpdateLine(radius);
        }
        public void Clear()
        {
            ClearMarker();
            ClearLine();
            ClearRadius();
        }
        private void ClearMarker()
        {
            Overlay.Markers.Remove(Marker);
        }
        private void ClearRadius()
        {
            foreach (double radius in Polygons.Keys)
            {
                Overlay.Polygons.Remove(Polygons[radius]);
            }
            Polygons.Clear();
        }
        public void MoveRadar(PointLatLng point)
        {
            ClearMarker();
            Bitmap pic = (Bitmap)Image.FromFile("Img/point/RS.png");
            GMapMarker marker = new GMarkerGoogle(point, pic)
            {
                ToolTipText = this.Name,
                ToolTipMode = MarkerTooltipMode.Never,
                Tag = "RadarPinPoint"
            };
            Overlay.Markers.Add(marker);
            this.Marker = marker;
            Dictionary<Double, GMapPolygon> polylist = new Dictionary<Double, GMapPolygon>(this.Polygons);
            foreach (double index in polylist.Keys)
            {
                Overlay.Polygons.Remove(Polygons[index]);
                Polygons[index] = CreateRadius(index);
                Overlay.Polygons.Add(Polygons[index]);
            }
            ClearLine();
            UpdateLine(Radius);
            Marker.Position = point;
        }
        private void UpdateLine(double radius)
        {
            ClearLine();
            GMapPolygon vertical = CreateLine(radius, "vertical");
            GMapPolygon horizontal = CreateLine(radius, "horizontal");
            Overlay.Polygons.Add(vertical);
            Overlay.Polygons.Add(horizontal);
            Lines.Add(vertical);
            Lines.Add(horizontal);
        }
        private void ClearLine()
        {
            foreach (GMapPolygon line in Lines)
            {
                Overlay.Polygons.Remove(line);
            }
            Lines.Clear();
        }
        private GMapPolygon CreateLine(double radius, string action)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            PointLatLng point = new PointLatLng(Position.Lat, Position.Lng);
            Debug.WriteLine("center point=" + point);
            if (action == "vertical")
            {
                points.Add(FindPointAtDistanceFrom(point, 0, radius));
                points.Add(FindPointAtDistanceFrom(point, 180, radius));
                Debug.WriteLine("vertical points=" + points[0] + ", " + points[1]);
            }
            else if (action == "horizontal")
            {
                PointLatLng p = FindPointAtDistanceFrom(point, 90, radius);
                p.Lat = point.Lat;
                points.Add(p);
                p = FindPointAtDistanceFrom(point, 270, radius);
                p.Lat = point.Lat;
                points.Add(p);
                //points.Add(FindPointAtDistanceFrom(point, 90, radius));
                //points.Add(FindPointAtDistanceFrom(point, 270, radius));
            }
            GMapPolygon line = new GMapPolygon(points, "line");
            line.Stroke = new Pen(DefaultLineColor, 1);
            return line;
        }
        private GMapPolygon CreateRadius(double radius)
        {
            PointLatLng point = new PointLatLng(Position.Lat, Position.Lng);
            int segments = 1080;
            List<PointLatLng> gpollist = new List<PointLatLng>();
            for (int i = 0; i < segments; i++)
            {
                gpollist.Add(FindPointAtDistanceFrom(point, i, radius));
            }
            GMapPolygon gpol = new GMapPolygon(gpollist, "circ");
            gpol.Fill = new SolidBrush(Color.Transparent);
            gpol.Stroke = new Pen(Color.CornflowerBlue, 1);
            gpol.IsHitTestVisible = true;
            return gpol;
        }
        private PointLatLng FindPointAtDistanceFrom(GMap.NET.PointLatLng startPoint, double initialBearingRadians, double distanceKilometres)
        {
            double radius = 6371.01;

            var δ = distanceKilometres / radius; // angular distance in radians
            var θ = DegreesToRadians(initialBearingRadians);

            var φ1 = DegreesToRadians(startPoint.Lat);
            var λ1 = DegreesToRadians(startPoint.Lng);

            var sinφ2 = Math.Sin(φ1) * Math.Cos(δ) + Math.Cos(φ1) * Math.Sin(δ) * Math.Cos(θ);
            var φ2 = Math.Asin(sinφ2);
            var y = Math.Sin(θ) * Math.Sin(δ) * Math.Cos(φ1);
            var x = Math.Cos(δ) - Math.Sin(φ1) * sinφ2;
            var λ2 = λ1 + Math.Atan2(y, x);

            var lat = RadiansToDegrees(φ2);
            var lon = RadiansToDegrees(λ2);

            return new PointLatLng(lat, lon);
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
