using DFSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFSLibrary.Services
{
    public class LineupOptimizer
    {
        public List<Player> Players { get; set; }
        public DFSConfig Config { get; set; }
        public int DuplicatedLineups { get; set; }
        private static readonly int LineupsToGenerate = 10000000;

        public LineupOptimizer(List<Player> players, DFSConfig config)
        {
            DuplicatedLineups = 0;
            Players = players;
            Config = config;
            Optimize();
        }

        private void Optimize()
        {
            NBACombinations combinations = new NBACombinations(Players, Config);
            List<NBALineup> topLineups = new List<NBALineup>();
            int writeNumber = 0;
            int loopNum = 0;
            while (true)
            {
                Console.WriteLine("\nGenerating Lineups...");
                for (int i = 0; i < LineupsToGenerate; i++)
                {
                    topLineups.Add(combinations.GetLineup());
                }

                topLineups = SortLineups(topLineups);

                FileBuilder.WriteTopLineups(topLineups, Config.TopLineupOutputPath);
                
                for(int i = 4; i >= 0; i--)
                {
                    Console.WriteLine(FileBuilder.BuildNBALineupString(topLineups[i], i + 1));
                }
                writeNumber++;
                loopNum++;
                Console.WriteLine($"Duplicated Lineups: {DuplicatedLineups}");
                Console.WriteLine($"Writes till optimization: {Config.WTNR - writeNumber}");
                Console.WriteLine($"Lineups Generated: {loopNum}0 Million");
                if(writeNumber >= Config.WTNR)
                {
                    Console.WriteLine("Optimizing Lineups...");
                    combinations.OptimizeCombinations(topLineups);
                    writeNumber = 0;
                }
            }
        }

        private List<NBALineup> SortLineups(List<NBALineup> lineups)
        {
            lineups = lineups.OrderByDescending(o => o.Score).ToList();
            List<NBALineup> tempLineups = new List<NBALineup>();
            List<string> usedLineups = new List<string>();
            foreach(var lineup in lineups)
            {
                if (usedLineups.Contains(lineup.LineupString))
                {
                    Console.WriteLine("Duplicate Lineup");
                    DuplicatedLineups++;
                    continue;
                }
                tempLineups.Add(lineup);
                usedLineups.Add(lineup.LineupString);

                if(tempLineups.Count >= Config.Lineups)
                {
                    break;
                }
            }
            return tempLineups;
        }

    }
}
