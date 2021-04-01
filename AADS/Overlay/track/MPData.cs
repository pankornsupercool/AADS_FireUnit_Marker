using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewRadarUX
{
    public abstract class AbstractDataInfo
    {
        private int mission;
        private int id;
        private string name;
        protected AbstractDataInfo(int mission)
        {
            this.mission = mission;
        }
        public int Mission
        {
            get { return mission; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    public class SymbolInfo : AbstractDataInfo, ICloneable
    {
        private int _ImgIndex;
        private int _ImgID;
        private PointLatLng _Point;
        private GMapMarker _Marker;
        public SymbolInfo(int mission, int id, string name) : base(mission)
        {
            base.ID = id;
            base.Name = name;
        }
        public int ImgIndex
        {
            get { return _ImgIndex; }
            set { _ImgIndex = value; }
        }
        public int ImgID
        {
            get { return _ImgID; }
            set { _ImgID = value; }
        }
        public PointLatLng Point
        {
            get { return _Point; }
            set { _Point = value; }
        }
        public GMapMarker Marker
        {
            get { return _Marker; }
            set { _Marker = value; }
        }
        public object Clone()
        {
            SymbolInfo clone = new SymbolInfo(base.Mission, base.ID, base.Name);
            clone._ImgIndex = ImgIndex;
            clone._ImgID = ImgID;
            clone._Point = Point;
            clone._Marker = Marker;
            return clone;
        }
    }
    public class AreaInfo : AbstractDataInfo, ICloneable
    {
        private int _Property;
        private Dictionary<int, PointLatLng> _Points;
        private GMapPolygon _Polygon;
        public AreaInfo(int mission, int id, string name, int property) : base(mission)
        {
            base.ID = id;
            base.Name = name;
            this._Property = property;
        }
        public int Property
        {
            get { return _Property; }
            set { _Property = value; }
        }
        public Dictionary<int, PointLatLng> Points
        {
            get { return _Points; }
            set { _Points = value; }
        }
        public List<PointLatLng> GetPoints()
        {
            return new List<PointLatLng>(_Points.Values);
        }
        public void SetPoints(Dictionary<int, PointLatLng> points)
        {
            this._Points = new Dictionary<int, PointLatLng>(points);
        }
        public GMapPolygon Polygon
        {
            get { return _Polygon; }
            set { _Polygon = value; }
        }
        public object Clone()
        {
            AreaInfo ainfo = new AreaInfo(base.Mission, base.ID, base.Name, _Property);
            ainfo._Points = new Dictionary<int, PointLatLng>(_Points);
            ainfo._Polygon = _Polygon;
            return ainfo;
        }
    }
}
