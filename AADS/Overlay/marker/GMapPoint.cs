using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRadarUX
{
    public class GMapPoint : GMap.NET.WindowsForms.GMapMarker
    {
        private PointLatLng point_;
        private float size_;
        public PointLatLng Point
        {
            get
            {
                return point_;
            }
            set
            {
                point_ = value;
            }
        }
        public GMapPoint(PointLatLng p, int size)
            : base(p)
        {
            point_ = p;
            size_ = size;
        }

        public override void OnRender(Graphics g)
        {
            g.FillRectangle(Brushes.White, LocalPosition.X, LocalPosition.Y, size_, size_);
            //OR 
            g.DrawEllipse(Pens.Black, LocalPosition.X, LocalPosition.Y, size_, size_);
            //OR whatever you need

        }
    }
}
