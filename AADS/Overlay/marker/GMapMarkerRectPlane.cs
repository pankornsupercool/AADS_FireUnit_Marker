
namespace NewRadarUX
{
    using System.Drawing;
    using GMap.NET.WindowsForms;
    using GMap.NET.WindowsForms.Markers;
    using GMap.NET;
    using System;
    using System.Runtime.Serialization;
    using System.Drawing.Drawing2D;
    using AADS;

    public class GMapMarkerRectPlane : GMapMarker, ISerializable
    {
        public Pen Pen;

        public GMapMarkerPlane InnerMarker;

        public GMapMarkerRectPlane(PointLatLng p)
         : base(p)
        {
            MainForm main = MainForm.GetInstance();
            GMapControl map = main.GetmainMap();
            double zoom = map.Zoom;
            double pr = ((zoom - 5) * 10) / 100;
            if (pr == 0)
                pr = 1;
            Pen = new Pen(Color.Empty, 3);
            // do not forget set Size of the marker
            // if so, you shall have no event on it ;
            Size = new System.Drawing.Size((int)(40*pr), (int)(40*pr));
            Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height / 2);
        }
        public override void OnRender(Graphics g)
        {
            g.DrawRectangle(Pen, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height));
        }

        public override void Dispose()
        {
            if (Pen != null)
            {
                Pen.Dispose();
                Pen = null;
            }

            if (InnerMarker != null)
            {
                InnerMarker.Dispose();
                InnerMarker = null;
            }

            base.Dispose();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        protected GMapMarkerRectPlane(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
        }

        #endregion
    }
}
