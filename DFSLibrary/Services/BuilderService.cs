using DFSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFSLibrary.Services
{
    public class BuilderService
    {
        public static List<BasketballPlayer> SetUpPlayers(string filepath)
        {
            var players = FileBuilder.BuildPlayers(filepath);
            players = players.Where(p => p.ProjectedCSV != null && p.PPGAvgCSV != null && p.PPGFloorCSV != null && p.PPGMaxCSV != null).ToList();
            players.ForEach(p => {
                p.PPGAvg = Convert.ToDouble(p.PPGAvg);
                p.PPGMax = Convert.ToDouble(p.PPGMaxCSV);
                p.PPGFloor = Convert.ToDouble(p.PPGFloorCSV);
                p.PreProjected = Convert.ToDouble(p.ProjectedCSV);
            });
            return players;
        }

        public static Dictionary<string, Team> BuildTeams(List<BasketballPlayer> players)
        {
            Dictionary<string, Team> teams = new Dictionary<string, Team>();
            foreach (var player in players)
            {
                if (!teams.ContainsKey(player.Team))
                {
                    teams.Add(player.Team, new Team(player.Team));
                }

                switch (player.Position)
                {
                    case "PG":
                        teams[player.Team].PointGuards.Add(player);
                        break;
                    case "SG":
                        teams[player.Team].ShootingGuards.Add(player);
                        break;
                    case "SF":
                        teams[player.Team].SmallForwards.Add(player);
                        break;
                    case "PF":
                        teams[player.Team].PowerForwards.Add(player);
                        break;
                    case "C":
                        teams[player.Team].Centers.Add(player);
                        break;
                }
            }
            return teams;
        }

        public static Dictionary<string, Team> GenerateProjections(double averageScore, Dictionary<string, Team> teams)
        {
            foreach (var team in teams)
            {
                team.Value.GenerateProjections(averageScore);
            }
            return teams;
        }
    }
}
