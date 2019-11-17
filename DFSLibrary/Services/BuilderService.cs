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
        public static List<BasketballPlayer> SetUpNBAPlayers(string filepath)
        {
            var players = FileBuilder.BuildNBAPlayers(filepath);
            players = players.Where(p => p.ProjectedCSV != null && p.PPGAvgCSV != null && p.PPGFloorCSV != null && 
                                    p.PPGMaxCSV != null && p.PointsPerMinuteCSV != null && p.TotalMinutesCSV != null).ToList();
            players.ForEach(p => {
                p.PPGAvg = Convert.ToDouble(p.PPGAvg);
                p.PPGMax = Convert.ToDouble(p.PPGMaxCSV);
                p.PPGFloor = Convert.ToDouble(p.PPGFloorCSV);
                p.PreProjected = Convert.ToDouble(p.ProjectedCSV);
                p.PointsPerMinute = Convert.ToDouble(p.PointsPerMinuteCSV);
                p.TotalMinutes = Convert.ToDouble(p.TotalMinutesCSV);
            });
            return players;
        }

        public static List<FootballPlayer> SetUpNFLPlayers(string filepath)
        {
            var players = FileBuilder.BuildNFLPlayers(filepath);
            return players;
        }

        public static Dictionary<string, FootballTeam> BuildNFLTeams(List<FootballPlayer> players)
        {
            Dictionary<string, FootballTeam> teams = new Dictionary<string, FootballTeam>();
            foreach(var player in players)
            {
                if (!teams.ContainsKey(player.Team))
                {
                    var footballTeam = new FootballTeam(player.Team);
                    string opponent = player.Opponent;
                    if (opponent.Contains("@"))
                    {
                        footballTeam.isHome = false;
                        opponent = opponent.Replace("@", "");
                    }
                    else
                    {
                        footballTeam.isHome = true;
                    }
                    footballTeam.Opponent = opponent;

                    teams.Add(player.Team, footballTeam);

                }

                switch (player.Position)
                {
                    case "qb":
                        teams[player.Team].Quarterbacks.Add(player);
                        break;
                    case "rb":
                        teams[player.Team].RunningBacks.Add(player);
                        break;
                    case "wr":
                        teams[player.Team].WideReceivers.Add(player);
                        break;
                    case "te":
                        teams[player.Team].TightEnds.Add(player);
                        break;
                }
            }

            return teams;
        }

        public static List<Tuple<FootballTeam, FootballTeam>> BuildNFLGames(Dictionary<string, FootballTeam> teams)
        {
            List<Tuple<FootballTeam, FootballTeam>> games = new List<Tuple<FootballTeam, FootballTeam>>();

            foreach (var team in teams)
            {
                if (team.Value.isHome)
                {
                    Tuple<FootballTeam, FootballTeam> game = new Tuple<FootballTeam, FootballTeam>(team.Value, teams[team.Value.Opponent]);
                    games.Add(game);
                }
            }
            return games;
        }

        public static Dictionary<string, BasketballTeam> BuildNBATeams(List<BasketballPlayer> players)
        {
            Dictionary<string, BasketballTeam> teams = new Dictionary<string, BasketballTeam>();
            foreach (var player in players)
            {
                if (!teams.ContainsKey(player.Team))
                {
                    teams.Add(player.Team, new BasketballTeam(player.Team));
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

        public static Dictionary<string, BasketballTeam> GenerateNBAProjections(double averageScore, Dictionary<string, BasketballTeam> teams)
        {
            foreach (var team in teams)
            {
                team.Value.GenerateProjections(averageScore);
            }
            return teams;
        }

        public static Dictionary<string, FootballTeam> GenerateNFLProjections(double averageScore, Dictionary<string, FootballTeam> teams)
        {
            foreach(var team in teams)
            {
                team.Value.GenerateProjections(averageScore);
            }
            return teams;
        }
    }
}
