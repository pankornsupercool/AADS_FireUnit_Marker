using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET.WindowsForms;

using GMap.NET.MapProviders;

using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using System.Runtime.Serialization;
using System.Drawing;
using NewRadarUX;

namespace Net_GmapMarkerWithLabel
{
    [Serializable]
    public class GmapMarkerWithLabel : GMarkerGoogle, ISerializable
    {
        public PointF localPoint;
        [NonSerialized]
        public Font _font;
        [NonSerialized]
        public GMarkerGoogle _InnerMarker;
        [NonSerialized]
        public string _caption;
        public int _sizeStr;
        public static Brush _CaptionColor = Brushes.Black;

        public GmapMarkerWithLabel(PointLatLng p, string caption, Bitmap type,int size)
            : base(p, type)
        {
            _sizeStr = size;
            _InnerMarker = new GMarkerGoogle(p, type);
            _caption = caption;
        }

        public static Brush CaptionColor
        {
            get { return _CaptionColor; }
            set { _CaptionColor = value; }
        }

        public override void OnRender(Graphics g)
        {
            base.OnRender(g);
            _font = new Font("Angsana New", _sizeStr);
            var stringSize = g.MeasureString(_caption, _font);
            var localPoint = new PointF(LocalPosition.X - stringSize.Width / 2, LocalPosition.Y + stringSize.Height / 2);
            g.DrawString(_caption, _font, _CaptionColor, localPoint);
        }

        public override void Dispose()
        {
            if (_InnerMarker != null)
            {
                _InnerMarker.Dispose();
                _InnerMarker = null;
            }

            base.Dispose();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            GetObjectData(info, context);
        }

        protected GmapMarkerWithLabel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        #endregion
    }
}