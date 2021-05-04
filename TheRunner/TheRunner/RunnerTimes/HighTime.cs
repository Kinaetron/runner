using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TheRunner.RunnerTimes
{
    public struct HighTimeData
    {
        private string playerPosition;
        private string playerName;
        private string playerTime;

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

    public class HighTime
    {
        private static List<HighTimeData> timeList = new List<HighTimeData>();
        private static HighTimeData playerData;
        private const int amount = 50;

        public static List<HighTimeData> TimeList
        {
            get { return timeList; }
        }

        public static HighTimeData PlayerData
        {
            get { return playerData; }
        }

        public static void SaveData(string filename, HighTimeData data)
        {
           string pathString = "Content/Leaderboard";

            if (File.Exists(pathString) == false) {
                System.IO.Directory.CreateDirectory(pathString);
            }

            timeList.Clear();
            string fullname = filename + ".dat";

            pathString = System.IO.Path.Combine(pathString, fullname);

            playerData.PlayerName = data.PlayerName;

            HighTimeOnline.CreateTable(filename);

            XmlDocument doc = new XmlDocument();

            XmlElement rootElement = doc.CreateElement("TimeBoard");
            doc.AppendChild(rootElement);

            LoadData(fullname);
            timeList.Add(data);

            timeList.Sort((s1, s2) => s1.PlayerTime.CompareTo(s2.PlayerTime));
            List<HighTimeData> list = timeList.Distinct().ToList();


            HighTimeOnline.AddUserTime(filename, list[0].PlayerName, list[0].PlayerTime);


            if (list.Count >= amount)
            {
                int max = list.Count;
                list.RemoveRange(amount, max - amount);
            }

            for (int i = 0; i < list.Count; i++)
            {
                int position = i + 1;

                XmlElement pElement = doc.CreateElement("PlayerDetails");

                XmlAttribute positionAttr = doc.CreateAttribute("PlayerPosition");
                positionAttr.Value = position.ToString();

                XmlAttribute nameAttr = doc.CreateAttribute("PlayerName");
                nameAttr.Value = list[i].PlayerName;

                XmlAttribute timeAttr = doc.CreateAttribute("PlayerTime");
                timeAttr.Value = list[i].PlayerTime;

                pElement.Attributes.Append(positionAttr);
                pElement.Attributes.Append(nameAttr);
                pElement.Attributes.Append(timeAttr);

                rootElement.AppendChild(pElement);
            }
            doc.Save(pathString);
        }

         public static void LoadData(string filename)
         {
             string fullname = string.Empty;

             if (filename.Contains('.') == true) {
                 fullname = filename;
             }
             else {
                 fullname = filename + ".dat";
             }

             string pathString = "Content/Leaderboard";
             fullname = System.IO.Path.Combine(pathString, fullname);

             XmlDocument doc = new XmlDocument();

             if (File.Exists(fullname) == false) {
                 return;
             }
                
             if (timeList.Count > 0) {
                 timeList.Clear();
             }

             doc.Load(fullname);

             foreach (XmlNode node in doc.DocumentElement.ChildNodes)
             {
                 if (node.Name == "PlayerDetails")
                 {
                     HighTimeData data = new HighTimeData();

                     data.PlayerPosition = node.Attributes["PlayerPosition"].Value;
                     data.PlayerName = node.Attributes["PlayerName"].Value;
                     data.PlayerTime = node.Attributes["PlayerTime"].Value;
                     timeList.Add(data);
                 }
             }
         }
    }
}


