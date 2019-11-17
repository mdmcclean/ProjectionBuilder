using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace DFSLibrary.Models
{
    public class FootballPlayer
    {

        [Name("playerName")]
        public string Name { get; set; }
        [Name("Id")]
        public string InitialId { get; set; }
        [Name("teamName")]
        public string Team { get; set; }
        [Name("position")]
        public string Position { get; set; }
        [Name("games")]
        public string Opponent { get; set; }
        [Name("fantasyPoints")]
        public double PreProjected { get; set; }
        [Name("salary")]
        public int Salary { get; set; }
        [Name("DVP")]
        public int DefenseVsPosition { get; set; }
        public double ImpliedScore { get; set; }
        public double Projected { get; set; }
        public string Id 
        {
            get
            {
                return $"{InitialId}:{Name}";
            }
        }
        public double DVPMultiplier
        {
            get
            {
                double rtn = ((DefenseVsPosition - 16) / 112.0);
                return rtn;
            }
        }
        public double PricePerPoint { get; set; }

        public string TopLineupString
        {
            get
            {
                const string format = "{0,-4} {1,-8} {2,-25} {3,6} {4,6} {5,7} {6,6} {7,20}\n";
                return string.Format(format, Position.ToUpper(), Projected.ToString("0.###"), Name, Team, Opponent, Salary, PricePerPoint.ToString("0.###"), Id);
            }
        }

    }
}
