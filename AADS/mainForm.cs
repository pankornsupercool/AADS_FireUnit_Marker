
using AADS.Views.FireUnit;
using Demo.WindowsForms.CustomMarkers;
using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Net_GmapMarkerWithLabel;
using NewRadarUX;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS
{
    public partial class MainForm : Form
    {
        public FireunitEvent handler;
        private List<PointLatLng> _points = new List<PointLatLng>();
        internal readonly GMapOverlay top = new GMapOverlay();
        GMapOverlay markersP = new GMapOverlay("markersP");
        internal readonly GMapOverlay Radar = new GMapOverlay("Radar");
        internal readonly GMapOverlay lineDistance = new GMapOverlay("lineDistance");
        internal readonly GMapOverlay polygons = new GMapOverlay("polygons");
        internal readonly GMapOverlay objects = new GMapOverlay("objects");
        internal readonly GMapOverlay Track = new GMapOverlay("Track");
        internal readonly GMapOverlay Test = new GMapOverlay("Test");
        internal readonly GMapOverlay BordersTrack = new GMapOverlay("BordersTrack");
        internal readonly GMapOverlay midlineDistance = new GMapOverlay("midlineDistance");

        internal readonly GMapOverlay minMapOverlay = new GMapOverlay("minMapOverlay");
        //

        public GMapControl gMap;
        public TrackManager trackHandler = new TrackManager();
        List<MapMode> mapModes = new List<MapMode>();
        GMapMarker currentMarker;
        GMapMarkerRect CurentRectMarker = null;
        public static GMapMarker currentMarkerTag;
        GMapPolygon polygon;
        string action = null;
        static int simID = 300;
        bool minMapAutoZoom = false;

        MarkerAndPolygon markerAndPolygon;
        public RadarOverlay radarP;

        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            bool connected = RadarClient.ConnectToServer(IPAddress.Parse(txtServerAddress.Text), 8888);
            if (!connected)
            {
                MessageBox.Show("Cant connect");
            }
        }
        public GMapOverlay GetOverlay(string Name)
        {
            return mainMap.Overlays.FirstOrDefault(x => x.Id == Name);
        }
        public static MainForm GetInstance()
        {
            return Instance;
        }
        public GMapControl GetmainMap()
        {
            return mainMap;
        }
        public static MainForm Instance;
        private void readJsonMap()
        {
            using (StreamReader reader = new StreamReader("Maps.json"))
            {
                string json = reader.ReadToEnd();
                JArray list = JArray.Parse(json);
                foreach (JObject data in list.Children())
                {
                    mapModes.Add(new MapMode
                    {
                        Name = (string)data["Name"],
                        MapProvider = GMapProviders.TryGetProvider((string)data["Type"]),
                        MainMapMinZoom = (int)data["MainMapMinZoom"],
                        MainMapMaxZoom = (int)data["MainMapMaxZoom"],
                        MiniMapMinZoom = (int)data["MiniMapMinZoom"],
                        MiniMapMaxZoom = (int)data["MiniMapMaxZoom"]
                    });
                }
            }
        }
        private static PointLatLng radarLocation = new PointLatLng(14.94561195, 102.0929003);
        private static double radarRadius = 240;
        private GMapPolygon CreateCircle(PointLatLng point, double radius, float stroke)
        {
            int segments = 1080;
            List<PointLatLng> gpollist = new List<PointLatLng>();
            for (int i = 0; i < segments; i++)
            {
                gpollist.Add(FindPointAtDistanceFrom(point, i, radius));
            }
            GMapPolygon gpol = new GMapPolygon(gpollist, "circle");
            gpol.Fill = new SolidBrush(Color.Transparent);
            gpol.Stroke = new Pen(Color.CornflowerBlue, stroke);
            gpol.IsHitTestVisible = true;
            return gpol;
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
        private void setupRadar()
        {
            Radar.Markers.Clear();
            Radar.Polygons.Clear();
            Bitmap image = new Bitmap("Images/radar_site.png");
            GMapMarker marker = new GMarkerGoogle(radarLocation, image);
            GMapPolygon circle = CreateCircle(radarLocation, radarRadius, 2.0f);
            Radar.Markers.Add(marker);
            Radar.Polygons.Add(circle);
        }
        private void trackHandler_TrackClear(List<TrackData> tracks, List<GMarkerTrack> markers, TrackEventArgs e)
        {
            Track.Markers.Clear();
        }
        private void trackHandler_TrackAdd(TrackData item, TrackEventArgs e)
        {
            GMarkerTrack marker = new GMarkerTrack(item);
            Track.Markers.Add(marker);
            trackHandler.trackMarkers.Add(item.Key, marker);
        }
        private void trackHandler_TrackUpdate(TrackData item, TrackEventArgs e)
        {
            if (trackHandler.trackMarkers.ContainsKey(item.Key))
            {
                GMarkerTrack marker = trackHandler.trackMarkers[item.Key];
                marker.SetTrack(item);
            }
        }
        private void trackHandler_TrackRemove(TrackData item, TrackEventArgs e)
        {
            if (trackHandler.trackMarkers.ContainsKey(item.Key))
            {
                GMarkerTrack marker = trackHandler.trackMarkers[item.Key];
                Track.Markers.Remove(marker);
                trackHandler.trackMarkers.Remove(item.Key);
            }
        }
        public MainForm()
        {
            handler = FireunitEvent.Instance;
            InitializeComponent();
            Instance = this;
            gMap = mainMap;
            mainMap.Manager.BoostCacheEngine = true;
            mainMap.MapScaleInfoEnabled = true;
            mainMap.MapScaleInfoPosition = MapScaleInfoPosition.Bottom;

            readJsonMap();

            trackHandler.OnTrackClear += trackHandler_TrackClear;
            trackHandler.OnTrackAdd += trackHandler_TrackAdd;
            trackHandler.OnTrackUpdate += trackHandler_TrackUpdate;
            trackHandler.OnTrackRemove += trackHandler_TrackRemove;

            if (!GMapControl.IsDesignerHosted)
            {
                mainMap.Overlays.Add(Radar);
                mainMap.Overlays.Add(polygons);
                mainMap.Overlays.Add(lineDistance);
                mainMap.Overlays.Add(midlineDistance);
                mainMap.Overlays.Add(markersP);
                mainMap.Overlays.Add(objects);
                mainMap.Overlays.Add(Test);
                mainMap.Overlays.Add(Track);
                mainMap.Overlays.Add(BordersTrack);
                mainMap.Overlays.Add(top);
                minMap1.Overlays.Add(minMapOverlay);

                mainMap.Manager.Mode = AccessMode.ServerAndCache;
                mainMap.MapProvider = mapModes[0].MapProvider;
                mainMap.Position = new PointLatLng(13.7563, 100.5018);
                mainMap.MinZoom = mapModes[0].MainMapMinZoom;
                mainMap.MaxZoom = mapModes[0].MainMapMaxZoom;
                mainMap.Zoom = 8;

                minMap1.Manager.Mode = AccessMode.ServerAndCache;
                minMap1.MapProvider = mapModes[0].MapProvider;
                minMap1.Position = new PointLatLng(13.7563, 100.5018);
                minMap1.MinZoom = mapModes[0].MiniMapMinZoom;
                minMap1.MaxZoom = mapModes[0].MiniMapMaxZoom;
                minMap1.Zoom = 4;

                {
                    mainMap.OnPositionChanged += new PositionChanged(mainMap_OnPositionChanged);

                    //mainMap.OnMapZoomChanged += new MapZoomChanged(mainMap_OnMapZoomChanged);
                    mainMap.OnMapTypeChanged += new MapTypeChanged(mainMap_OnMapTypeChanged);

                    mainMap.MouseUp += new MouseEventHandler(mainMap_MouseUp);
                    mainMap.MouseDown += new MouseEventHandler(mainMap_MouseDown);
                    mainMap.MouseMove += new MouseEventHandler(mainMap_MouseMove);
                    mainMap.MouseClick += new MouseEventHandler(mainMap_MouseClick);
                    //mainMap.MouseClick += new MouseEventHandler(mainMap_MouseClick2);
                    mainMap.OnMarkerClick += new MarkerClick(mainMap_OnMarkerClick);
                }

                currentMarker = new GMarkerGoogle(mainMap.Position, GMarkerGoogleType.arrow);
                currentMarker.IsHitTestVisible = false;
                top.Markers.Add(currentMarker);
                //Console.WriteLine(CPC.Intersection(new PointLatLng(51.8853, 0.2545), new PointLatLng(49.0034, 2.5735), 108.547, 32.435).ToString());
                //Console.WriteLine(CPC.destinationPoint(new PointLatLng(51.127, 1.338), 40300, 116.7));
                //Console.WriteLine(CPC.rhumbBearingTo(new PointLatLng(51.127, 1.338),new PointLatLng(50.964, 1.853)).ToString());
            }
        }
        void updateMinMap()
        {
            minMap1.Position = mainMap.Position;
            List<PointLatLng> plist = new List<PointLatLng>();
            int width = mainMap.Size.Width - 1;
            int height = mainMap.Size.Height - 1;
            plist.Add(mainMap.FromLocalToLatLng(0, 0));
            plist.Add(mainMap.FromLocalToLatLng(width, 0));
            plist.Add(mainMap.FromLocalToLatLng(width, height));
            plist.Add(mainMap.FromLocalToLatLng(0, height));
            GMapPolygon poly = new GMapPolygon(plist, "");
            poly.Fill = Brushes.Transparent;
            poly.Stroke = new Pen(Brushes.Red, 1.6f);
            minMapOverlay.Polygons.Clear();
            minMapOverlay.Polygons.Add(poly);
            if (minMapAutoZoom)
            {
                int minMapArea = minMap1.Size.Height * minMap1.Size.Width;
                long dX = minMap1.FromLatLngToLocal(poly.Points[1]).X - minMap1.FromLatLngToLocal(poly.Points[0]).X;
                long dY = minMap1.FromLatLngToLocal(poly.Points[2]).Y - minMap1.FromLatLngToLocal(poly.Points[1]).Y;
                long area = dX * dY;
                double ratio = (double)area / minMapArea;
                if (ratio < 0.2)
                {
                    minMap1.Zoom++;
                }
                else if (ratio > 0.8)
                {
                    minMap1.Zoom--;
                }
            }
        }

        #region -- event mainMap --
        void mainMap_OnPositionChanged(PointLatLng point)
        {
            updateMinMap();
        }

        void mainMap_OnMapTypeChanged(GMapProvider type)
        {

        }

        void mainMap_OnMapZoomChanged()
        {
            updateMinMap();
        }


        bool isMouseDown = false;
        bool isRightClick = false;
        Point lastLocation;
        void mainMap_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        void mainMap_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            isRightClick = e.Button == MouseButtons.Right;
        }

        void mainMap_MouseMove(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }

        
        #endregion

        #region -- data Code --
        private int RadarRadius = 140;
        private Dictionary<string, RadarOverlay> radars = new Dictionary<string, RadarOverlay>();
        private void loadPointR()
        {
            //first section
            {
                string path = @"พิกัดเรดาร์.txt";
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            //PointLatLng pointR = PositionConverter.ParsePointFromString(line);
                            //pointRader = pointR;
                            //textBoxPositionR.Text = PositionConverter.ParsePointToString(pointR, comboBoxScale.Text);
                        }
                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine("13.75, 100.517");
                        //textBoxPositionR.Text = "13.75, 100.517";
                        writer.Close();
                    }
                }
            }
            string name = "Test";
            int interval = 140;//Convert.ToInt32(textBoxRadarInterval.Text);
            radarP = new RadarOverlay(name, Radar);
            int x = 0;
            //Int32.TryParse(textBoxRadarRadius.Text, out x);
            radarP.InitialRadar(PositionConverter.ParsePointFromString("13.75, 100.517"), x, interval);
            radars.Add(name, radarP);
        }
        #endregion
        private void updateCmbMapMode()
        {
            cmbMapMode.Items.Clear();
            foreach (MapMode mapMode in mapModes)
            {
                cmbMapMode.Items.Add(mapMode.Name);
            }
        }
        private Point label27Location;

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (RadarClient.ClientSocket.Connected)
            {
                RadarClient.Exit();
            }
        }




        private void Handler_onFireunitChanged2(FireunitEventArgs2 e)
        {
            // Just pass in
        }

        private void Handler_onFireunitChanged(FireunitEventArgs e)
        {
            // Just pass in 
        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            timerClose.Start();
        }

        private void maximizeBox_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
            label27Location = new Point(this.Width - label27.Width, label27.Location.Y);
        }

        private void minimizeBox_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void timeNow_Tick(object sender, EventArgs e)
        {
            DateTime Date = DateTime.Now;
            dateLabel.Text = Date.ToString("dd-MMM-yyyy");
            time_label.Text = Date.ToString("HH:mm:ss");
        }
        private void callFixedPoint()
        {
            using (Views.FixedPoint.main form = new Views.FixedPoint.main())
            {
                form.OnClickAdd += new EventHandler(fixedPoint_ClickAdd);
                form.ShowDialog();
            }
        }
        private void cmbMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMenu.SelectedIndex > -1)
            {
                if (cmbMenu.SelectedItem.ToString().Equals("Fixed Point"))
                {
                    callFixedPoint();
                }
            }
        }
        private void fixedPoint_ClickAdd(object sender, EventArgs e)
        {
            action = "fixedPointAdd";
            labelCurrentAction.Text = "Action: Fixed Point";
        }

        private void cmbMapMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbMapMode.SelectedIndex;
            MapMode mode = mapModes[index];
            if (mainMap.MapProvider != mode.MapProvider)
            {
                mainMap.MapProvider = mode.MapProvider;
                minMap1.MapProvider = mode.MapProvider;
                mainMap.MinZoom = mode.MainMapMinZoom;
                mainMap.MaxZoom = mode.MainMapMaxZoom;
                minMap1.MinZoom = mode.MiniMapMinZoom;
                minMap1.MaxZoom = mode.MiniMapMaxZoom;
            }
        }
        private bool hasPanelRight = false;
        private void label27_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        
      
        private void btnShow_Maker_Click(object sender, EventArgs e)
        {
            var MarkerPage = new Views.ShowCategory.Marker();
            panelRightShow.Controls.Clear();
            panelRightShow.Controls.Add(MarkerPage);

        }

        private void btnShow_Line_Click(object sender, EventArgs e)
        {
            var LinePage = new Views.ShowCategory.Polygon();
            panelRightShow.Controls.Clear();
            panelRightShow.Controls.Add(LinePage);
        }
        private void btnShow_Polygon_Click(object sender, EventArgs e)
        {
            var PlygonPage = new Views.ShowCategory.Polygon();
            panelRightShow.Controls.Clear();
            panelRightShow.Controls.Add(PlygonPage);
        }
        private void btnShow_Track_Click(object sender, EventArgs e)
        {
            var TrackPage = new Views.ShowCategory.Track();
            panelRightShow.Controls.Clear();
            panelRightShow.Controls.Add(TrackPage);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int x = panelRight.Location.X;
            int y = 93;
            if (hasPanelRight)
            {
                x = x + (panelRight.Width / 14);
                label27.Location = new Point(x, y);
                panelRight.Location = new Point(x, y);
                if (x >= this.Width)
                {
                    timer1.Stop();
                    hasPanelRight = false;
                    label27.Text = "<<<";
                    label27.Location = label27Location;
                }
            }
            else
            {
                x = x - (panelRight.Width / 14);
                label27.Location = new Point(x - label27.Width, y);
                panelRight.Location = new Point(x, y);
                if (x <= this.Width - panelRight.Width + 14)
                {
                    timer1.Stop();
                    hasPanelRight = true;
                    label27.Text = ">>>";
                }
            }
        }

        
        private void timerOpen_(object sender, EventArgs e)
        {
            Opacity += 0.05;
            if (Opacity >= 1)
            {
                timerOpen.Stop();
            }
        }
        private void timerClose_(object sender, EventArgs e)
        {
            Opacity -= 0.05;
            if (Opacity <= 0)
            {
                Application.Exit();
                timerOpen.Stop();
            }

        }

        private void btnUnit_Click(object sender, EventArgs e)
        {
            if (panelRightMap.Visible)
                panelRightShow.Controls.Clear();
            panelRightMap.Visible = false;
            panelRightUnit.Visible = true; 
           
        }
        private void btnMap_Click(object sender, EventArgs e)
        {
            if (!panelRightMap.Visible)
                panelRightShow.Controls.Clear();
            panelRightMap.Visible = true;
            panelRightUnit.Visible = false;
           
        }

        private void timerCheckConnection_Tick(object sender, EventArgs e)
        {
            if (RadarClient.ClientSocket.Connected)
            {
                labelConnect.Text = "Open";
                labelTrackCount.Text = trackHandler.GetTracks().Count.ToString();
            }
            else
            {
                labelConnect.Text = "Close";
                labelTrackCount.Text = "0";
            }
        }








        public event EventHandler<int> RaiseDelete;
        public event EventHandler HideDelete;
        public event EventHandler ClearScreen;
        public event EventHandler<int> isSelected;
        public event EventHandler<GMapMarker> CurrentMarker;
        public event EventHandler<int> TagNumber;

        private void mainForm_Load(object sender, EventArgs e)
        {
            mainMap.ShowCenter = false;
            //handler = FireunitEvent.Instance;

            handler.onFireunitChanged += Handler_onFireunitChanged;
            handler.onFireunitChanged2 += Handler_onFireunitChanged2;
            //RaiseDelete += MainForm_RaiseDelete;
            //HideDelete += MainForm_HideDelete;
            MainFireunit.DeleteEvent += MainFireunit_DeleteEvent;
            //MainFireunit.EditEvent += MainFireunit_EditEvent;


            timeNow.Start();
            timerCheckConnection.Start();
            updateMinMap();
            updateCmbMapMode();
            setupRadar();
            mainMap.Position = radarLocation;
            cmbMapMode.SelectedIndex = 0;
            panelRight.Height = this.Height - panelControl.Height - panelTop.Height - panelBottom.Height;
            panelRight.Location = new Point(1950, 93);
            label27Location = new Point(this.Width - label27.Width, label27.Location.Y);

        }





        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if(currentMarkerTag != null)
        //    {
        //        markersP.Markers.Remove(currentMarkerTag);
        //        FireunitList.Tag.Remove(Convert.ToInt32(currentMarkerTag.Tag));
        //        currentMarkerTag = null;
        //    }
        //}


        void mainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {

            //MessageBox.Show(FireUnitTag.tag.Count.ToString());
            //foreach (var items in FireUnitTag.tag)
            //{
            //    MessageBox.Show("This : ",items.ToString());
            //}     
            currentMarkerTag = item;
            //MessageBox.Show(currentMarkerTag.Tag.ToString(), "onMarkerClick");

            int idx = FireunitList.Tag.IndexOf(Convert.ToInt32(currentMarkerTag.Tag));
            if(currentMarkerTag.Tag != null)
            {
                try
                {
                    if (FireunitList.Tag.Contains(Convert.ToInt32(currentMarkerTag.Tag)))
                    {
                        //MessageBox.Show(idx.ToString(), "Current tag index on list");
                        CurrentMarker?.Invoke(this, item);
                        isSelected?.Invoke(this, idx);
                        TagNumber?.Invoke(this, Convert.ToInt32(currentMarkerTag.Tag));
                        RaiseDelete?.Invoke(this, idx);

                    }
                    else
                    {
                        isSelected?.Invoke(this, idx);
                        RaiseDelete?.Invoke(this, idx);
                        //MessageBox.Show("Unknowed");
                    }
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }
            else
            {
                MessageBox.Show("Radar Marker");
                MessageBox.Show("Tag is Null");
                HideDelete?.Invoke(this, EventArgs.Empty);
            }



            

            //(FireUnitTag.tag.Contains(currentMarkerTag.Tag.ToString()))
            //{
            //    FireunitEventArgs2 args2 = new FireunitEventArgs2
            //    {
            //        Location = item.Position.ToString(),
            //        Detail = item.Tag.ToString()
            //    };
            //    FireunitEvent handler = FireunitEvent.Instance;
            //    handler.InvokeFireunitChanged2(args2);
            //    btnDelete.Enabled = true;
            //}
            //else
            //{
            //    MessageBox.Show("Unknowed");
            //}
            //if (e.Button == MouseButtons.Right)
            //{
            //    MessageBox.Show("Right Click");
            //}
        }


        void mainMap_MouseClick(object sender, MouseEventArgs e)
        {
            PointLatLng pnew = mainMap.FromLocalToLatLng(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                currentMarker.Position = pnew;
                GlobalFireUnit.LatLng = PositionConverter.ParsePointToString(pnew, "Signed Degree");
                GlobalFireUnit.Lat = pnew.Lat;
                GlobalFireUnit.Lng = pnew.Lng;
                FireunitEventArgs args = new FireunitEventArgs
                {
                    LatLng = GlobalFireUnit.LatLng
                };
                FireunitEvent handler = FireunitEvent.Instance;
                handler.InvokeFireunitChanged(args);
                ClearScreen?.Invoke(this, EventArgs.Empty);
            }else if (e.Button == MouseButtons.Right)
            {

            }

            if (action != null)
            {
                string ca = action;
                action = null;
                if (ca == "fixedPointAdd")
                {
                    labelCurrentAction.Text = "Action: Free";
                    callFixedPoint();
                }
            }


        }


        private void MainFireunit_DeleteEvent(object sender, EventArgs e)
        {
            var idx_toremove = Convert.ToInt32(currentMarkerTag.Tag);
            if (currentMarkerTag != null)
            {
                markersP.Markers.Remove(currentMarkerTag);

                var idx_tag_remove = FireunitList.Tag.IndexOf(idx_toremove);
                FireunitList.Tag.Remove(idx_toremove);

                //MessageBox.Show(idx_tag_remove.ToString());

                FireunitList.BatteryID.RemoveAt(idx_tag_remove);
                FireunitList.Number.RemoveAt(idx_tag_remove);
                FireunitList.Type.RemoveAt(idx_tag_remove);
                FireunitList.Location.RemoveAt(idx_tag_remove);
                FireunitList.Detail.RemoveAt(idx_tag_remove);
                FireunitList.Status.RemoveAt(idx_tag_remove);

                currentMarkerTag = null;
                //MessageBox.Show($"Delete index {idx_toremove.ToString()}");
                MessageBox.Show("Delete Completed!");
            }
            else
            {
                MessageBox.Show("Select Marker Needed");
                
            }

            HideDelete?.Invoke(this, EventArgs.Empty);
        }

        //private void MainFireunit_EditEvent(object sender, EventArgs e)
        //{
        //    var idx_toEdit = Convert.ToInt32(currentMarkerTag.Tag);
        //    if (currentMarkerTag != null)
        //    {
                
        //    }
        //}

    }
}
