using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AADS
{
    public class GMarkerTrack : GMapMarker, ISerializable
    {
        public TrackData track;
        public static Pen _TailColor = new Pen(Brushes.DeepSkyBlue);
        public bool Tile;
        public string _caption;
        public Point[] Arrow = new Point[] { new Point(-7, 7), new Point(0, -22), new Point(7, 7), new Point(0, 2) };
        public Point[] Box = new Point[] { new Point(-7, 7), new Point(7, 7), new Point(7, -7), new Point(-7, -7) };
        public Brush Fill = new SolidBrush(Color.FromArgb(255, Color.Gray));
        private float scale = 1;

        public static Pen TailColor
        {
            get { return _TailColor; }
            set { _TailColor = value; }
        }

        public GMarkerTrack(TrackData track) : base(track.Position)
        {
            this.track = track;
            Scale = 1;
        }
        public void SetTrack(TrackData track)
        {
            this.track = track;
            Position = track.Position;
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

                Size = new System.Drawing.Size((int)(12 * scale), (int)(12 * scale));
                Offset = new System.Drawing.Point(-Size.Width / 2, (int)(-Size.Height / 1.4));
            }
        }

        private static Brush CaptionColor
        {
            get
            {
                GMapControl map = MainForm.GetInstance().gMap;
                bool provider = map.NegativeMode;
                if (provider == false)
                {
                    return Brushes.Black;
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
                Fill = new SolidBrush(Color.FromArgb(255, Color.Green));
            }
            else if (status == 'H')
            {
                Fill = new SolidBrush(Color.FromArgb(255, Color.Red));
            }
            else if (status == 'U')
            {
                Fill = new SolidBrush(Color.FromArgb(255, Color.YellowGreen));
            }
            else
            {
                Fill = new SolidBrush(Color.FromArgb(255, Color.Gray));
            }
        }
        private double Length
        {
            get
            {
                var speed = track.Speed * 3600;
                if (speed >= 125 && speed <= 300)
                {
                    return 2;
                }
                else if (speed > 300 && speed <= 550)
                {
                    return 3.5;
                }
                else if (speed > 550 && speed <= 2000)
                {
                    return 5.5;
                }
                else if (speed > 2000)
                {
                    return 7;
                }
                else
                {
                    return 0;
                }
            }
        }
        public override void OnRender(Graphics g)
        {
            Font _font = new Font("Angsana New", (float)14, FontStyle.Bold);
            _caption = track.Key;
            var stringSize = g.MeasureString(_caption, _font);
            var localPoint = new PointF((LocalPosition.X + 30) - (stringSize.Width / 2), (LocalPosition.Y) + (stringSize.Height / 2));
            {
                double angle = ((track.Bearing + 180) % 360) * Math.PI / 180;
                GPoint start = new GPoint(LocalPosition.X + 7, LocalPosition.Y + 12);
                double deltaX = Length * Math.Sin(angle);
                double deltaY = Length * Math.Cos(angle);
                GPoint end = new GPoint((int)(start.X + deltaX * 5), (int)(start.Y - deltaY * 5));

                _TailColor.Width = 2.0f;
                g.DrawLine(_TailColor, start.X, start.Y, end.X, end.Y);
                g.DrawString(_caption, _font, CaptionColor, localPoint);

                Matrix temp = g.Transform;
                g.TranslateTransform(LocalPosition.X + 7, LocalPosition.Y + 12);
                var c = g.BeginContainer();
                g.ScaleTransform(Scale, Scale);
                g.RotateTransform((float)track.Bearing - Overlay.Control.Bearing);

                g.FillPolygon(Fill, Arrow);
                g.EndContainer(c);
                //g.TranslateTransform(LocalPosition.X+7, LocalPosition.Y+12);
                g.Transform = temp;
            }
        }
    }

}
