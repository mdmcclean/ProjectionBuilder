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
        public static List<BasketballPlayer> BuildPlayers(string filepath)
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

        public static void BuildPlayerListJson(List<BasketballPlayer> players, string filepath)
        {
            List<Player> playerList = new List<Player>();
            playerList.AddRange(players);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(playerList);
            File.WriteAllText(filepath, json);
        }

        public static void BuildTopLineupListJson(List<NBALineup> players, string filepath)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(players);
            File.WriteAllText(filepath, json);
        }

        public static void BuildConfig(string lineups, string maxPrice, string wtnr, string basepath, string date)
        {
            string[] lines =
            {
                $"Lineups::{lineups}",
                $"Max Price::{maxPrice}",
                $"Writes til read new::{wtnr}",
                $"File to read::{Path.Combine(basepath, date, "Player List", "nba.csv")}",
                $"Top Lineup output path::{Path.Combine(basepath, date, "Lineups", "top_lineups.txt")}"
            };

            File.WriteAllLines(Path.Combine(basepath, "dfs-nba-config.txt"), lines);
        }

        public static string GetTxtText(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        public class CSVHelper
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public string Team { get; set; }
            public double Salary { get; set; }
            public double Projected { get; set; }
            public double PricePerPoint { get; set; }
        }
    }
    public enum DocumentFilepaths
    {
        PlayerList = 0,
        PlayersWithProjections = 1,
        TopLineups = 2,
        DFSConfigFile = 3,
        NBAJsonPlayerList = 4
    }
}
