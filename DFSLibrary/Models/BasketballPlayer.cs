using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace DFSLibrary.Models
{
    public class BasketballPlayer : Player
    {
        [Name("Name")]
        public string Name{  get; set; }
        [Name("Pos")]
        public string Position { get; set; }
        [Name("Team")]
        public string Team { get; set; }
        [Name("Opponent")]
        public string Opponent { get; set; }
        public double ImpliedScore { get; set; }
        [Name("Salary")]
        public double Salary { get; set; }
        [Name("L5_dvp_rank")]
        public int DVPLastFive { get; set; }
        [Name("L5_ppg_floor")]
        public double? PPGFloorCSV { get; set; }
        [Name("L5_ppg_max")]
        public double? PPGMaxCSV { get; set; }
        [Name("L5_ppg_avg")]
        public double? PPGAvgCSV { get; set; }
        public double PPGFloor { get; set; }
        public double PPGMax { get; set; }
        public double PPGAvg { get; set; }
        [Name("ppg_projection")]
        public double? ProjectedCSV { get; set; }
        [Name("Pts/Min")]
        public double? PointsPerMinuteCSV { get; set; }
        public double PointsPerMinute { get; set; }
        [Name("Target Value")]
        public double TargetValueCSV { get; set; }
        public double TargetValue { get; set; }
        [Name("ToMin")]
        public double? TotalMinutesCSV { get; set; }
        public double TotalMinutes { get; set; }
        public double PreProjected { get; set; }
        public double Projected { get; set; }
        public double PricePerPoint
        {
            get
            {
                return Salary / Projected;
            }
        }
        public double DVPMultiplier
        {
            get
            {
                double rtn = ((DVPLastFive - 15) / 120.0);
                return rtn;
            }
        }

        public void SetPlayer()
        {
            base.PlayerName = Name;
            base.PlayerPosition = Position;
            base.PlayerPricePerPoint = PricePerPoint;
            base.PlayerProjected = Projected;
            base.PlayerSalary = Salary;
            base.PlayerTeam = Team;
        }
    }
}
