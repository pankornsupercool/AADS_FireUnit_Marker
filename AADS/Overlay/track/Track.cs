using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    public enum TrackType
    {
        Real, Faker
    }
    public struct SimpleTrackInfo
    {
        public TrackType Type;
        public int ID;
        public SimpleTrackInfo(TrackType Type, int ID)
        {
            this.Type = Type;
            this.ID = ID;
        }
    }
    public class Track
    {
        private static Track Instance;

        public static Track GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Track();
                Instance.Load();
            }
            return Instance;
        }

        public Dictionary<int, FlightRadarData> realTrack = new Dictionary<int, FlightRadarData>();
        public Dictionary<int, FlightRadarData> fakerTrack = new Dictionary<int, FlightRadarData>();
        public Dictionary<int, FlightRadarData> AllTrack = new Dictionary<int, FlightRadarData>();

        private HashSet<int> followTracks = new HashSet<int>();
        private Dictionary<int, char> statusTracks = new Dictionary<int, char>();

        public int mission_id = 0;
        public bool isFollow(int id)
        {
            return followTracks.Contains(id);
        }
        public char getStatus(int id)
        {
            if (ContainsTrack(id))
            {
                FlightRadarData fd = GetAll(id);
                return statusTracks.ContainsKey(id) ? statusTracks[id] : fd.identification;
            }
            else
                return '\0';
        }
        public void setStatus(int id, char status)
        {
            if (statusTracks.ContainsKey(id))
                statusTracks[id] = status;
            else
                statusTracks.Add(id, status);
            if (status == '\0')
                statusTracks.Remove(id);
        }
        public void follow(int id)
        {
            if (!followTracks.Contains(id))
                followTracks.Add(id);
        }
        public void unfollow(int id)
        {
            if (followTracks.Contains(id))
                followTracks.Remove(id);
        }

        public dynamic Real
        {
            get
            {
                return realTrack.Keys;
            }
        }

        public Dictionary<int, FlightRadarData> CloneRealTrack()
        {
            Dictionary<int, FlightRadarData> temp = new Dictionary<int, FlightRadarData>(realTrack);
            return temp;
        }

        public Dictionary<int, FlightRadarData> CloneAllTrack()
        {
            Dictionary<int, FlightRadarData> temp = new Dictionary<int, FlightRadarData>(AllTrack);
            return temp;
        }

        public dynamic Fakers
        {
            get
            {
                return fakerTrack.Keys;
            }
        }

        public void Load()
        {
            if (DataSettings.UseDatabase)
            {
                Dictionary<string, dynamic> connDat = DataSettings.Connection;
                string connStr = string.Format("host={0};user={1};password={2};database={3};Convert Zero Datetime=True;", connDat["Host"], connDat["Username"], connDat["Password"], connDat["Database"]);
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    //faker
                    cmd.CommandText = "SELECT * FROM faker_track";
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("tk_id");
                            char iff = reader.GetChar("tk_iff");
                            double lat = reader.GetDouble("tk_lat");
                            double lng = reader.GetDouble("tk_lng");
                            double dir = reader.GetDouble("tk_dir");
                            double speed = reader.GetDouble("tk_speed");
                            bool auto = reader.GetBoolean("tk_auto");

                            FlightRadarData fd = new FlightRadarData(TrackType.Faker);
                            fd.Id = id;
                            fd.identification = iff;
                            fd.point = new GMap.NET.PointLatLng(lat, lng);
                            fd.bearing = dir;
                            fd.speed = speed;
                            fd.altitude = 0;
                            fd.auto = auto;
                            /*
                            fd.cumulativeRange = range_sum;
                            fd.maxSpeed = speed_max;
                            fd.maxAltitude = altitude_max;
                            fd.startTime = ftime;
                            fd.lastTime = ltime;
                            fd.transactionId = transaction_id;

                            fd.auto = type_srce == 1 ? true : false;
                            */
                            AddFaker(fd);
                        }
                    }
                }
                conn.Close();
            }
        }
        public void Save()
        {
            if (DataSettings.UseDatabase)
            {
                int mission_id = Mission.GetMissionId();
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn)) {
                    //Store real data
                    Dictionary<int, FlightRadarData> CloneReal = new Dictionary<int, FlightRadarData>(realTrack);
                    foreach (int id in CloneReal.Keys)
                    {
                        FlightRadarData fd = CloneReal[id];
                        cmd.CommandText = "INSERT INTO real_track VALUES (@id, @name, @iff, @lat, @lng, @dir, @speed, @mission) " +
                            "ON DUPLICATE KEY UPDATE tk_name = @name, tk_iff = @iff, tk_lat = @lat, tk_lng = @lng, tk_dir = @dir, tk_speed = @speed";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", fd.Id);
                        cmd.Parameters.AddWithValue("name", fd.name);
                        cmd.Parameters.AddWithValue("iff", fd.identification);
                        cmd.Parameters.AddWithValue("lat", fd.point.Lat);
                        cmd.Parameters.AddWithValue("lng", fd.point.Lng);
                        cmd.Parameters.AddWithValue("dir", fd.bearing);
                        cmd.Parameters.AddWithValue("speed", fd.speed);
                        cmd.Parameters.AddWithValue("mission", mission_id);
                        cmd.ExecuteNonQuery();
                    }
                    //Store faker data
                    Dictionary<int, FlightRadarData> CloneFaker = new Dictionary<int, FlightRadarData>(fakerTrack);
                    foreach (int id in CloneFaker.Keys)
                    {
                        FlightRadarData fd = CloneFaker[id];
                        cmd.CommandText = "INSERT INTO faker_track VALUES (@id, @name, @iff, @lat, @lng, @dir, @speed, @auto, @mission) " +
                            "ON DUPLICATE KEY UPDATE tk_name = @name, tk_iff = @iff, tk_lat = @lat, tk_lng = @lng, tk_dir = @dir, tk_speed = @speed, tk_auto = @auto";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", fd.Id);
                        cmd.Parameters.AddWithValue("name", fd.name);
                        cmd.Parameters.AddWithValue("iff", fd.identification);
                        cmd.Parameters.AddWithValue("lat", fd.point.Lat);
                        cmd.Parameters.AddWithValue("lng", fd.point.Lng);
                        cmd.Parameters.AddWithValue("dir", fd.bearing);
                        cmd.Parameters.AddWithValue("speed", fd.speed);
                        cmd.Parameters.AddWithValue("auto", fd.auto);
                        cmd.Parameters.AddWithValue("mission", mission_id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void ClearReal()
        {
            realTrack.Clear();
        }
        public FlightRadarData GetReal(int no)
        {
            if (realTrack.ContainsKey(no))
            {
                return realTrack[no];
            }
            return null;
        }

        public FlightRadarData GetFaker(int id)
        {
            if (fakerTrack.ContainsKey(id))
            {
                return fakerTrack[id];
            }
            return null;
        }

        public FlightRadarData GetAll(int id)
        {
            return AllTrack[id];
        }
        public bool ContainsTrack(int id)
        {
            return AllTrack.ContainsKey(id);
        }
        public FlightRadarData findidTrack(string name)
        {
            int get = -1;
            foreach(int id in AllTrack.Keys)
            {
                if(AllTrack[id].name == name)
                {
                    get = id;
                }
            }
            return AllTrack[get];
        }

        public bool CheckidTrack(string name)
        {
            bool get = false;
            foreach (int id in AllTrack.Keys)
            {
                if (AllTrack[id].name == name)
                {
                    get = true;
                }
            }
            return get;
        }

        public void CompareDict(Dictionary<int, RecievedTrackData> prev, Dictionary<int, RecievedTrackData> curr)
        {
            if (DataSettings.UseDatabase)
            {
                MySqlConnection conn = DataSettings.GetConnection();
                List<int> keys = new List<int>(curr.Keys);
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    foreach (int id in curr.Keys)
                    {
                        RecievedTrackData track = curr[id];
                        FlightRadarData fd = GetReal(track.TrackNumber);
                        cmd.CommandText = "INSERT INTO real_track_point (point_lat, point_lng, real_id) VALUES (@lat, @lng, @id)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("lat", fd.point.Lat);
                        cmd.Parameters.AddWithValue("lng", fd.point.Lng);
                        cmd.Parameters.AddWithValue("id", fd.Id);
                        cmd.ExecuteNonQuery();
                    }
                    if (keys.Count > 0)
                    {
                        cmd.CommandText = "DELETE FROM real_track WHERE tk_id NOT IN (" + string.Join(",", keys) + ")";
                    }
                    else
                    {
                        cmd.CommandText = "DELETE FROM real_track";
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            foreach (int id in prev.Keys)
            {
                if (!curr.ContainsKey(id))
                {
                    RecievedTrackData data = prev[id];
                    RemoveReal(data.TrackNumber);
                }
            }
        }

        public void AddReal(FlightRadarData fd)
        {
            DataDecoder.InsertOrUpdate(realTrack, fd.Id, fd);
            DataDecoder.InsertOrUpdate(AllTrack, fd.Id, fd);
        }

        public void RemoveReal(int no)
        {
            if (realTrack.ContainsKey(no))
            {
                realTrack.Remove(no);
                AllTrack.Remove(no);
            }
        }
        public void AddFaker(FlightRadarData fd)
        {
            DataDecoder.InsertOrUpdate(fakerTrack, fd.Id, fd);
            DataDecoder.InsertOrUpdate(AllTrack, fd.Id, fd);
        }
        public void AddFaker(FlightRadarData fd, MySqlConnection conn)
        {
            AddFaker(fd);
            if (DataSettings.UseDatabase)
            {
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    cmd.CommandText = "INSERT INTO faker_track VALUES (@id, @name, @iff, @lat, @lng, @dir, @speed, @auto, @mission) " +
                                "ON DUPLICATE KEY UPDATE tk_name = @name, tk_iff = @iff, tk_lat = @lat, tk_lng = @lng, tk_dir = @dir, tk_speed = @speed, tk_auto = @auto";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", fd.Id);
                    cmd.Parameters.AddWithValue("name", fd.name);
                    cmd.Parameters.AddWithValue("iff", fd.identification);
                    cmd.Parameters.AddWithValue("lat", fd.point.Lat);
                    cmd.Parameters.AddWithValue("lng", fd.point.Lng);
                    cmd.Parameters.AddWithValue("dir", fd.bearing);
                    cmd.Parameters.AddWithValue("speed", fd.speed);
                    cmd.Parameters.AddWithValue("auto", fd.auto);
                    cmd.Parameters.AddWithValue("mission", mission_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void RemoveFaker(int id)
        {
            if (fakerTrack.ContainsKey(id))
            {
                fakerTrack.Remove(id);
                AllTrack.Remove(id);
            }
        }
        public void RemoveFaker(int id, MySqlConnection conn)
        {
            RemoveFaker(id);
            if (DataSettings.UseDatabase)
            {
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    cmd.CommandText = "DELETE FROM faker_track WHERE tk_id = @id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ChangeFlightData(TrackType type, int index, FlightRadarData tempfd)
        {
            if (type == TrackType.Real)
            {
                realTrack[index] = tempfd;
            }
            else if (type == TrackType.Faker)
            {
                fakerTrack[index] = tempfd;
            }
        }
    }
}
