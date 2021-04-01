using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewRadarUX
{
    class Mission
    {

        public static int? MissionID;
        private static bool ShowDialog = false;
        private static bool Contains = false;

        public static int ForceMissionId(string mission_name)
        {
            int mission_id = -1;
            DateTime dt = DateTime.Now;
            MySqlConnection conn = DataSettings.GetConnection();
            using (MySqlCommand cmd = new MySqlCommand(null, conn))
            {
                cmd.CommandText = "SELECT * FROM save_mission WHERE mission_date = @date";
                cmd.Parameters.Add(new MySqlParameter("date", MySqlDbType.Date)
                {
                    Value = dt
                });
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mission_id = reader.GetInt32("mission_id");
                }
                reader.Close();
                if (mission_id == -1)
                {
                    cmd.CommandText = "INSERT INTO save_mission (mission_name, mission_date) " +
                        "VALUES (@name, @date); SELECT LAST_INSERT_ID();";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("name", mission_name);
                    cmd.Parameters.Add(new MySqlParameter("date", MySqlDbType.Date)
                    {
                        Value = dt
                    });
                    mission_id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                MissionID = mission_id;
            }
            return mission_id;
        }
        public static bool IsContains()
        {
            bool r = false;
            if (DataSettings.UseDatabase)
            {
                MySqlConnection conn = DataSettings.GetConnection();
                using (MySqlCommand cmd = new MySqlCommand(null, conn))
                {
                    cmd.CommandText = "SELECT * FROM save_mission WHERE mission_date = @date";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new MySqlParameter("date", MySqlDbType.Date)
                    {
                        Value = DateTime.Now
                    });
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            r = true;
                            Contains = true;
                            MissionID = reader.GetInt32("mission_id");
                        }
                    }
                }
            }
            else
            {
                r = true;
            }
            return r;
        }
        public static int GetMissionId()
        {
            if (MissionID == null)
            {
                if (DataSettings.UseDatabase)
                {
                    ForceMissionId("");
                    return MissionID.Value;
                }
                else
                {
                    MissionID = 0;
                }
            }
            return MissionID.Value;
        }

    }
}
