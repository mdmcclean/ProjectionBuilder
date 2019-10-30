using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionBuilder.Models
{
    public class Team
    {
        public List<BasketballPlayer> PointGuards { get; set; }
        public List<BasketballPlayer> ShootingGuards { get; set; }
        public List<BasketballPlayer> SmallForwards { get; set; }
        public List<BasketballPlayer> PowerForwards { get; set; }
        public List<BasketballPlayer> Centers { get; set; }
        public string TeamName { get; set; }
        public double ImpliedScore { get; set; }
        public double TeamTotalPoints { get; set; }

        public Team(string teamName)
        {
            TeamName = teamName;
            PointGuards = new List<BasketballPlayer>();
            ShootingGuards = new List<BasketballPlayer>();
            SmallForwards = new List<BasketballPlayer>();
            PowerForwards = new List<BasketballPlayer>();
            Centers = new List<BasketballPlayer>();
        }

        public void SetImpliedScore(double score)
        {
            ImpliedScore = score;

            PointGuards.ForEach(s => s.ImpliedScore = score);
            ShootingGuards.ForEach(s => s.ImpliedScore = score);
            SmallForwards.ForEach(s => s.ImpliedScore = score);
            PowerForwards.ForEach(s => s.ImpliedScore = score);
            Centers.ForEach(s => s.ImpliedScore = score);
        }

        public void GenerateProjections(double Average)
        {
            List<BasketballPlayer> teamPlayers = new List<BasketballPlayer>();
            teamPlayers.AddRange(PointGuards);
            teamPlayers.AddRange(ShootingGuards);
            teamPlayers.AddRange(SmallForwards);
            teamPlayers.AddRange(PowerForwards);
            teamPlayers.AddRange(Centers);

            double multiplier = (ImpliedScore - Average) / 50;

            double pgScore = PointGuards.Sum(s => s.PreProjected);
            double sgScore = ShootingGuards.Sum(s => s.PreProjected);
            double sfScore = SmallForwards.Sum(s => s.PreProjected);
            double pfScore = PowerForwards.Sum(s => s.PreProjected);
            double cScore = Centers.Sum(s => s.PreProjected);

            TeamTotalPoints = pgScore + sgScore + sfScore + pfScore + cScore;

            foreach(var player in teamPlayers)
            {
                double percentOfTeam = player.PreProjected / TeamTotalPoints;
                double playerMultiplier = (multiplier * percentOfTeam) + (percentOfTeam* player.DVPMultiplier) + 1;
                player.Projected = playerMultiplier * player.PreProjected;
            }

            
        }

    }
}
