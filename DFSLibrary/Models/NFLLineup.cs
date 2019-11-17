using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFSLibrary.Models
{
    public class NFLLineup
    {
        public FootballPlayer Quarterback { get; set; }
        public List<FootballPlayer> RunningBacks { get; set; }
        public List<FootballPlayer> WideReceivers { get; set; }
        public FootballPlayer TightEnd { get; set; }
        public FootballPlayer FLEXPlayer { get; set; }

        public int Price
        {
            get
            {
                return Quarterback.Salary + RunningBacks.Sum(f => f.Salary) +
                        WideReceivers.Sum(w => w.Salary) + TightEnd.Salary + FLEXPlayer.Salary;
            }
        }
        public double Score
        {
            get
            {
                return Quarterback.Projected + RunningBacks.Sum(f => f.Projected) +
                        WideReceivers.Sum(w => w.Projected) + TightEnd.Projected + FLEXPlayer.Projected;
            }
        }

        public string LineupString
        {
            get
            {
                string rtn = Quarterback.TopLineupString + string.Join("",RunningBacks.Select(r => r.TopLineupString).ToList()) +
                        string.Join("", WideReceivers.Select(w => w.TopLineupString).ToList()) + TightEnd.TopLineupString + FLEXPlayer.TopLineupString;

                return String.Concat(rtn.OrderBy(r=>r));
            }
        }

        public string CSVString
        {
            get
            {
                return $"{Quarterback.Id},{string.Join(",", RunningBacks.Select(r => r.Id).ToList())}," +
                        $"{string.Join(",", WideReceivers.Select(w => w.Id).ToList())},{TightEnd.Id},{FLEXPlayer.Id}";
            }
        }
        public int DefenseBudget
        {
            get
            {
                return 60000 - Price;
            }
        }
        public void GetFlex(List<FootballPlayer> flexPlayers, Random rand)
        {
            while (true)
            {
                FootballPlayer flex = flexPlayers[rand.Next(0, flexPlayers.Count)];
                if(!(RunningBacks.Contains(flex) || WideReceivers.Contains(flex)))
                {
                    FLEXPlayer = flex;
                    return;
                }
            }
        }
    }
}
