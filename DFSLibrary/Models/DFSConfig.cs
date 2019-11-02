using System;
using System.Collections.Generic;
using System.Text;

namespace DFSLibrary.Models
{
    public class DFSConfig
    {
        public string FileToRead { get; set; }
        public string TopLineupOutputPath { get; set; }
        public int WTNR { get; set; }
        public int MaxSalary { get; set; }
        public int Lineups { get; set; }

        public DFSConfig(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach(string line in lines)
            {
                AddConfig(line.Split(new string[] { "::" }, StringSplitOptions.None));
            }
            
        }

        private void AddConfig(string[] line)
        {
            switch (line[0])
            {
                case "Lineups":
                    Lineups = int.Parse(line[1]);
                    break;
                case "Max Price":
                    MaxSalary = int.Parse(line[1]);
                    break;
                case "Writes til read new":
                    WTNR = int.Parse(line[1]);
                    break;
                case "File to read":
                    FileToRead = line[1];
                    break;
                case "Top Lineup output path":
                    TopLineupOutputPath = line[1];
                    break;
            }
        }
    }
}
