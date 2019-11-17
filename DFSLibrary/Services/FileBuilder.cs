using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using DFSLibrary.Models;

namespace DFSLibrary.Services
{
    public class FileBuilder
    {
        public static List<BasketballPlayer> BuildNBAPlayers(string filepath)
        {
            List<BasketballPlayer> rtnList = new List<BasketballPlayer>();

            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                rtnList = csv.GetRecords<BasketballPlayer>().ToList();
            }
            return rtnList;
        }

        public static List<FootballPlayer> BuildNFLPlayers(string filepath) 
        {
            var rtnList = new List<FootballPlayer>();

            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                rtnList = csv.GetRecords<FootballPlayer>().ToList();
            }
            return rtnList;

        } 

        public static void BuildCSV(List<BasketballPlayer> players, string filepath)
        {
            List<CSVHelper> playerHelper = new List<CSVHelper>();
            foreach(var player in players)
            {
                player.SetPlayer();
                playerHelper.Add(new CSVHelper()
                {
                    Name = player.Name,
                    Position = player.Position,
                    Team = player.Team,
                    Salary = player.Salary,
                    Projected = player.Projected,
                    PricePerPoint = player.PricePerPoint
                });
            }

            playerHelper = playerHelper.OrderByDescending(p => p.Projected).ToList();

            using (var writer = new StreamWriter(filepath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(playerHelper);
            }

            string[] jsonFpArr = filepath.Split('\\');
            jsonFpArr[jsonFpArr.Length - 1] = "player_list.json";
            string jfp = String.Join("\\", jsonFpArr);
            BuildPlayerListJson(players, jfp);
        }

        public static void BuildNFLCSV(List<FootballPlayer> players, string filepath)
        {
            players = players.OrderBy(p => p.Position).ThenByDescending(s => s.Salary).ThenByDescending(p => p.Projected).ToList();

            using (var writer = new StreamWriter(filepath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(players);
            }


            string[] jsonFpArr = filepath.Split('\\');
            jsonFpArr[jsonFpArr.Length - 1] = "player_list.json";
            string jfp = String.Join("\\", jsonFpArr);

            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(players);
            File.WriteAllText(jfp, jsonStr);
        }

        public static void CSVCleaner(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            List<string> newLines = new List<string>();
            foreach(var line in lines)
            {
                if (!line.Contains("#N/A"))
                {
                    newLines.Add(line);
                }
            }

            File.WriteAllLines(filepath, newLines);
        }

        public static void WriteTopLineups(List<NBALineup> topLineups, string topLineupOutputPath)
        {
            string topLineupString = "";
            for(int i = 0; i < topLineups.Count; i++)
            {
                topLineupString += BuildNBALineupString(topLineups[i], i + 1);
            }
            File.WriteAllLines(topLineupOutputPath, topLineupString.Split('\n'));


            string[] jsonFpArr = topLineupOutputPath.Split('\\');
            jsonFpArr[jsonFpArr.Length - 1] = "toplineupsjson.json";
            string jfp = String.Join("\\", jsonFpArr);
            BuildTopLineupListJson(topLineups, jfp);
        }

        public static void WriteTopNFLLineups(List<NFLLineup> topLineups, DFSConfig config)
        {
            string topLineupString = "";
            for (int i = 0; i < topLineups.Count; i++)
            {
                topLineupString += BuildNFLLineupString(topLineups[i], i + 1);
            }
            File.WriteAllLines(config.TopLineupOutputPath, topLineupString.Split('\n'));

            BuildTopNFLLineupListJson(topLineups, config.TopLineupJSONPath);

            BuildTopNFLCSV(topLineups, config.TopLineupCSVPath);

        }
        
        public static string BuildNBALineupString(NBALineup lineup, int position)
        {
            string rtnString = $"-----------{position}-----------\n" +
                                $"{lineup.PointGuard1.TopLineupString}\n" +
                                $"{lineup.PointGuard2.TopLineupString}\n" +
                                $"{lineup.ShootingGuard1.TopLineupString}\n" +
                                $"{lineup.ShootingGuard2.TopLineupString}\n" +
                                $"{lineup.SmallForward1.TopLineupString}\n" +
                                $"{lineup.SmallForward2.TopLineupString}\n" +
                                $"{lineup.PowerForward1.TopLineupString}\n" +
                                $"{lineup.PowerForward2.TopLineupString}\n" +
                                $"{lineup.Center.TopLineupString}\n" +
                                $"\nCost: {lineup.Price}\n" +
                                $"Projected Total: {lineup.Score.ToString("0.####")}\n\n";

            return rtnString;
        }

        public static string BuildNFLLineupString(NFLLineup lineup, int position)
        {
            string rtnString = $"-----------{position}-----------\n" +
                                $"{lineup.Quarterback.TopLineupString}" +
                                $"{string.Join("", lineup.RunningBacks.Select(r => r.TopLineupString).ToList())}" +
                                $"{string.Join("", lineup.WideReceivers.Select(w => w.TopLineupString).ToList())}" +
                                $"{lineup.TightEnd.TopLineupString}" +
                                $"{lineup.FLEXPlayer.TopLineupString}" +
                                $"\nCost: {lineup.Price}\n" +
                                $"Defense Budget: {lineup.DefenseBudget}\n" +
                                $"Projected Total: {lineup.Score.ToString("0.####")}\n\n";

            return rtnString;
        }

        public static void BuildPlayerListJson(List<BasketballPlayer> players, string filepath)
        {
            players = players.OrderBy(a => a.PlayerPosition).ThenByDescending(p => p.PlayerSalary).ThenByDescending(s => s.PlayerProjected).ToList();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(players);
            File.WriteAllText(filepath, json);
        }

        public static void BuildTopLineupListJson(List<NBALineup> players, string filepath)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(players);
            File.WriteAllText(filepath, json);
        }

        public static void BuildTopNFLLineupListJson(List<NFLLineup> players, string filepath)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(players);
            File.WriteAllText(filepath, json);
        }
        private static void BuildTopNFLCSV(List<NFLLineup> lineups, string filepath)
        {
            File.WriteAllLines(filepath, lineups.Select(a => a.CSVString).ToList());
        }
        public static void BuildConfig(string lineups, string maxPrice, string wtnr, string basepath, string date, string defenseBudget = null)
        {
            string txtFileToWrite = "dfs-nba-config.txt";
            List<string> lines = new List<string>()
            {
                $"Lineups::{lineups}",
                $"Max Price::{maxPrice}",
                $"Writes til read new::{wtnr}",
                $"File to read::{Path.Combine(basepath, date, "Player List", "nba.csv")}",
                $"Top Lineup output path::{Path.Combine(basepath, date, "Lineups", "top_lineups.txt")}"
            };

            if (!String.IsNullOrEmpty(defenseBudget))
            {
                lines.Add($"DefenseBudget::{defenseBudget}");
                txtFileToWrite = "dfs-nfl-config.txt";
            }

            File.WriteAllLines(Path.Combine(basepath, txtFileToWrite), lines);
        }

        public static void BuildJSONConfig(string lineups, string maxPrice, string wtnr, string basepath, string date, string defenseBudget = null)
        {

            string txtFileToWrite = String.IsNullOrEmpty(defenseBudget) ? "dfs-nba-config.json" : "dfs-nfl-config.json";

            var config = new DFSConfig()
            {
                Lineups = Convert.ToInt32(lineups),
                DefenseBudget = !String.IsNullOrEmpty(defenseBudget) ? Convert.ToInt32(defenseBudget) : 0,
                FileToRead = Path.Combine(basepath, date, "Player List", "nfl.csv"),
                MaxSalary = Convert.ToInt32(maxPrice),
                WTNR = Convert.ToInt32(wtnr),
                TopLineupCSVPath = Path.Combine(basepath, date, "Lineups", "top_lineupsCSV.csv"),
                TopLineupOutputPath = Path.Combine(basepath, date, "Lineups", "top_lineups.txt"),
                TopLineupJSONPath = Path.Combine(basepath, date, "Lineups", "top_lineupsJSON.json")
            };

            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(config);

            File.WriteAllText(Path.Combine(basepath, txtFileToWrite), jsonStr);
        }


        public static string GetTxtText(string filepath)
        {
            return File.ReadAllText(filepath);
        }
        public static string GetFilepath(DocumentFilepaths filepath, string UserBasepath, string Date, string sport)
        {
            if (filepath == DocumentFilepaths.PlayerList)
            {
                return $"{UserBasepath}\\{sport}\\{Date}\\Player List\\playerlist.csv";
            }
            else if (filepath == DocumentFilepaths.PlayersWithProjections)
            {
                if(sport == "Football")
                {
                    return $"{UserBasepath}\\{sport}\\{Date}\\Player List\\nfl.csv";
                }
                else if(sport == "Basketball")
                {
                    return $"{UserBasepath}\\{sport}\\{Date}\\Player List\\nba.csv";
                }
            }
            else if (filepath == DocumentFilepaths.TopLineups)
            {
                return $"{UserBasepath}\\{sport}\\{Date}\\Lineups\\top_lineups.txt";
            }
            else if (filepath == DocumentFilepaths.JsonPlayerList)
            {
                return $"{UserBasepath}\\{sport}\\{Date}\\Player List\\player_list.json";
            }
            else if(filepath == DocumentFilepaths.DFSConfigFile)
            {

                if (sport == "Football")
                {
                    return $"{UserBasepath}\\{sport}\\dfs-nfl-config.json";
                }
                else if (sport == "Basketball")
                {
                    return $"{UserBasepath}\\{sport}\\{Date}\\dfs-nba-config.json";
                }
            }
            return "";
        }

        public class CSVHelper
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public string Team { get; set; }
            public double Salary { get; set; }
            public double Projected { get; set; }
            public double PricePerPoint { get; set; }
            public string Id { get; set; }
        }
    }
    public enum DocumentFilepaths
    {
        PlayerList = 0,
        PlayersWithProjections = 1,
        TopLineups = 2,
        DFSConfigFile = 3,
        JsonPlayerList = 4
    }
}
