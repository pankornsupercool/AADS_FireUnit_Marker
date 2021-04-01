using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using AADS;
using MySql.Data.MySqlClient;
using Net_GmapMarkerWithLabel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    public class MPWrapper
    {
        private Dictionary<int, SymbolInfo> _Markers;
        private Dictionary<int, AreaInfo> _Polygons;
        public MPWrapper(Dictionary<int, SymbolInfo> Markers, Dictionary<int, AreaInfo> Polygons)
        {
            _Markers = new Dictionary<int, SymbolInfo>();
            _Polygons = new Dictionary<int, AreaInfo>();
            foreach (int id in Markers.Keys)
            {
                _Markers.Add(id, (SymbolInfo) Markers[id].Clone());
            }
            foreach (int id in Polygons.Keys)
            {
                _Polygons.Add(id, (AreaInfo) Polygons[id].Clone());
            }
        }
        public Dictionary<int, SymbolInfo> Markers
        {
            get
            {
                return _Markers;
            }
        }
        public Dictionary<int, AreaInfo> Polygons
        {
            get
            {
                return _Polygons;
            }
        }
    }
    class MarkerAndPolygon
    {
        private MainForm Main = MainForm.GetInstance();
        public static MarkerAndPolygon Instance;
        //Marker and Polygon data
        public Dictionary<int, SymbolInfo> Markers = new Dictionary<int, SymbolInfo>();
        public Dictionary<int, AreaInfo> Polygons = new Dictionary<int, AreaInfo>();
        public static MarkerAndPolygon GetInstance()
        {
            if (Instance == null)
            {
                Instance = new MarkerAndPolygon();
                if (DataSettings.UseDatabase)
                {
                    Instance.LoadAll();
                }
            }
            return Instance;
        }
        public static List<string> GetJsonFiles()
        {
            string path = "Data/MarkerAndPolygon";
            List<string> mpList = new List<string>();
            foreach (string fileName in Directory.GetFiles(path))
            {
                Regex regex = new Regex(@"(Data/MarkerAndPolygon\\)(.*)(.json)");
                Match match = regex.Match(fileName);
                if (match.Success)
                {
                    mpList.Add(match.Groups[2].Value);
                }
            }
            return mpList;
        }
        public static string ReadJsonFromFile(string name)
        {
            string path = "Data/MarkerAndPolygon/" + name + ".json";
            string json;
            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
        public static void WriteJsonToFile(string name, string json, bool auto)
        {
            if (auto)
            {
                DateTime dt = DateTime.Now;
                name = dt.ToString("dd-MM-yyyy") + "_" + name;
            }
            string path = "Data/MarkerAndPolygon/" + name + ".json";
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
        }
        public string ToJson()
        {
            MPWrapper wrapper = new MPWrapper(Markers, Polygons);
            foreach (int key in wrapper.Markers.Keys)
            {
                wrapper.Markers[key].Marker = null;
            }
            foreach (int key in wrapper.Polygons.Keys)
            {
                wrapper.Polygons[key].Polygon = null;
            }
            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            return json;
        }
        public void LoadJson(string json)
        {
            Clear();
            MPWrapper wrapper = JsonConvert.DeserializeObject<MPWrapper>(json);
            this.Markers = new Dictionary<int, SymbolInfo>(wrapper.Markers);
            this.Polygons = new Dictionary<int, AreaInfo>(wrapper.Polygons);
            foreach (int id in Markers.Keys)
            {
                SymbolInfo sinfo = Markers[id];
                sinfo.Marker = MarkSymbol(sinfo.ImgID, sinfo.Name, sinfo.Point, sinfo.ID);
            }
            foreach (int id in Polygons.Keys)
            {
                AreaInfo ainfo = Polygons[id];
                ainfo.Polygon = CreatePolygon(ainfo.Name, ainfo.GetPoints(), ainfo.Property);
            }
        }
        public void Clear()
        {
            GMapOverlay overlay1 = Main.GetOverlay("markersP");
            foreach (int id in Markers.Keys)
            {
                var dat = Markers[id];
                overlay1.Markers.Remove(dat.Marker);
            }
            GMapOverlay overlay2 = Main.GetOverlay("polygons");
            foreach (int id in Polygons.Keys)
            {
                var dat = Polygons[id];
                overlay2.Markers.Remove((GmapMarkerWithLabel)dat.Polygon.Tag);
                overlay2.Polygons.Remove(dat.Polygon);
            }
            Markers.Clear();
            Polygons.Clear();
        }
        public void Restore()
        {
            int mission_id = Mission.GetMissionId();
            LoadMarker(mission_id);
            LoadPolygon(mission_id);
        }
        public bool LoadAll()
        {
            bool result = false;
            int mission_id = Mission.GetMissionId();
            LoadMarker(mission_id);
            LoadPolygon(mission_id);
            return result;
        }
        public bool LoadMarker(int mission_id)
        {
            bool result = false;
            if (DataSettings.UseDatabase)
            {
                DateTime dt = DateTime.Now;
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    //clear old marker
                    GMapOverlay overlay = Main.GetOverlay("markersP");
                    foreach (int id in Markers.Keys)
                    {
                        var dat = Markers[id];
                        overlay.Markers.Remove(dat.Marker);
                    }
                    Markers.Clear();
                    //load symbol marker
                    cmd.CommandText = "SELECT * FROM marker_info WHERE mission_id = @mission_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("mission_id", mission_id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("marker_id");
                            string name = reader.GetString("marker_name");
                            int img_index = reader.GetInt32("marker_imgindex");
                            int img_id = reader.GetInt32("marker_imgid");
                            double lat = reader.GetDouble("marker_lat");
                            double lng = reader.GetDouble("marker_lng");

                            SymbolInfo sinfo = new SymbolInfo(mission_id, id, name);
                            sinfo.ImgIndex = img_index;
                            sinfo.ImgID = img_id;
                            sinfo.Point = new PointLatLng(lat, lng);
                            sinfo.Marker = MarkSymbol(img_id, name, sinfo.Point, id);
                            Markers.Add(id, sinfo);
                        }
                    }
                }
            }
            return result;
        }
        public static void AdjustPoints(List<PointLatLng> points)
        {
            double maxlat = points.Max(point => point.Lat);
            double minlat = points.Min(point => point.Lat);
            double maxlng = points.Max(point => point.Lng);
            double minlng = points.Min(point => point.Lng);
            PointLatLng _center = new PointLatLng()
            {
                Lat = (maxlat + minlat) / 2,
                Lng = (maxlng + minlng) / 2
            };
            List<PointLatLng> _q1 = points.Where(point => point.Lat >= _center.Lat && point.Lng < _center.Lng).OrderBy(point => point.Lng).ToList();
            List<PointLatLng> _q2 = points.Where(point => point.Lat >= _center.Lat && point.Lng >= _center.Lng).OrderBy(point => point.Lng).ToList();
            List<PointLatLng> _q3 = points.Where(point => point.Lat < _center.Lat && point.Lng >= _center.Lng).OrderByDescending(point => point.Lng).ToList();
            List<PointLatLng> _q4 = points.Where(point => point.Lat < _center.Lat && point.Lng < _center.Lng).OrderByDescending(point => point.Lng).ToList();

            points.Clear();
            points.AddRange(_q1);
            points.AddRange(_q2);
            points.AddRange(_q3);
            points.AddRange(_q4);
        }
        private PointLatLng GetZoneCenter(List<PointLatLng> vertexes)
        {
            PointLatLng centerPoint = new PointLatLng();
            double lat = 0;
            double lng = 0;
            foreach (var point in vertexes)
            {
                lat += point.Lat;
                lng += point.Lng;
            }
            centerPoint.Lat = lat / vertexes.Count;
            centerPoint.Lng = lng / vertexes.Count;
            return centerPoint;
        }
        public bool LoadPolygon(int mission_id)
        {
            bool result = false;
            if (DataSettings.UseDatabase)
            {
                DateTime dt = DateTime.Now;
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    //clear old marker
                    GMapOverlay overlay = Main.GetOverlay("polygons");
                    foreach (int id in Polygons.Keys)
                    {
                        var dat = Polygons[id];
                        overlay.Markers.Remove((GmapMarkerWithLabel)dat.Polygon.Tag);
                        overlay.Polygons.Remove(dat.Polygon);
                    }
                    Polygons.Clear();
                    //load polygon area
                    cmd.CommandText = "SELECT * FROM polygon_info WHERE mission_id = @mission_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("mission_id", mission_id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("area_id");
                            string name = reader.GetString("area_name");
                            int property = reader.GetInt32("area_property");
                            AreaInfo ainfo = new AreaInfo(mission_id, id, name, property);
                            Polygons.Add(id, ainfo);
                        }
                    }
                    //load polygon point
                    foreach (int id in Polygons.Keys)
                    {
                        AreaInfo ainfo = Polygons[id];
                        Dictionary<int, PointLatLng> points = new Dictionary<int, PointLatLng>();
                        cmd.CommandText = "SELECT * FROM polygon_point WHERE area_id = @id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int point_id = reader.GetInt32("point_id");
                                double lat = reader.GetDouble("point_lat");
                                double lng = reader.GetDouble("point_lng");
                                PointLatLng point = new PointLatLng(lat, lng);
                                points.Add(point_id, point);
                            }
                        }
                        ainfo.SetPoints(points);
                        ainfo.Polygon = CreatePolygon(ainfo.Name, ainfo.GetPoints(), ainfo.Property);
                    }
                }
            }
            return result;
        }

        public List<GMapPolygon> RestrictedArea
        {
            get
            {
                List<GMapPolygon> polygons = new List<GMapPolygon>();
                foreach (int id in Polygons.Keys)
                {
                    var ainfo = Polygons[id];
                    if (ainfo.Property == 1)
                    {
                        polygons.Add(ainfo.Polygon);
                    }
                }
                return polygons;
            }
        }
        private GMapMarker MarkSymbol(int img_id, string name, PointLatLng point, int IdSy)
        {
            Bitmap bmpMarker = null;
            if (string.IsNullOrEmpty(name))
                name = "-";
            if (img_id == 1)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/S1.png");
            }
            else if (img_id == 2)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/S2.png");
            }
            else if (img_id == 3)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/S3.png");
            }
            else if (img_id == 4)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/U1.png");
            }
            else if (img_id == 5)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/U2.png");
            }
            else if (img_id == 6)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/U3.png");
            }
            else if (img_id == 7)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/AB.png");
            }
            else if (img_id == 8)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/AC.png");
            }
            else if (img_id == 9)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/BL.png");
            }
            else if (img_id == 10)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/GU.png");
            }
            else if (img_id == 11)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/GA.png");
            }
            else if (img_id == 12)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/GH.png");
            }
            else if (img_id == 13)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/RD.png");
            }
            else if (img_id == 14)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/SU.png");
            }
            else if (img_id == 15)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/SA.png");
            }
            else if (img_id == 16)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/SH.png");
            }
            else if (img_id == 17)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/BC.png");
            }
            else if (img_id == 18)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/BU.png");
            }
            else if (img_id == 19)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/CP.png");
            }
            else if (img_id == 20)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/LM.png");
            }
            else if (img_id == 21)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/MA.png");
            }
            else if (img_id == 22)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/MU.png");
            }
            else if (img_id == 23)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/RS.png");
            }
            else if (img_id == 24)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/SC.png");
            }
            else if (img_id == 25)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/SN.png");
            }
            else if (img_id == 26)
            {
                bmpMarker = (Bitmap)Image.FromFile("Img/point/VP.png");
            }
            GMapMarker marker = new GMarkerGoogle(point, bmpMarker)
            {
                ToolTipText = PositionConverter.ParsePointToString(point, PositionConverter.DefaultScale) + ", " + name,
                ToolTipMode = MarkerTooltipMode.OnMouseOver,
                Tag = IdSy,
                IsHitTestVisible = true
            };
            MainForm main = MainForm.GetInstance();
            GMapControl map = main.GetmainMap();
            double zoom = map.Zoom;
            double pr = ((zoom - 5) * 10) / 100;
            if (pr == 0)
                pr = 1;
            marker.Size = new Size((int)(100*pr),(int)(100*pr));
            if (zoom <= 9)
            {
                marker.Offset = new Point((-marker.Size.Width / 2), (-marker.Size.Height / 2) - 20);
            }
            else
            {
                marker.Offset = new Point((-marker.Size.Width / 2), (-marker.Size.Height / 2) - 25);
            }
            marker.ToolTip.Font = new Font("TH SarabunPSK", 14, FontStyle.Bold);
            

            GMapOverlay overlay = Main.GetOverlay("markersP");
            overlay.Markers.Add(marker);
            return marker;
        }
        public GMapPolygon CreatePolygon(string name, List<PointLatLng> points, int property)
        {
            AdjustPoints(points);
            GMapPolygon polygon = new GMapPolygon(points, name);
            polygon.Fill = new SolidBrush(Color.FromArgb(30, Color.DarkGray));
            polygon.Stroke = new Pen(Color.Red, 2);
            if (property == 1)
            {
                polygon.Stroke.DashStyle = DashStyle.Dot;
            }
            polygon.IsHitTestVisible = true;

            Bitmap pic = (Bitmap)Image.FromFile("Img/point/em.png");
            PointLatLng center = GetZoneCenter(points);
            MainForm main = MainForm.GetInstance();
            GMapControl map = main.GetmainMap();
            double zoom = map.Zoom;
            double pr = ((zoom - 5) * 10) / 100;
            if (pr == 0)
                pr = 1;
            var labelMarker = new GmapMarkerWithLabel(center, name, pic,(int)(20*pr));
            labelMarker.Offset = new System.Drawing.Point(((-labelMarker.Size.Width / 2)+25), (-labelMarker.Size.Height / 2));

            GMapOverlay overlay = Main.GetOverlay("polygons");
            overlay.Polygons.Add(polygon);
            overlay.Markers.Add(labelMarker);

            polygon.Tag = labelMarker;
            return polygon;
        }
        public bool FindMarker(int ID)
        {
            return Markers.ContainsKey(ID);
        }
        public int FindMarker(GMapMarker marker)
        {
            foreach (int ID in Markers.Keys)
            {
                SymbolInfo sinfo = Markers[ID];
                if (marker == sinfo.Marker)
                {
                    return ID;
                }
            }
            return -1;
        }
        public bool FindPolygon(int ID)
        {
            return Polygons.ContainsKey(ID);
        }
        public int FindPolygon(GMapPolygon polygon)
        {
            foreach (int ID in Polygons.Keys)
            {
                AreaInfo ainfo = Polygons[ID];
                if (polygon == ainfo.Polygon)
                {
                    return ID;
                }
            }
            return -1;
        }
        public SymbolInfo GetMarkerData(int ID)
        {
            return Markers[ID];
        }
        public AreaInfo GetPolygonData(int ID)
        {
            return Polygons[ID];
        }
        private int random_num()
        {
            Random random = new Random();
            int getNum = random.Next(1, 100000);
            return getNum;
        }
        public void SaveMarker(DateTime dt)
        {
            if (DataSettings.UseDatabase)
            {
                int mission_id = -1;
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    //get mission id
                    cmd.CommandText = "SELECT * FROM save_mission WHERE mission_date = @date";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("date", dt);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mission_id = reader.GetInt32("mission_id");
                        }
                    }
                    foreach (int id in Markers.Keys)
                    {
                        SymbolInfo sinfo = Markers[id];
                        cmd.CommandText = "INSERT INTO marker_info VALUES (@id, @name, @img_index, @img_id, @lat, @lng, @mission_id) " +
                            "ON DUPLICATE KEY UPDATE marker_name = @name, marker_imgindex = @img_index, marker_imgid = @img_id, marker_lat = @lat, marker_lng = @lng";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", sinfo.ID);
                        cmd.Parameters.AddWithValue("name", sinfo.Name);
                        cmd.Parameters.AddWithValue("img_index", sinfo.ImgIndex);
                        cmd.Parameters.AddWithValue("img_id", sinfo.ImgID);
                        cmd.Parameters.AddWithValue("lat", sinfo.Point.Lat);
                        cmd.Parameters.AddWithValue("lng", sinfo.Point.Lng);
                        cmd.Parameters.AddWithValue("mission_id", sinfo.Mission);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private static int simID = 0;
        public void AddMarker(PointLatLng Point, string Name, int Img_index, int Img_ID)
        {
            SymbolInfo sinfo;
            int mission_id, marker_id;
            if (DataSettings.UseDatabase)
            {
                mission_id = Mission.GetMissionId();
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    cmd.CommandText = "INSERT INTO marker_info (marker_name, marker_imgindex, marker_imgid, marker_lat, marker_lng, mission_id) " +
                        "VALUES (@name, @img_index, @img_id, @lat, @lng, @mission_id); SELECT LAST_INSERT_ID();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("name", Name);
                    cmd.Parameters.AddWithValue("img_index", Img_index);
                    cmd.Parameters.AddWithValue("img_id", Img_ID);
                    cmd.Parameters.AddWithValue("lat", Point.Lat);
                    cmd.Parameters.AddWithValue("lng", Point.Lng);
                    cmd.Parameters.AddWithValue("mission_id", mission_id);
                    marker_id = Convert.ToInt32(cmd.ExecuteScalar());
                    sinfo = new SymbolInfo(mission_id, marker_id, Name);
                    sinfo.ImgIndex = Img_index;
                    sinfo.ImgID = Img_ID;
                    sinfo.Point = Point;
                    sinfo.Marker = MarkSymbol(Img_ID, Name, Point, marker_id);
                }
            }
            else
            {
                mission_id = 0;
                marker_id = simID++;
                sinfo = new SymbolInfo(mission_id, marker_id, Name);
                sinfo.ImgIndex = Img_index;
                sinfo.ImgID = Img_ID;
                sinfo.Point = Point;
                sinfo.Marker = MarkSymbol(Img_ID, Name, Point, marker_id);
            }
            Markers.Add(marker_id, sinfo);
        }
        public void AddPolygon(List<PointLatLng> Points, string Name, int Property)
        {
            AreaInfo ainfo;
            int mission_id, polygon_id;
            if (DataSettings.UseDatabase)
            {
                mission_id = Mission.GetMissionId();
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    cmd.CommandText = "INSERT INTO polygon_info (area_name, area_property, mission_id) " +
                        "VALUES (@name, @property, @mission_id); SELECT LAST_INSERT_ID();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("name", Name);
                    cmd.Parameters.AddWithValue("property", Property);
                    cmd.Parameters.AddWithValue("mission_id", mission_id);
                    polygon_id = Convert.ToInt32(cmd.ExecuteScalar());
                    Dictionary<int, PointLatLng> points = new Dictionary<int, PointLatLng>();
                    foreach (PointLatLng point in Points)
                    {
                        cmd.CommandText = "INSERT INTO polygon_point (point_lat, point_lng, area_id) " +
                            "VALUES (@lat, @lng, @area_id); SELECT LAST_INSERT_ID();";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("lat", point.Lat);
                        cmd.Parameters.AddWithValue("lng", point.Lng);
                        cmd.Parameters.AddWithValue("area_id", polygon_id);
                        int point_id = Convert.ToInt32(cmd.ExecuteScalar());
                        points.Add(point_id, point);
                    }
                    ainfo = new AreaInfo(mission_id, polygon_id, Name, Property);
                    ainfo.SetPoints(points);
                    ainfo.Polygon = CreatePolygon(Name, Points, Property);
                    Polygons.Add(polygon_id, ainfo);
                }
            }
            else
            {
                mission_id = 0;
                polygon_id = simID++;
                ainfo = new AreaInfo(mission_id, polygon_id, Name, Property);
                Dictionary<int, PointLatLng> points = new Dictionary<int, PointLatLng>();
                for (int i = 0; i < Points.Count; i++)
                {
                    points.Add(i, Points[i]);
                }
                ainfo.SetPoints(points);
                ainfo.Polygon = CreatePolygon(Name, Points, Property);
                Polygons.Add(polygon_id, ainfo);
            }
        }
        public void EditMarker(int ID, string Name, int Img_index, int Img_ID, double Lat, double Lng)
        {
            if (Markers.ContainsKey(ID))
            {
                SymbolInfo sinfo = Markers[ID];
                sinfo.Name = Name;
                sinfo.ImgIndex = Img_index;
                sinfo.ImgID = Img_ID;
                sinfo.Point = new PointLatLng(Lat, Lng);
                sinfo.Marker.ToolTipText = PositionConverter.ParsePointToString(new PointLatLng(Lat, Lng), PositionConverter.DefaultScale) + ", " + Name;

                if (DataSettings.UseDatabase)
                {
                    MySqlConnection conn = DataSettings.GetConnection();
                    using (MySqlCommand cmd = new MySqlCommand(null, conn))
                    {
                        cmd.CommandText = "UPDATE marker_info SET marker_name = @name, marker_imgindex = @img_index, marker_imgid = @img_id" +
                            ", marker_lat = @lat, marker_lng = @lng, mission_id = @mission_id WHERE marker_id = @id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", sinfo.ID);
                        cmd.Parameters.AddWithValue("name", sinfo.Name);
                        cmd.Parameters.AddWithValue("img_index", sinfo.ImgIndex);
                        cmd.Parameters.AddWithValue("img_id", sinfo.ImgID);
                        cmd.Parameters.AddWithValue("lat", sinfo.Point.Lat);
                        cmd.Parameters.AddWithValue("lng", sinfo.Point.Lng);
                        cmd.Parameters.AddWithValue("mission_id", sinfo.Mission);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void EditPolygonPoints(int ID, List<PointLatLng> Points)
        {
            if (Polygons.ContainsKey(ID))
            {
                List<PointLatLng> np = new List<PointLatLng>(Points);
                AdjustPoints(np);
                AreaInfo ainfo = Polygons[ID];
                if (DataSettings.UseDatabase)
                {
                    MySqlConnection conn = DataSettings.GetConnection();
                    using (MySqlCommand cmd = new MySqlCommand(null, conn))
                    {
                        Dictionary<int, PointLatLng> points = new Dictionary<int, PointLatLng>();
                        Dictionary<int, PointLatLng> op = ainfo.Points;
                        foreach (int point_id in op.Keys)
                        {
                            cmd.CommandText = "DELETE FROM polygon_point WHERE point_id = @id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("id", point_id);
                            cmd.ExecuteNonQuery();
                        }
                        foreach (PointLatLng point in Points)
                        {
                            cmd.CommandText = "INSERT INTO polygon_point (point_lat, point_lng, area_id) " +
                                "VALUES (@lat, @lng, @area_id); SELECT LAST_INSERT_ID();";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("lat", point.Lat);
                            cmd.Parameters.AddWithValue("lng", point.Lng);
                            cmd.Parameters.AddWithValue("area_id", ID);
                            int point_id = Convert.ToInt32(cmd.ExecuteScalar());
                            points.Add(point_id, point);
                        }
                        ainfo.SetPoints(points);
                        ainfo.Polygon = CreatePolygon(ainfo.Name, Points, ainfo.Property);
                    }
                }
                else
                {
                    Dictionary<int, PointLatLng> points = new Dictionary<int, PointLatLng>();
                    for (int i = 0; i < Points.Count; i++)
                    {
                        points.Add(i, Points[i]);
                    }
                    ainfo.SetPoints(points);
                    ainfo.Polygon = CreatePolygon(ainfo.Name, Points, ainfo.Property);
                }
            }
        }
        public void RemoveMarker(int ID)
        {
            if (Markers.ContainsKey(ID))
            {
                SymbolInfo sinfo = Markers[ID];
                GMapOverlay overlay = Main.GetOverlay("markersP");
                overlay.Markers.Remove(sinfo.Marker);

                Markers.Remove(ID);
                if (DataSettings.UseDatabase)
                {
                    MySqlConnection conn = DataSettings.GetConnection();
                    using (MySqlCommand cmd = new MySqlCommand(null, conn))
                    {
                        cmd.CommandText = "DELETE FROM marker_info WHERE marker_id = @id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", ID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void RemovePolygon(int ID)
        {
            if (Polygons.ContainsKey(ID))
            {
                AreaInfo ainfo = Polygons[ID];
                GMapOverlay overlay = Main.GetOverlay("polygons");
                overlay.Markers.Remove((GmapMarkerWithLabel) ainfo.Polygon.Tag);
                overlay.Polygons.Remove(ainfo.Polygon);

                Polygons.Remove(ID);
                if (DataSettings.UseDatabase)
                {
                    MySqlConnection conn = DataSettings.GetConnection();
                    using (MySqlCommand cmd = new MySqlCommand(null, conn))
                    {
                        cmd.CommandText = "DELETE FROM polygon_info WHERE area_id = @id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", ID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
