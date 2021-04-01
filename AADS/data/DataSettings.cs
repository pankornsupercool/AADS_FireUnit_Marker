using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    enum SerialPortType
    {
        Recieving, Sending
    }
    class DataSettings
    {
        /*
        public class DataBase
        {
            public static bool useDataBase = false;
            public static string DataBaseHost = "localhost";
            public static string DataBaseUser = "root"; //"adminRadar";
            public static string DataBasePassWord = "password"; //"AdminPK23710";
            public static string DataBaseName = "radar_info";
        }

        public class Tail
        {
            public static bool TailSY = false;
        }

        public class PositionAndScale
        {
            public static int point = 0;
            public static int Altitude = 2;
            public static int spee = 1;
            public static int Bearing = 0;
            public static bool CheckboxScaleQuick = false;
        }
        public class RadiusSet
        {
            public static bool SwitchSetRadius = false;
            public static int Radius1 = 0;
            public static int Radius2 = 0;
            public static int Radius3 = 0;
            public static int Radius4 = 0;
        }
        */

        public DataSettings()
        {

        }

        public static FlightRadarData dataFormTrack = null;
        private MySqlConnection connection;
        private static DataSettings Instance;

        public static DataSettings GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataSettings();
            }
            Instance.LoadOrDefault();
            Instance.ToggleConnection();
            return Instance;
        }
        public static MySqlConnection GetConnection()
        {
            return Instance.SqlConnection;
        }
        public Dictionary<string, dynamic> Settings = new Dictionary<string, dynamic>();

        public Dictionary<string, dynamic> ParseObject(Dictionary<string, dynamic> map)
        {
            Dictionary<string, dynamic> newmap = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> clonemap = new Dictionary<string, dynamic>(map);
            foreach (string key in clonemap.Keys)
            {
                dynamic obj = clonemap[key];
                if (obj is JObject)
                {
                    JObject jobj = obj as JObject;
                    Dictionary<string, dynamic> submap = jobj.ToObject<Dictionary<string, dynamic>>();
                    newmap.Add(key, ParseObject(submap));
                }
                else
                {
                    newmap.Add(key, obj);
                }
            }
            return newmap;
            /*
            dynamic obj;
            Dictionary<string, dynamic> NewSettings = new Dictionary<string, dynamic>();
            try
            {
                foreach (string key in Settings.Keys)
                {
                    obj = Settings[key];
                    if (obj is JObject)
                    {
                        JObject jobj = obj;
                        NewSettings.Add(key, jobj.ToObject<Dictionary<string, dynamic>>());
                    }
                    else
                    {
                        NewSettings.Add(key, obj);
                    }
                }
            }
            finally
            {
            }
            return NewSettings;
            */
        }

        public static string ConnectionString
        {
            get
            {
                var conn = Connection;
                string connStr = string.Format("host={0};user={1};password={2};database={3}", conn["Host"], conn["Username"], conn["Password"], conn["Database"]);
                return connStr;
            }
        }
        public void LoadOrDefault()
        {
            Settings.Clear();
            string FileName = @"Settings.json";
            JsonSerializer serializer = new JsonSerializer();
            //Database connection
            Dictionary<string, dynamic> connections = new Dictionary<string, dynamic>();
            connections.Add("Host", "localhost");
            connections.Add("Username", "root");
            connections.Add("Password", "password");
            connections.Add("Database", "radar_info");

            //Track
            Dictionary<string, dynamic> track = new Dictionary<string, dynamic>();
            track.Add("PlaneSY", 0);
            track.Add("TailSY", false);
            track.Add("MaxSpeed", 2000.0);

            //Position and scale
            Dictionary<string, dynamic> positionAndScale = new Dictionary<string, dynamic>();
            positionAndScale.Add("Point", 0);
            positionAndScale.Add("Altitude", 2);
            positionAndScale.Add("Speed", 1);
            positionAndScale.Add("Bearing", 0);
            positionAndScale.Add("QuickScale", false);

            //Radar interval
            Dictionary<string, dynamic> radarInterval = new Dictionary<string, dynamic>();
            radarInterval.Add("Radius1", 0);
            radarInterval.Add("Radius2", 0);
            radarInterval.Add("Radius3", 0);
            radarInterval.Add("Radius4", 0);

            //Serial port recieving
            Dictionary<string, dynamic> recievingSerialPort = new Dictionary<string, dynamic>();
            recievingSerialPort.Add("Enable", false);
            recievingSerialPort.Add("Port", "");
            recievingSerialPort.Add("BitPerSecond", 9600);
            recievingSerialPort.Add("DataBits", 8);
            recievingSerialPort.Add("Parity", "None");
            recievingSerialPort.Add("StopBits", "One");
            recievingSerialPort.Add("FlowControl", "None");

            //Serial port sending
            Dictionary<string, dynamic> sendingSerialPort = new Dictionary<string, dynamic>();
            sendingSerialPort.Add("Enable", false);
            sendingSerialPort.Add("Port", "");
            sendingSerialPort.Add("BitPerSecond", 9600);
            sendingSerialPort.Add("DataBits", 8);
            sendingSerialPort.Add("Parity", "None");
            sendingSerialPort.Add("StopBits", "One");
            sendingSerialPort.Add("FlowControl", "None");

            //Serial port
            Dictionary<string, dynamic> serialPort = new Dictionary<string, dynamic>();
            serialPort.Add("Recieving", recievingSerialPort);
            serialPort.Add("Sending", sendingSerialPort);

            if (File.Exists(FileName))
            {
                using (StreamReader reader = File.OpenText(FileName))
                {
                    JsonReader jsonReader = new JsonTextReader(reader);
                    Settings = serializer.Deserialize<Dictionary<string, dynamic>>(jsonReader);
                    jsonReader.Close();
                }
                Settings = ParseObject(Settings);

                if (!Settings.ContainsKey("UseDatabase"))
                {
                    Settings.Add("UseDatabase", false);
                }
                if (!Settings.ContainsKey("Connection"))
                {
                    Settings.Add("Connection", connections);
                }
                if (!Settings.ContainsKey("Track"))
                {
                    Settings.Add("Track", track);
                }
                if (!Settings.ContainsKey("PositionAndScale"))
                {
                    Settings.Add("PositionAndScale", positionAndScale);
                }
                if (!Settings.ContainsKey("EnableRadarInterval"))
                {
                    Settings.Add("EnableRadarInterval", false);
                }
                if (!Settings.ContainsKey("RadarInterval"))
                {
                    Settings.Add("RadarInterval", radarInterval);
                }
                if (!Settings.ContainsKey("SerialPort"))
                {
                    Settings.Add("SerialPort", serialPort);
                }
            }
            else
            {
                Settings.Add("UseDatabase", false);
                Settings.Add("Connection", connections);
                Settings.Add("Track", track);
                Settings.Add("PositionAndScale", positionAndScale);
                Settings.Add("EnableRadarInterval", false);
                Settings.Add("RadarInterval", radarInterval);
                Settings.Add("SerialPort", serialPort);

                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    JsonWriter jsonWriter = new JsonTextWriter(writer)
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(jsonWriter, Settings);
                    jsonWriter.Close();
                }
            }
        }

        public void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            string FileName = @"Settings.json";
            using (StreamWriter writer = new StreamWriter(FileName))
            {
                JsonWriter jsonWriter = new JsonTextWriter(writer)
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(jsonWriter, Settings);
                jsonWriter.Close();
            }
        }
        public MySqlConnection SqlConnection
        {
            get { return connection; }
        }
        public void ToggleConnection()
        {
            if (UseDatabase)
            {
                try
                {
                    if (connection != null && connection.State == System.Data.ConnectionState.Open) connection.Close();
                    connection = new MySqlConnection(ConnectionString);
                    connection.Open();
                }
                catch (Exception)
                {
                    UseDatabase = false;
                    MessageBox.Show("ไม่สามารถเชื่อมต่อฐานข้อมูลได้");
                }
            }
            else
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open) connection.Close();
            }
        }
        public static bool UseDatabase
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["UseDatabase"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["UseDatabase"] = value;
                Instance.Save();
            }
        }

        public static Dictionary<string, dynamic> Connection
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["Connection"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["Connection"] = value;
                Instance.Save();
            }
        }

        public static Dictionary<string, dynamic> Track
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["Track"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["Track"] = value;
                Instance.Save();
            }
        }

        public static Dictionary<string, dynamic> PositionAndScale
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["PositionAndScale"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["PositionAndScale"] = value;
                Instance.Save();
            }
        }
        public static string ParseScale(string scale)
        {
            var pas = PositionAndScale;
            if (scale == "Bearing")
            {
                if (pas[scale] == 0)
                {
                    return "deg";
                }
                else if (pas[scale] == 1)
                {
                    return "rad";
                }
                else if (pas[scale] == 2)
                {
                    return "mil";
                }
            }
            else if (scale == "Speed")
            {
                if (pas[scale] == 0)
                {
                    return "m/s";
                }
                else if (pas[scale] == 1)
                {
                    return "km/h";
                }
                else if (pas[scale] == 2)
                {
                    return "kts";
                }
                else if (pas[scale] == 3)
                {
                    return "mph";
                }
            }
            else if (scale == "Altitude")
            {
                if (pas[scale] == 0)
                {
                    return "m";
                }
                else if (pas[scale] == 1)
                {
                    return "km";
                }
                else if (pas[scale] == 2)
                {
                    return "ft";
                }
                else if (pas[scale] == 3)
                {
                    return "kft";
                }
                else if (pas[scale] == 4)
                {
                    return "FL";
                }
            }
            else if (scale == "Point")
            {
                if (pas[scale] == 0)
                {
                    return "Signed Degree";
                }
                else if (pas[scale] == 1)
                {
                    return "Lat/Lon d°";
                }
                else if (pas[scale] == 2)
                {
                    return "Lat/Lon dms";
                }
                else if (pas[scale] == 3)
                    return "UTM";
                else if (pas[scale] == 4)
                {
                    return "MGRS";
                }
                else if (pas[scale] == 5)
                {
                    return "GEOREF";
                }
            }
            return null;
        }
        public static bool EnableRadarInterval
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["EnableRadarInterval"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["EnableRadarInterval"] = value;
                Instance.Save();
            }
        }

        public static Dictionary<string, dynamic> RadarInterval
        {
            get
            {
                if (Instance == null) Instance = GetInstance();
                Instance.LoadOrDefault();
                return Instance.Settings["RadarInterval"];
            }
            set
            {
                if (Instance == null) Instance = GetInstance();
                if (Instance == null) Instance = GetInstance();
                Instance.Settings["RadarInterval"] = value;
                Instance.Save();
            }
        }

        public static void SetSerialPort(SerialPortType type, bool open)
        {
            if (Instance == null)
            {
                Instance = GetInstance();
            }
            if (type == SerialPortType.Recieving)
            {
                Instance.Settings["SerialPort"]["Recieving"]["Enable"] = open;
            }
            else if (type == SerialPortType.Sending)
            {
                Instance.Settings["SerialPort"]["Sending"]["Enable"] = open;
            }
            Instance.Save();
        }

        public static Dictionary<string, dynamic> GetSerialPortData(SerialPortType type)
        {
            if (Instance == null)
            {
                Instance = GetInstance();
            }
            Instance.LoadOrDefault();
            if (type == SerialPortType.Recieving)
            {
                return Instance.Settings["SerialPort"]["Recieving"];
            }
            else if (type == SerialPortType.Sending)
            {
                return Instance.Settings["SerialPort"]["Sending"];
            }
            return null;
        }

        public static void SetSerialPortData(SerialPortType type, Dictionary<string, dynamic> data)
        {
            if (Instance == null)
            {
                Instance = GetInstance();
            }
            if (type == SerialPortType.Recieving)
            {
                Instance.Settings["SerialPort"]["Recieving"] = data;
            }
            else if (type == SerialPortType.Sending)
            {
                Instance.Settings["SerialPort"]["Sending"] = data;
            }
            Instance.Save();
        }
    }
}
