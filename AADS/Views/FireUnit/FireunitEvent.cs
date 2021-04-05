using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    public delegate void FireunitChanged(FireunitEventArgs e);
    public delegate void FireunitCganged2(FireunitEventArgs2 e);
    public class FireunitEventArgs : EventArgs
    {
        public string LatLng { get; set; }
    }
    public class FireunitEventArgs2 : EventArgs
    {
        public string Location { get; set; }
        public string Detail { get; set; }

    }
    public class FireunitEvent
    {
        private static FireunitEvent _Instance;
        public static FireunitEvent Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new FireunitEvent();
                }
                return _Instance;
            }
        }


        public event FireunitChanged onFireunitChanged;
        public event FireunitCganged2 onFireunitChanged2;
        public void InvokeFireunitChanged(FireunitEventArgs args)
        {
            onFireunitChanged.Invoke(args);
        }
        public void InvokeFireunitChanged2(FireunitEventArgs2 args2)
        {
            onFireunitChanged2.Invoke(args2);
        }
    }
}
