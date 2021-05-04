using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace TheRunner.RunnerTimes
{
    public struct HighTimeOnlineData
    {
        private string playerID;
        private string playerPosition;
        private string playerName;
        private string playerTime;

        public string PlayerID
        {
            get { return playerID; }
            set { playerID = value; }
        }

        public string PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        public string PlayerTime
        {
            get { return playerTime; }
            set { playerTime = value; }
        }
    }

    class HighTimeOnline
    {
        static string fileName = "Content/idNumber.txt";
        static string error = "error.txt";
        static string idNumber;

        public static List<HighTimeOnlineData> OnlineTimeList
        {
            get { return onlineTimeList; }
        }

        public static bool ConnectionIsDead
        {
            get { return connectionIsDead; }
        }
        private static bool connectionIsDead;

        private static List<HighTimeOnlineData> onlineTimeList = new List<HighTimeOnlineData>();


        public static HighTimeOnlineData PlayerData
        {
            get { return playerData ; }
        }
        private static HighTimeOnlineData playerData;

        static string connectionString =
@"Server=tcp:jndw0j1agf.database.windows.net,1433;Database=RunAnnLeaderBoard;User ID=RunAnn@jndw0j1agf;Password= Kinaetron102030;Trusted_Connection= False;Encrypt=True;Connection Timeout=3";

        public static void CreateTable(string levelname)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (checkTableExists(levelname) == true)
                    return;

                try {
                    con.Open();
                }
                catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return;
                }

                connectionIsDead = false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                    "CREATE TABLE " + levelname +
                    " (Id uniqueidentifier DEFAULT NEWID() NOT NULL PRIMARY KEY,Name TEXT, Time Text)", con))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Debug.WriteLine("Could not create table");
                }
            }
        }

        public static void AddUserTime(string levelname ,string name, string time)
        {

            if (File.Exists(fileName) == true)
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    idNumber = sr.ReadToEnd();

                    if (checkRecordExists(levelname) == true)
                    {
                        UpdateUserTime(levelname, name, time);
                    }
                    else
                    {
                        InsertUserTime(levelname, name, time);
                    }
                }
            }
            else if (File.Exists(fileName) == false)
            {
                InsertUserTime(levelname, name, time);
            }
        }

        static bool checkTableExists(string levelname)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try {
                    con.Open();
                }
                catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return false;
                }

                return false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                       "SELECT COUNT(*) FROM " + levelname, con))
                    {
                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                            return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }


        static bool checkRecordExists(string levelname)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try {
                    con.Open();
                }
                catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return false;
                }

                connectionIsDead = false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) FROM " + levelname + " WHERE Id = @Id", con))
                    {
                        command.Parameters.AddWithValue("@Id", idNumber);
                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                            return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        static void InsertUserTime(string levelname, string name, string time)
        {
            if (File.Exists(fileName) == true)
            {

                using (StreamReader sr = new StreamReader(fileName))
                {
                    idNumber = sr.ReadToEnd();
                }

                InsertUserTimeWithID(levelname, name, time);
            }
            else
            {
                InsertUserTimeWithoutID(levelname, name, time);
            }
        }

        static void InsertUserTimeWithID(string levelname, string name, string time)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try{
                    con.Open();
                }
                catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return;
                }

                connectionIsDead = false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                          "INSERT INTO " + levelname +
                          " (Id ,Name, Time) VALUES(@Id, @Name, @Time)", con))
                    {
                        command.Parameters.Add(new SqlParameter("Id", idNumber));
                        command.Parameters.Add(new SqlParameter("Name", name));
                        command.Parameters.Add(new SqlParameter("Time", time));
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Debug.WriteLine("Could not insert new time");
                }
            }
        }

        static void InsertUserTimeWithoutID(string levelname, string name, string time) 
        {
            string id = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try {
                    con.Open();
                }
                catch (SqlException ex){
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return;
                }

                connectionIsDead = false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                          "INSERT INTO " + levelname +
                          " (Name, Time) OUTPUT INSERTED.Id VALUES(@Name, @Time)", con))
                    {
                        command.Parameters.Add(new SqlParameter("Name", name));
                        command.Parameters.Add(new SqlParameter("Time", time));
                        id = command.ExecuteScalar().ToString();
                        //command.ExecuteNonQuery();
                    }

                    System.IO.File.WriteAllText(@fileName, id);
                }
                catch
                {
                    Debug.WriteLine("Could not insert new time");
                }
            }
        }

        static void UpdateUserTime(string levelname,string name, string time)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try {
                    con.Open();
                }
               catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return;
                }

                connectionIsDead = false;

                try
                {
                    using (SqlCommand command = new SqlCommand(
                          "UPDATE " + levelname +  
                          " SET Name = @Name, Time = @Time WHERE Id = @Id", con))
                    {
                        command.Parameters.AddWithValue("@Id", idNumber);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Time", time);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Debug.WriteLine("Could not update record");
                }
            }
        }

        public static void ReadOnlineTable(string levelname)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try {
                    con.Open();
                }
                catch (SqlException ex) {
                    connectionIsDead = true;
                    System.IO.File.WriteAllText(error, ex.ToString());
                    return;
                }

                onlineTimeList.Clear();
                connectionIsDead = false;
                try
                {
                    using (SqlCommand command = new SqlCommand("SELECT * FROM " + levelname, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            HighTimeOnlineData data = new HighTimeOnlineData();

                            data.PlayerID = reader.GetSqlGuid(0).ToString().ToLower();
                            data.PlayerName = reader.GetString(1);
                            data.PlayerTime = reader.GetString(2);
                            onlineTimeList.Add(data);
                        }
                    }

                    onlineTimeList.Sort((s1, s2) => s1.PlayerTime.CompareTo(s2.PlayerTime));
                    List<HighTimeOnlineData> tempHoldList = new List<HighTimeOnlineData>();

                    for (int i = 0; i < onlineTimeList.Count; i++) {
                        HighTimeOnlineData tempHold = new HighTimeOnlineData();

                        int playerPos = i + 1;

                        tempHold.PlayerID = onlineTimeList[i].PlayerID;
                        tempHold.PlayerName = onlineTimeList[i].PlayerName;
                        tempHold.PlayerTime = onlineTimeList[i].PlayerTime;
                        tempHold.PlayerPosition = playerPos.ToString();

                        if (tempHold.PlayerID == idNumber) {
                            playerData.PlayerID = tempHold.PlayerID;
                            playerData.PlayerName = tempHold.PlayerName;
                            playerData.PlayerTime = tempHold.PlayerTime;
                            playerData.PlayerPosition = tempHold.PlayerPosition;
                        }

                        tempHoldList.Add(tempHold);
                    }

                    onlineTimeList = tempHoldList;
                }
                catch
                {
                    Debug.WriteLine("Could not retrieve online data");
                }
            }
        }
    }
}