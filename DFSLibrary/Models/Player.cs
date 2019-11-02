using System;
using System.Collections.Generic;
using System.Text;

namespace DFSLibrary.Models
{
    public class Player
    {

        public string PlayerName { get; set; }
        public string PlayerPosition { get; set; }
        public string PlayerTeam { get; set; }
        public double PlayerSalary { get; set; }
        public double PlayerProjected { get; set; }
        public double PlayerPricePerPoint { get; set; }

        public string TopLineupString
        {
            get
            {
                const string format = "{0,-4} {1,-8} {2,-25} {3,6} {4,7} {5,6}";
                return string.Format(format, PlayerPosition, PlayerProjected.ToString("0.###"), PlayerName, PlayerTeam, PlayerSalary, PlayerPricePerPoint.ToString("0.###"));
            }
        }
    }
}
