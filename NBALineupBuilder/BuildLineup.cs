using DFSLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using DFSLibrary.Services;

namespace NBALineupBuilder
{
    public class BuildLineup
    {
        private static readonly string ConfigFilepath = @"C:\\Users\\15133\\Documents\\dfs\\Basketball\\dfs-nba-config.txt";
        public static void Build(string filepath, Sport sport)
        {
            List<Player> players = new List<Player>();
            if (sport == Sport.Basketball)
            {
                players = GetBasketballPlayerList(filepath);
            }
            DFSConfig config = new DFSConfig(ConfigFilepath);

            LineupOptimizer lo = new LineupOptimizer(players, config);
        }
        private static List<Player> GetBasketballPlayerList(string fp)
        {
            List<Player> players = new List<Player>();
            string json = File.ReadAllText(fp);
            players.AddRange(Newtonsoft.Json.JsonConvert.DeserializeObject<List<BasketballPlayer>>(json));
            return players;
        }
    }

    public enum Sport
    {
        Basketball = 0,
        Football = 1,
        Hockey = 2
    }
}
