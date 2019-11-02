using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFSLibrary.Models
{
    public class NBALineup
    {
        public Player PointGuard1 { get; set; }
        public Player PointGuard2 { get; set; }
        public Player ShootingGuard1 { get; set; }
        public Player ShootingGuard2 { get; set; }
        public Player SmallForward1 { get; set; }
        public Player SmallForward2 { get; set; }
        public Player PowerForward1 { get; set; }
        public Player PowerForward2 { get; set; }
        public Player Center { get; set; }

        public int Price
        {
            get
            {
                double price = PointGuard1.PlayerSalary + PointGuard2.PlayerSalary +
                                ShootingGuard1.PlayerSalary + ShootingGuard2.PlayerSalary +
                                SmallForward1.PlayerSalary + SmallForward2.PlayerSalary +
                                PowerForward1.PlayerSalary + PowerForward2.PlayerSalary +
                                Center.PlayerSalary;
                return (int)price;
            }
        }

        public double Score
        {
            get
            {
                double score = PointGuard1.PlayerProjected + PointGuard2.PlayerProjected +
                                ShootingGuard1.PlayerProjected + ShootingGuard2.PlayerProjected +
                                SmallForward1.PlayerProjected + SmallForward2.PlayerProjected +
                                PowerForward1.PlayerProjected + PowerForward2.PlayerProjected +
                                Center.PlayerProjected;
                return score;
            }
        }

        public string LineupString
        {
            get
            {
                string lineup = PointGuard1.PlayerName + PointGuard1.PlayerTeam +
                                PointGuard2.PlayerName + PointGuard2.PlayerTeam +
                                ShootingGuard1.PlayerName + ShootingGuard1.PlayerTeam +
                                ShootingGuard2.PlayerName + ShootingGuard2.PlayerTeam +
                                SmallForward1.PlayerName + SmallForward1.PlayerTeam +
                                SmallForward2.PlayerName + SmallForward2.PlayerTeam +
                                PowerForward1.PlayerName + PowerForward1.PlayerTeam +
                                PowerForward2.PlayerName + PowerForward2.PlayerTeam +
                                Center.PlayerName + Center.PlayerTeam;
                lineup = String.Concat(lineup.OrderBy(s => s));
                return lineup;
            }
        }
    }
}
