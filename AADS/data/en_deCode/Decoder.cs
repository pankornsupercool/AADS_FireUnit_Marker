using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using AADS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    public enum RecievedType
    {
        Radar, Track
    }
    public class RecievedData
    {
        private RecievedType _Type;
        private int _BlockLength;
        private string _Excess;
        public RecievedType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int BlockLength
        {
            get { return _BlockLength; }
            set { _BlockLength = value; }
        }
        public string Excess
        {
            get { return _Excess; }
            set { _Excess = value; }
        }
    }
    public class RecievedRadarData : RecievedData
    {
        private int _Number;
        private double _Latitude;
        private double _Longitude;
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }
        public double Lat
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }
        public double Lng
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }
        public string Encode()
        {
            string hex = Convert.ToString(BlockLength, 16);
            string expanded = "";
            for (int i = 0; i < 4 - hex.Length; i++)
            {
                expanded += "0";
            }
            expanded += hex;
            string encoded = "0202";
            encoded += expanded + "01";
            encoded += "0100000000";
            encoded += Number.ToString("00") + "20";
            encoded += "";
            return encoded;
        }
    }
    public class RecievedTrackType
    {
        public static readonly RecievedTrackType Friendly = new RecievedTrackType("Friendly", 'F');
        public static readonly RecievedTrackType Special = new RecievedTrackType("Special", 'S');
        public static readonly RecievedTrackType Rescue = new RecievedTrackType("Rescue", 'R');
        public static readonly RecievedTrackType Fighter = new RecievedTrackType("Fighter", 'I');
        public static readonly RecievedTrackType Hostile = new RecievedTrackType("Hostile", 'H');
        public static readonly RecievedTrackType Faker = new RecievedTrackType("Faker", 'K');
        public static readonly RecievedTrackType PotentialEnemy = new RecievedTrackType("Potential Enemy", 'E');
        public static readonly RecievedTrackType Pending = new RecievedTrackType("Pending", 'P');
        public static readonly RecievedTrackType Unknown = new RecievedTrackType("Unknown", 'U');
        public static readonly RecievedTrackType NonsignificantUnknown = new RecievedTrackType("Nonsignificant Unknown", 'N');
        public static IEnumerable<RecievedTrackType> Values
        {
            get
            {
                yield return Friendly;
                yield return Special;
                yield return Rescue;
                yield return Fighter;
                yield return Hostile;
                yield return Faker;
                yield return PotentialEnemy;
                yield return Pending;
                yield return Unknown;
                yield return NonsignificantUnknown;
            }
        }
        public static RecievedTrackType FromName(string name)
        {
            foreach (RecievedTrackType type in Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        private string _Name;
        private char _Identification;

        public RecievedTrackType(string Name, char Identification)
        {
            this._Name = Name;
            this._Identification = Identification;
        }

        public string Name
        {
            get { return _Name; }
        }

        public char Identification
        {
            get { return _Identification; }
        }

        //Friendly, Special, Rescue, Fighter, Hostile, Faker, PotentialEnemy, Pending, Unknown, NonsignificantUnknown
    }
    public class RecievedTrackData : RecievedData
    {
        private double _PositionX;
        private double _PositionY;
        private double _SpeedX;
        private double _SpeedY;
        private int _TrackNumber;
        private char _Identification;
        private RecievedTrackType _TrackType;
        private int _RadarNumber;
        public double PositionX
        {
            get { return _PositionX; }
            set { _PositionX = value; }
        }
        public double PositionY
        {
            get { return _PositionY; }
            set { _PositionY = value; }
        }
        public double SpeedX
        {
            get { return _SpeedX; }
            set { _SpeedX = value; }
        }
        public double SpeedY
        {
            get { return _SpeedY; }
            set { _SpeedY = value; }
        }
        public double Speed
        {
            get
            {
                double x = Math.Pow(SpeedX, 2);
                double y = Math.Pow(SpeedY, 2);
                return Math.Sqrt(x + y);
            }
        }
        public double Bearing
        {
            get
            {
                if (SpeedX != 0)
                {
                    double theta = Math.Atan(SpeedY / SpeedX);
                    double angle = Math.Abs(theta) * 180 / Math.PI;
                    if (SpeedX >= 0 && SpeedY >= 0)
                    {
                        return 90 - angle;
                    }
                    else if (SpeedX >= 0 && SpeedY < 0)
                    {
                        return 90 + angle;
                    }
                    else if (SpeedX < 0 && SpeedY >= 0)
                    {
                        return 270 + angle;
                    }
                    else if (SpeedX < 0 && SpeedY < 0)
                    {
                        return 270 - angle;
                    }
                    return 0;
                }
                else
                {
                    return SpeedY >= 0 ? 0 : 180;
                }
            }
        }
        public int TrackNumber
        {
            get { return _TrackNumber; }
            set { _TrackNumber = value; }
        }
        public char Identification
        {
            get { return _Identification; }
            set { _Identification = value; }
        }
        public RecievedTrackType TrackType
        {
            get { return _TrackType; }
            set { _TrackType = value; }
        }
        public int RadarNumber
        {
            get { return _RadarNumber; }
            set { _RadarNumber = value; }
        }
    }
    public class DataDecoder
    {
        protected string _Encoded;
        protected Dictionary<string, RecievedRadarData> _DecodedRadar;
        protected Dictionary<int, RecievedTrackData> PreviousTrack;
        protected Dictionary<int, RecievedTrackData> CurrentTrack;
        protected Dictionary<string, char> _Status;
        protected Dictionary<string, int> _Priority;
        protected RecievedRadarData _FirstRadar;
        private Track track = Track.GetInstance();
        public DataDecoder()
        {
            _DecodedRadar = new Dictionary<string, RecievedRadarData>();
            PreviousTrack = new Dictionary<int, RecievedTrackData>();
            CurrentTrack = new Dictionary<int, RecievedTrackData>();
            _Status = new Dictionary<string, char>();
            _Priority = new Dictionary<string, int>();
            _FirstRadar = null;
            _Data = "";
            _Latest = "";
            _AllDecode = new List<string>();
            _IsTrackDecode = 0;
        }
        public string Encoded
        {
            get { return _Encoded; }
            set { _Encoded = value; }
        }
        public int IsTrackDecode
        {
            get { return _IsTrackDecode; }
            set { _IsTrackDecode = value; }
        }
        public Dictionary<string, RecievedRadarData> Radars
        {
            get
            {
                return _DecodedRadar;
            }
        }
        public string ExpandBinary(int value, int length)
        {
            string base2 = Convert.ToString(value, 2);
            string expanded = "";
            for (int i = 0; i < length - base2.Length; i++)
            {
                expanded += "0";
            }
            return expanded + base2;
        }
        private double FindPositionInDegree(string dms)
        {
            string dmsPath = dms.Substring(0, 4);
            string degPath = dms.Substring(4, 4);
            int dmsBase10 = Convert.ToInt32(dmsPath, 16);
            string dmsBase2 = ExpandBinary(dmsBase10, 16);
            string minutePath = dmsBase2.Substring(0, 6);
            string secondPath = dmsBase2.Substring(6, 6);
            /*
            int multiplier = 1;
            string directionPath = dmsBase2.Substring(12, 4);
            int direction = Convert.ToInt32(directionPath, 2);
            if (direction == 0 || direction == 3) // 0: East, 3: North
            {
                multiplier = 1;
            }
            else if (direction == 1 || direction == 2) // 1: South, 2: West
            {
                multiplier = -1;
            }
            */
            int minutes = Convert.ToInt32(minutePath, 2);
            int seconds = Convert.ToInt32(secondPath, 2);
            int degree = Convert.ToInt32(degPath, 16);
            double dd = degree + ((double)minutes / 60) + ((double)seconds / 3600);
            return dd;
        }
        public static RecievedTrackType ParseStatus(char status)
        {
            if (status == 'F')
                return RecievedTrackType.Friendly;
            else if (status == 'S')
                return RecievedTrackType.Special;
            else if (status == 'R')
                return RecievedTrackType.Rescue;
            else if (status == 'I')
                return RecievedTrackType.Fighter;
            else if (status == 'H')
                return RecievedTrackType.Hostile;
            else if (status == 'K')
                return RecievedTrackType.Faker;
            else if (status == 'E')
                return RecievedTrackType.PotentialEnemy;
            else if (status == 'U')
                return RecievedTrackType.Unknown;
            else if (status == 'N')
                return RecievedTrackType.NonsignificantUnknown;
            return RecievedTrackType.Pending;
        }
        public static char ParseStatus(RecievedTrackType type)
        {
            if (type == RecievedTrackType.Friendly)
                return 'F';
            else if (type == RecievedTrackType.Special)
                return 'S';
            else if (type == RecievedTrackType.Rescue)
                return 'R';
            else if (type == RecievedTrackType.Fighter)
                return 'I';
            else if (type == RecievedTrackType.Hostile)
                return 'H';
            else if (type == RecievedTrackType.Faker)
                return 'K';
            else if (type == RecievedTrackType.PotentialEnemy)
                return 'E';
            else if (type == RecievedTrackType.Unknown)
                return 'U';
            else if (type == RecievedTrackType.NonsignificantUnknown)
                return 'N';
            return 'P';
        }
        public static string GenerateId(RecievedType type, int number)
        {
            string value = type + "-";
            string no = number.ToString("0000");
            value += no;
            return value;
        }
        public static void InsertOrUpdate(Dictionary<int, FlightRadarData> map, int key, FlightRadarData value)
        {
            if (map.ContainsKey(key))
            {
                map[key] = value;
            }
            else
            {
                map.Add(key, value);
            }
        }
        public PointLatLng FindPositionFor(RecievedTrackData track)
        {
            int radarNumber = track.RadarNumber;
            string radarKey = DataDecoder.GenerateId(RecievedType.Radar, radarNumber);
            Dictionary<string, RecievedRadarData> radars = Radars;
            RecievedRadarData radar;
            PointLatLng point = new PointLatLng();
            if (radars.ContainsKey(radarKey))
            {
                radar = radars[radarKey] as RecievedRadarData;
                point.Lat = radar.Lat + track.PositionY;
                point.Lng = radar.Lng + track.PositionX;
            }
            else if (_FirstRadar != null)
            {
                radar = _FirstRadar;
                point.Lat = radar.Lat + track.PositionY;
                point.Lng = radar.Lng + track.PositionX;
            }
            return point;
        }
        public RecievedTrackData GetTrackData(int id)
        {
            return CurrentTrack[id] as RecievedTrackData;
        }
        private PointLatLng RE_radar;
        public bool Decode()
        {
            /*
            _Encoded = _Encoded.Replace("\n", ""); //clear escape string
            string[] delimiters = { "0202" };
            foreach (string code in _Encoded.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
            {
                if (code.Length < 6)
                {
                    return false;
                }
                string result = "0202" + code;
                try
                {
                    int blockLength = Convert.ToInt32(result.Substring(4, 4), 16);
                    string dataType = result.Substring(8, 2);
                    string excess = "";
                    if (dataType.Equals("01"))
                    {
                        //decode data : radar use 20 bytes
                        int no = Convert.ToInt32(result.Substring(20, 2));
                        string latPath = result.Substring(24, 8);
                        string lngPath = result.Substring(32, 8);
                        if (result.Length > 40)
                        {
                            excess = result.Substring(40);
                        }
                        //set radar data
                        RecievedRadarData data = new RecievedRadarData();
                        data.BlockLength = blockLength;
                        data.Type = RecievedType.Radar;
                        data.Number = no;
                        data.Lat = FindPositionInDegree(latPath);
                        data.Lng = FindPositionInDegree(lngPath);
                        data.Excess = excess;
                        string key = GenerateId(data.Type, data.Number);
                        //result message
                        string message = "RADAR, ";
                        message += data.Number.ToString("00") + ", ";
                        message += data.Lat.ToString("0.####") + ", ";
                        message += data.Lng.ToString("0.####");
                        //radar marker
                        PointLatLng point = new PointLatLng(data.Lat, data.Lng);
                        Bitmap pic = (Bitmap)Image.FromFile("Img/point/RS.png");
                        GMapMarker marker = new GMarkerGoogle(point, pic);
                        marker.Tag = "Radar";
                        //store value
                        if (!_Keys.Contains(key))
                        {
                            _Keys.Add(key);
                            _Decoded.Add(key, data);
                            _Result.Add(key, message);
                            _Markers.Add(key, marker);
                        }
                        else
                        {
                            _Decoded[key] = data;
                            _Result[key] = message;
                            _Markers[key] = marker;
                        }
                    }
                    else if (dataType.Equals("02"))
                    {
                        //decode data : track use 18 bytes
                        double xPos = (double)Convert.ToInt16(result.Substring(12, 4), 16) / 480;
                        double yPos = (double)Convert.ToInt16(result.Substring(16, 4), 16) / 480;
                        double xSpeed = (double)Convert.ToInt16(result.Substring(20, 4), 16) / 8;
                        double ySpeed = (double)Convert.ToInt16(result.Substring(24, 4), 16) / 8;
                        int trackNo = Convert.ToInt32(result.Substring(28, 4));
                        //TODO dont change if edited by user
                        char status = (char)Convert.ToInt32(result.Substring(32, 2), 16);
                        int radarNo = Convert.ToInt32(result.Substring(34, 2));
                        MessageBox.Show("dataType = " + dataType + "\nblockLength = " + blockLength + "\n" + "result.Length = " + result.Length);
                        if (result.Length > 36)
                        {
                            excess = result.Substring(36);
                            Regex regex = new Regex(@"(02)(.{26})");
                            MatchCollection matches = regex.Matches(excess);
                            foreach (Match match in matches)
                            {
                                MessageBox.Show(match.Groups[0].Value);
                            }
                        }
                        //set track data
                        RecievedTrackData data = new RecievedTrackData();
                        data.BlockLength = blockLength;
                        data.Type = RecievedType.Track;
                        data.PositionX = xPos;
                        data.PositionY = yPos;
                        data.SpeedX = xSpeed;
                        data.SpeedY = ySpeed;
                        data.TrackNumber = trackNo;
                        data.TrackType = ParseStatus(status);
                        data.RadarNumber = radarNo;
                        data.Excess = excess;
                        string key = GenerateId(data.Type, data.TrackNumber);
                        if (_Status.ContainsKey(key))
                        {
                            data.Identification = _Status[key];
                        }
                        else
                        {
                            data.Identification = status;
                        }
                        PointLatLng point = FindPositionFor(data);
                        double kmph = ScaleConverter.ConvertSpeed(data.Speed, "mph", "km/h");
                        //result message
                        string message = "TRACK, ";
                        message += data.TrackNumber.ToString("0000") + ", ";
                        message += data.Identification + ", ";
                        message += point.Lat.ToString("0.####") + ", ";
                        message += point.Lng.ToString("0.####") + ", ";
                        message += kmph.ToString("0.####") + ", ";
                        message += data.RadarNumber.ToString("00");
                        //track marker
                        GMapMarker marker = new GMarkerArrow(point)
                        {
                            ToolTipText = message,
                            ToolTipMode = MarkerTooltipMode.OnMouseOver
                        };
                        (marker as GMarkerArrow).Bearing = (float)data.Bearing;
                        marker.setFill(data.Identification.ToString());
                        marker.Tag = "Track";
                        //flight tail
                        FlightTail tail = new FlightTail(point, data.Speed, data.Bearing);
                        //store value
                        if (!_Keys.Contains(key))
                        {
                            _Keys.Add(key);
                            _Decoded.Add(key, data);
                            _Result.Add(key, message);
                            _Markers.Add(key, marker);
                            _Tails.Add(key, tail);
                        }
                        else
                        {
                            _Decoded[key] = data;
                            _Result[key] = message;
                            _Markers[key] = marker;
                            _Tails[key] = tail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
            return true;
            */
            _Encoded = _Encoded.Replace("\n", ""); //clear escape string
            _Encoded = _Encoded.Replace("\r", ""); //clear escape string
            _Encoded = _Encoded.Replace("\t", ""); //clear escape string
            string code = _Encoded;
            if (IsTrackDecode == 0)
            {
                //normal data
                while (code.StartsWith("0202", StringComparison.OrdinalIgnoreCase) && IsTrackDecode == 0)
                {
                    string result = code;
                    int blockLength = Convert.ToInt32(result.Substring(4, 4), 16);
                    int dataType = Convert.ToInt32(result.Substring(8, 2), 16);
                    string excess = "";
                    if (dataType == 1)
                    {
                        //decode data : radar use 20 bytes
                        int no = Convert.ToInt32(result.Substring(20, 2), 16);
                        string latPath = result.Substring(24, 8);
                        string lngPath = result.Substring(32, 8);
                        //set radar data
                        RecievedRadarData data = new RecievedRadarData();
                        data.BlockLength = blockLength;
                        data.Type = RecievedType.Radar;
                        data.Number = no;
                        data.Lat = FindPositionInDegree(latPath);
                        data.Lng = FindPositionInDegree(lngPath);
                        data.Excess = excess;
                        string key = GenerateId(data.Type, data.Number);
                        //result message
                        string message = "RADAR, ";
                        message += data.Number.ToString("00") + ", ";
                        message += data.Lat.ToString("0.####") + ", ";
                        message += data.Lng.ToString("0.####");
                        //radar marker
                        /*
                        PointLatLng point = new PointLatLng(data.Lat, data.Lng);
                        Bitmap pic = (Bitmap)Image.FromFile("Img/point/RS.png");
                        GMapMarker marker = new GMarkerGoogle(point, pic);
                        /
                            ToolTipText = message,
                            ToolTipMode = MarkerTooltipMode.OnMouseOver
                        };
                        marker.Tag = "Radar";
                        */
                        RE_radar = new PointLatLng(data.Lat, data.Lng);
                        MainForm main = MainForm.GetInstance();
                        if(main.radarP.Position != RE_radar)
                        {
                            main.radarP.Position = RE_radar;
                        }
                        //store value
                        if (!_DecodedRadar.ContainsKey(key))
                        {
                            _DecodedRadar.Add(key, data);
                        }
                        else
                        {
                            _DecodedRadar[key] = data;
                        }
                        if (_FirstRadar == null)
                        {
                            _FirstRadar = data;
                        }

                        track.ClearReal();
                        CurrentTrack.Clear();
                    }
                    else if (dataType == 2)
                    {
                        //decode data : track use 18 bytes
                        double xPos = (double)Convert.ToInt16(result.Substring(12, 4), 16) / 480;
                        double yPos = (double)Convert.ToInt16(result.Substring(16, 4), 16) / 480;
                        double xSpeed = (double)Convert.ToInt16(result.Substring(20, 4), 16);
                        double ySpeed = (double)Convert.ToInt16(result.Substring(24, 4), 16);
                        int trackNo = Convert.ToInt32(result.Substring(28, 4), 16);
                        char status = (char)Convert.ToInt32(result.Substring(32, 2), 16);
                        int radarNo = Convert.ToInt32(result.Substring(34, 2), 16);

                        track.CompareDict(PreviousTrack, CurrentTrack);
                        IsTrackDecode = blockLength - 18;
                        PreviousTrack = new Dictionary<int, RecievedTrackData>(CurrentTrack);
                        CurrentTrack.Clear();

                        RecievedTrackData data = new RecievedTrackData();
                        data.BlockLength = 14;
                        data.Type = RecievedType.Track;
                        data.PositionX = xPos;
                        data.PositionY = yPos;
                        data.SpeedX = xSpeed;
                        data.SpeedY = ySpeed;
                        data.TrackNumber = trackNo;
                        data.TrackType = ParseStatus(status);
                        data.RadarNumber = radarNo;
                        string key = GenerateId(data.Type, data.TrackNumber);
                        if (_Status.ContainsKey(key))
                        {
                            data.Identification = _Status[key];
                        }
                        else
                        {
                            data.Identification = status;
                        }
                        PointLatLng point = FindPositionFor(data);
                        double kmph = ScaleConverter.ConvertSpeed(data.Speed, "mph", "km/h");
                        //result message
                        string message = "TRACK, ";
                        message += data.TrackNumber.ToString("0000") + ", ";
                        message += data.Identification + ", ";
                        message += point.Lat.ToString("0.####") + ", ";
                        message += point.Lng.ToString("0.####") + ", ";
                        message += kmph.ToString("0.####") + ", ";
                        message += data.RadarNumber.ToString("00");

                        FlightRadarData fd = new FlightRadarData(TrackType.Real); //todo change
                        fd.Id = data.TrackNumber;
                        fd.point = point;
                        fd.speed = data.Speed;
                        fd.bearing = data.Bearing;
                        fd.identification = data.Identification;
                        //fd.lastTime = DateTime.Now;
                        track.AddReal(fd);

                        //store value
                        CurrentTrack.Add(fd.Id, data);
                    }
                    code = code.Substring(result.Length);
                }
            }
            else
            {
                //track data
                while (code.Length >= 28 && IsTrackDecode > 0)
                {
                    string result = code;
                    double xPos = (double)Convert.ToInt16(result.Substring(4, 4), 16) / 480;
                    double yPos = (double)Convert.ToInt16(result.Substring(8, 4), 16) / 480;
                    double xSpeed = (double)Convert.ToInt16(result.Substring(12, 4), 16);
                    double ySpeed = (double)Convert.ToInt16(result.Substring(16, 4), 16);
                    int trackNo = Convert.ToInt32(result.Substring(20, 4), 16);
                    char status = (char)Convert.ToInt32(result.Substring(24, 2), 16);
                    int radarNo = Convert.ToInt32(result.Substring(26, 2), 16);

                    IsTrackDecode -= 14;

                    RecievedTrackData data = new RecievedTrackData();
                    data.BlockLength = 14;
                    data.Type = RecievedType.Track;
                    data.PositionX = xPos;
                    data.PositionY = yPos;
                    data.SpeedX = xSpeed;
                    data.SpeedY = ySpeed;
                    data.TrackNumber = trackNo;
                    data.TrackType = ParseStatus(status);
                    data.RadarNumber = radarNo;
                    string key = GenerateId(data.Type, data.TrackNumber);
                    if (_Status.ContainsKey(key))
                    {
                        data.Identification = _Status[key];
                    }
                    else
                    {
                        data.Identification = status;
                    }
                    PointLatLng point = FindPositionFor(data);
                    double kmph = ScaleConverter.ConvertSpeed(data.Speed, "mph", "km/h");
                    //result message
                    string message = "TRACK, ";
                    message += data.TrackNumber.ToString("0000") + ", ";
                    message += data.Identification + ", ";
                    message += point.Lat.ToString("0.####") + ", ";
                    message += point.Lng.ToString("0.####") + ", ";
                    message += kmph.ToString("0.####") + ", ";
                    message += data.RadarNumber.ToString("00");

                    FlightRadarData fd = new FlightRadarData(TrackType.Real); //todo change
                    fd.Id = data.TrackNumber;
                    fd.point = point;
                    fd.speed = data.Speed;
                    fd.bearing = data.Bearing;
                    fd.identification = data.Identification;
                    //fd.lastTime = DateTime.Now;
                    track.AddReal(fd);

                    //store value
                    CurrentTrack.Add(fd.Id, data);
                    code = code.Substring(28);
                }
            }
            return true;
        }
        private string _Data;
        private string _Latest;
        private List<string> _AllDecode;
        private int _IsTrackDecode;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        public string Latest
        {
            get { return _Latest; }
        }
        public List<string> AllDecode
        {
            get { return _AllDecode; }
        }
        public void AddData(string NewData)
        {
            Data += NewData;
        }
        public void ClearData()
        {
            Data = "";
        }
        private bool IsCompleteCode(string testcode, out int blockLength)
        {
            blockLength = 0;
            int type, checkLength;
            if (IsTrackDecode == 0)
            {
                if (testcode.Length < 10 || !testcode.StartsWith("02"))
                {
                    return false;
                }
                blockLength = Convert.ToInt32(testcode.Substring(4, 4), 16);
                type = Convert.ToInt32(testcode.Substring(8, 2), 16);
                checkLength = type == 2 ? 18 : blockLength;
            }
            else
            {
                checkLength = 14;
            }
            if (testcode.Length < checkLength * 2)
            {
                return false;
            }
            blockLength = checkLength;
            return true;
        }
        private string checkForStartCmd(string testcode)
        {
            Regex regex = new Regex(@"(.*)(02)(02)(\d{4})(01)(.*)");
            Match match = regex.Match(testcode);
            if (match.Success)
            {
                Data = match.Groups[2].Value + match.Groups[3].Value + match.Groups[4].Value + match.Groups[5].Value + match.Groups[6].Value;
                isStartProperly = true;
            }
            return testcode;
        }
        public bool isStartProperly = false;
        public bool TryDecode()
        {
            bool ans = false;
            string[] cls = { "\n", "\t", "\r", " " };
            string testcode = Data;
            string testdata = "";
            foreach (string cl in cls)
            {
                testcode = testcode.Replace(cl, "");
            }
            int blockLength;
            if (!isStartProperly)
            {
                checkForStartCmd(testcode);
            }
            testcode = Data;
            if (isStartProperly)
            {
                foreach (string cl in cls)
                {
                    testcode = testcode.Replace(cl, "");
                }
                while (IsCompleteCode(testcode, out blockLength))
                {
                    //decode
                    testdata = testcode.Substring(blockLength * 2);
                    testcode = testcode.Substring(0, blockLength * 2);
                    _AllDecode.Add(testcode);
                    Encoded = testcode;
                    _Latest = Encoded;
                    Decode();
                    //remove substring
                    testcode = testdata;
                    foreach (string cl in cls)
                    {
                        testcode = testcode.Replace(cl, "");
                    }
                    Data = testcode;
                    ans = true;
                }
            }
            return ans;
        }
        public int GetPriority(string key)
        {
            return _Priority.ContainsKey(key) ? _Priority[key] : 0;
        }
        public void SetPriority(string key, int priority)
        {
            if (_Priority.ContainsKey(key))
            {
                _Priority[key] = priority;
            }
            else
            {
                _Priority.Add(key, priority);
            }
        }
    }
}
