using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EndersEditor
{
    public class ReadPoint
    {
        public Dictionary<string, string> Players = new Dictionary<string, string>();
        public Dictionary<string, string> Enemies = new Dictionary<string, string>();
        public Dictionary<string, string> Miscellaneous = new Dictionary<string, string>();

        public string PlayerFile = "Entities/Player.entity";
        public string EnemyFile = "Entities/Enemies.entity";
        public string MiscFile = "Entities/Mislellaneous.entity";


        public void ReadFile(string filename)
        {
            bool readingName = false;
            bool readingTileNumber = false;

            string stringName = null;

            using (StreamReader reader = new StreamReader(filename))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Name]"))
                    {
                        readingName = true;
                        readingTileNumber = false;
                    }
                    else if (line.Contains("[TileNumber]"))
                    {
                        readingTileNumber = true;
                        readingName = false;
                    }
                    else if (readingName)
                    {
                        stringName = line;
                    }
                    else if (readingTileNumber)
                    {
                        if (Path.GetFileNameWithoutExtension(filename) == "Player")
                        {
                            Players.Add(line, stringName);
                        }
                        else if (Path.GetFileNameWithoutExtension(filename) == "Enemies")
                        {
                            Enemies.Add(line, stringName);
                        }
                        else if (Path.GetFileNameWithoutExtension(filename) == "Mislellaneous")
                        {
                            Miscellaneous.Add(line, stringName);
                        }
                    }
                }
            }
        }

    }
}
