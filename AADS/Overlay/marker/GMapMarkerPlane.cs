using Demo.WindowsForms.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using AADS;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace NewRadarUX
{
    public class GMapMarkerPlane : GMapMarker, ISerializable
    {
        public Bitmap icon = (Bitmap)Image.FromFile("Images/icons8_airport.ico");
        private float Tailheading;
        public int id;
        public float heading;
        public double speed;
        public FlightRadarData flight;
        public static Pen _TailColor = new Pen(Brushes.DeepSkyBlue);
        public string ty;
        public bool Tile;
        public string _caption;
        public double zoom;
        //Arrow
        //static Point[] Arrow = new Point[] { new Point(-7, 7), new Point(0, -22), new Point(7, 7), new Point(0, 2) };
        public Brush Fill = new SolidBrush(Color.FromArgb(155, Color.Blue));
        private float scale = 1;
        private Image img;

        public static Pen TailColor
        {
            get { return _TailColor; }
            set { _TailColor = value; }
        }

        public GMapMarkerPlane(PointLatLng p, float heading, double speed,int id,double zoom)
            : base(p)
        {
            this.Position = p; 
            this.heading = heading;
            this.speed = speed;
            this.id = id;
            this.zoom = zoom;
            Size = icon.Size;
            Scale = 1;

            var track = DataSettings.Track;
            if (track["PlaneSY"] == 0)
            {
                ty = "Arrow";
            }
            else if (track["PlaneSY"] == 1)
            {
                ty = "Plane";
            }
            else if (track["PlaneSY"] == 2)
            {
                ty = "Nato";
            }
            Tailheading = track["TailSY"] ? heading : (heading + 180) % 360;
            Tile = track["TailSY"];
        }

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;

                Size = new System.Drawing.Size((int)(14 * scale), (int)(14 * scale));
                Offset = new System.Drawing.Point(-Size.Width / 2, (int)(-Size.Height / 1.4));
            }
        }

        public Point[] Arrow
        {
            get
            {
                return new Point[] { new Point(-(int)(10.5*pr), (int)(10.5*pr)), new Point(0, (int)(-33*pr)), new Point((int)(10.5 * pr), (int)(10.5 * pr)), new Point(0, (int)(3* pr)) };
            }
            set
            {
                Arrow = new Point[] { new Point(-7, 7), new Point(0, -22), new Point(7, 7), new Point(0, 2) };
            }
        }

        private double pr
        {
            get
            {
                double pr = ((zoom - 5) * 10) / 100;
                if (pr == 0)
                    pr = 1;
                return pr;
            }
            set
            {
                pr = 1;
            }
        }
        private double Length
        {
            get
            {
                int set = 0;
                //if (Tile == true) set = 2; else set = 0;
                if (ty == "Plane")
                    set = 2;
                else
                    set = 0;

                if (speed >= 125 && speed <= 300)
                {
                    return (4 + set) * pr;
                }
                else if (speed > 300 && speed <= 550)
                {
                    return (7 + set) * pr;
                }
                else if (speed > 550 && speed <= 2000)
                {
                    return (11 + set) * pr;
                }
                else if (speed > 2000)
                {
                    return (11 + set) * pr;
                }
                else
                {
                    return 0;
                }
            }
        }

        private static Brush CaptionColor
        {
            get
            {
                MainForm main = MainForm.GetInstance();
                GMapControl map = main.GetmainMap();
                bool provider = map.NegativeMode;
                if (provider == false)
                {
                    return Brushes.Black; 
                }
                else
                {
                    return Brushes.White;
                }
                return Brushes.White;
            }
        }
        Bitmap Resize(Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.DrawImage(image, 0, 0, w, h);
            graphic.Dispose();

            return bmp;
        }
        public void setIcon(char status)
        {
            if (status == 'F')
            {
                if(ty == "Plane")
                {
                    img = Image.FromFile("Images/color_airport_Frirndly_48.png");
                    icon = Resize(img, (int)(40 * pr), (int)(40 * pr));
                }
                else if(ty == "Nato")
                {
                    img = Image.FromFile("Images/nato/FRD_AIR.png");
                    Console.WriteLine((int)(22 * pr));
                    icon = Resize(img, (int)(22*pr), (int)(24*pr));
                }
                
                Fill = new SolidBrush(Color.FromArgb(255, Color.Green));
            }
            else if (status == 'H')
            {
                if(ty == "Plane")
                {
                    img = Image.FromFile("Images/color_airport_Hostile_48.png");
                    icon = Resize(img, (int)(40 * pr), (int)(40 * pr));
                }
                else if(ty == "Nato")
                {
                    img = Image.FromFile("Images/nato/HOS_AIR.png");
                    icon = Resize(img, (int)(22*pr),(int)(26*pr));
                }
                Fill = new SolidBrush(Color.FromArgb(255, Color.Red));
            }
            else if (status == 'U')
            {
                if(ty == "Plane") 
                {
                    img = Image.FromFile("Images/color_airport_Waiting_48.png");
                    icon = Resize(img, (int)(40 * pr), (int)(40 * pr));
                }
                else if (ty == "Nato")
                {
                    img = Image.FromFile("Images/nato/UNK_AIR.png");
                    icon = Resize(img, (int)(32*pr), (int)(28*pr));
                }
                
                Fill = new SolidBrush(Color.FromArgb(255, Color.YellowGreen));
            }
            else
            {
                if (ty == "Plane")
                {
                    img = Image.FromFile("Images/color_airport_Waiting_48.png");
                    icon = Resize(img, (int)(40 * pr), (int)(40 * pr));
                }
                else if (ty == "Nato")
                {
                    img = Image.FromFile("Images/nato/UNK_AIR.png");
                    icon = Resize(img, (int)(32 * pr), (int)(28 * pr));
                }
                Fill = new SolidBrush(Color.FromArgb(255, Color.Gray));
            }
        }

        public override void OnRender(Graphics g)
        {
            Font _font = new Font("Angsana New", (float)(36*pr), FontStyle.Bold);
            _caption = flight.name;
            var stringSize = g.MeasureString(_caption, _font);
            var localPoint = new PointF((LocalPosition.X+30) - (stringSize.Width / 2), (LocalPosition.Y) + (stringSize.Height / 2));
            if (ty == "Plane")
            {
                double angle = Tailheading * Math.PI / 180;
                GPoint start = new GPoint(LocalPosition.X+7, LocalPosition.Y+12);
                double deltaX = Length * Math.Sin(angle);
                double deltaY = Length * Math.Cos(angle);
                int endX = (int)(start.X + deltaX * 5);
                int endY = (int)(start.Y - deltaY * 5);

                _TailColor.Width = 2.0f;
                g.DrawLine(_TailColor, LocalPosition.X+7, LocalPosition.Y+12, endX, endY);
                g.DrawString(_caption, _font, CaptionColor, localPoint);

                Matrix temp = g.Transform;
                g.TranslateTransform(LocalPosition.X+7, LocalPosition.Y+12);
                g.RotateTransform(-Overlay.Control.Bearing);

                try
                {
                    g.RotateTransform(heading);
                }
                catch { }

                g.DrawImageUnscaled(icon, icon.Width / -2, icon.Height / -2);
                g.Transform = temp;
            }
            else if (ty == "Arrow")
            {
                double angle = Tailheading * Math.PI / 180;
                GPoint start = new GPoint(LocalPosition.X + 7, LocalPosition.Y + 12);
                double deltaX = Length * Math.Sin(angle);
                double deltaY = Length * Math.Cos(angle);
                int endX = (int)(start.X + deltaX * 5);
                int endY = (int)(start.Y - deltaY * 5);

                _TailColor.Width = 2.0f;
                g.DrawLine(_TailColor, (LocalPosition.X + 7), (LocalPosition.Y + 12), endX, endY);
                g.DrawString(_caption, _font, CaptionColor, localPoint);

                Matrix temp = g.Transform;
                g.TranslateTransform(LocalPosition.X+7, LocalPosition.Y+12);
                var c = g.BeginContainer();
                {
                    g.RotateTransform(heading- Overlay.Control.Bearing);
                    g.ScaleTransform(Scale, Scale);

                    g.FillPolygon(Fill, Arrow);
                }
                g.EndContainer(c);
                //g.TranslateTransform(LocalPosition.X+7, LocalPosition.Y+12);
                g.Transform = temp;
            }
            else if (ty == "Nato")
            {
                //TODO nato symbol
                double angle = Tailheading * Math.PI / 180;
                GPoint start = new GPoint(LocalPosition.X+7, LocalPosition.Y+12);
                double deltaX = Length * Math.Sin(angle);
                double deltaY = Length * Math.Cos(angle);
                int endX = (int)(start.X + deltaX * 5);
                int endY = (int)(start.Y - deltaY * 5);

                g.DrawString(_caption, _font, CaptionColor, localPoint);

                Matrix temp = g.Transform;
                g.TranslateTransform(LocalPosition.X+7, LocalPosition.Y+12);
                g.RotateTransform(-Overlay.Control.Bearing);

                g.DrawImageUnscaled(icon, icon.Width / -2, icon.Height / -2);
                g.Transform = temp;
            }
        }
    }

}
