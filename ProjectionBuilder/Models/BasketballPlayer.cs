using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace ProjectionBuilder.Models
{
    public class BasketballPlayer
    {
        [Name("first_name")]
        public string FirstName { get; set; }
        [Name("last_name")]
        public string LastName { get; set; }
        public string Name
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        [Name("position")]
        public string Position { get; set; }
        [Name("team")]
        public string Team { get; set; }
        [Name("opp")]
        public string Opponent { get; set; }
        public double ImpliedScore { get; set; }
        [Name("salary")]
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
    }
}
