using GMap.NET.MapProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    class MapMode
    {
        public string Name { get; set; }
        public GMapProvider MapProvider { get; set; }
        public int MainMapMinZoom { get; set; }
        public int MainMapMaxZoom { get; set; }
        public int MiniMapMinZoom { get; set; }
        public int MiniMapMaxZoom { get; set; }
    }
}
