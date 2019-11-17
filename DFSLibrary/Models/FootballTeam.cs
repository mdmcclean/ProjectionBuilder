using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFSLibrary.Models
{
    public class FootballTeam
    {
        public string TeamName { get; set; }
        public string Opponent { get; set; }
        public bool isHome { get; set; }
        public double ImpliedScore { get; set; }
        public double TeamTotalPoints { get; set; }
        public List<FootballPlayer> Quarterbacks { get; set; }
        public List<FootballPlayer> RunningBacks { get; set; }
        public List<FootballPlayer> WideReceivers { get; set; }
        public List<FootballPlayer> TightEnds { get; set; }
        
        public FootballTeam(string teamName)
        {
            TeamName = teamName;
            Quarterbacks = new List<FootballPlayer>();
            RunningBacks = new List<FootballPlayer>();
            WideReceivers = new List<FootballPlayer>();
            TightEnds = new List<FootballPlayer>();
        }

        public void SetImpliedScore(double score)
        {
            ImpliedScore = score;

            Quarterbacks.ForEach(s => s.ImpliedScore = score);
            RunningBacks.ForEach(s => s.ImpliedScore = score);
            WideReceivers.ForEach(s => s.ImpliedScore = score);
            TightEnds.ForEach(s => s.ImpliedScore = score);
        }


        public void GenerateProjections(double Average)
        {
            List<FootballPlayer> teamPlayers = new List<FootballPlayer>();
            teamPlayers.AddRange(Quarterbacks);
            teamPlayers.AddRange(RunningBacks);
            teamPlayers.AddRange(WideReceivers);
            teamPlayers.AddRange(TightEnds);

            double multiplier = (ImpliedScore - Average) / 60;
            double qbScore = Quarterbacks.Sum(s => s.PreProjected);
            double rbScore = RunningBacks.Sum(s => s.PreProjected);
            double wrScore = WideReceivers.Sum(s => s.PreProjected);
            double teScore = TightEnds.Sum(s => s.PreProjected);

            TeamTotalPoints = qbScore + rbScore + wrScore + teScore;
            double homeMultiplier = isHome ? 0.02 : 0;

            foreach (var player in teamPlayers)
            {
                double projectedAverage = player.PreProjected;
                double percentOfTeam = projectedAverage / TeamTotalPoints + .25;
                double playerMultiplier = homeMultiplier + (multiplier * percentOfTeam) + (percentOfTeam * player.DVPMultiplier) + 1;
                player.Projected = playerMultiplier * projectedAverage;
                player.PricePerPoint = player.Salary / player.Projected;
            }


        }

    }
}
