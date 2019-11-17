using System;
using System.Collections.Generic;
using System.Text;

namespace DFSLibrary.Models
{
    public class NFLCombinations
    {
        public List<FootballPlayer> Quarterbacks { get; set; }
        public List<FootballPlayer> RunningBacks { get; set; }
        public List<FootballPlayer> WideReceivers { get; set; }
        public List<FootballPlayer> TightEnds { get; set; }
        public List<FootballPlayer> FLEX { get; set; }
        public List<FootballPlayer> TopRunningBacks { get; set; }
        public List<FootballPlayer> TopWideReceivers { get; set; }
        public List<List<FootballPlayer>> RunningBackCombos { get; set; }
        public List<List<FootballPlayer>> WideReceiverCombos { get; set; }
        private DFSConfig Config { get; set; }
        private Random Rand { get; set; }

        public NFLCombinations(List<FootballPlayer> players, DFSConfig config)
        {
            Rand = new Random();
            Config = config;
            Quarterbacks = new List<FootballPlayer>();
            RunningBacks = new List<FootballPlayer>();
            WideReceivers = new List<FootballPlayer>();
            TightEnds = new List<FootballPlayer>();
            FLEX = new List<FootballPlayer>();
            TopRunningBacks = new List<FootballPlayer>();
            TopWideReceivers = new List<FootballPlayer>();
            RunningBackCombos = new List<List<FootballPlayer>>();
            WideReceiverCombos = new List<List<FootballPlayer>>();
            BuildInitialPositionLists(players);
            BuildCombinations();

        }

        private void BuildInitialPositionLists(List<FootballPlayer> players)
        {
            foreach (var player in players)
            {
                switch (player.Position)
                {
                    case "qb":
                        Quarterbacks.Add(player);
                        break;
                    case "rb":
                        RunningBacks.Add(player);
                        TopRunningBacks.Add(player);
                        FLEX.Add(player);
                        break;
                    case "wr":
                        WideReceivers.Add(player);
                        TopWideReceivers.Add(player);
                        FLEX.Add(player);
                        break;
                    case "te":
                        TightEnds.Add(player);
                        FLEX.Add(player);
                        break;
                }
            }
        }

        private void BuildCombinations()
        {
            RunningBackCombos.Clear();
            WideReceiverCombos.Clear();

            foreach(var top in TopRunningBacks)
            {
                foreach(var normalRb in RunningBacks)
                {
                    if(top.Id == normalRb.Id)
                    {
                        continue;
                    }
                    List<FootballPlayer> rbs = new List<FootballPlayer>() { top, normalRb };
                    RunningBackCombos.Add(rbs);
                }
            }

            foreach(var top in TopWideReceivers)
            {
                foreach(var top2 in TopWideReceivers)
                {
                    foreach(var normalwr in WideReceivers)
                    {
                        if(top.Name == top2.Name || top.Name == normalwr.Name || top2.Name == normalwr.Name)
                        {
                            continue;
                        }
                        List<FootballPlayer> wrs = new List<FootballPlayer>() { top, top2, normalwr };
                        WideReceiverCombos.Add(wrs);
                    }
                }
            }

            DisplayCombinations();
        }


        public void OptimizeCombinations(List<NFLLineup> lineups)
        {
            Console.WriteLine("Before Optimiaztion");
            DisplayCombinations();

            Quarterbacks.Clear();
            TopRunningBacks.Clear();
            TopWideReceivers.Clear();
            TightEnds.Clear();

            foreach(var lineup in lineups)
            {

                if (!Quarterbacks.Contains(lineup.Quarterback))
                {
                    Quarterbacks.Add(lineup.Quarterback);
                }
                if (!TightEnds.Contains(lineup.TightEnd))
                {
                    TightEnds.Add(lineup.TightEnd);
                }

                foreach(var player in lineup.WideReceivers)
                {
                    if (!TopWideReceivers.Contains(player))
                    {
                        TopWideReceivers.Add(player);
                    }
                }

                foreach (var player in lineup.RunningBacks)
                {
                    if (!TopRunningBacks.Contains(player))
                    {
                        TopRunningBacks.Add(player);
                    }
                }
            }

            Console.WriteLine("After Optimaztion");
            BuildCombinations();
        }


        private void DisplayCombinations()
        {

            Console.WriteLine($"Quarterbacks: {Quarterbacks.Count}");
            Console.WriteLine($"Running Backs: {TopRunningBacks.Count}");
            Console.WriteLine($"Wide Receivers: {TopWideReceivers.Count}");
            Console.WriteLine($"Tight Ends: { TightEnds.Count}");
            Console.WriteLine($"FLEX: {FLEX.Count}");

            Console.WriteLine($"\nRunning Back Combinations: {RunningBackCombos.Count}");
            Console.WriteLine($"Wide Recevier Combinations: {WideReceiverCombos.Count}");
        }
        public NFLLineup GetLineup()
        {
            NFLLineup lineup;

            while (true)
            {
                lineup = new NFLLineup()
                {
                    Quarterback = Quarterbacks[Rand.Next(0, Quarterbacks.Count)],
                    RunningBacks = RunningBackCombos[Rand.Next(0, RunningBackCombos.Count)],
                    WideReceivers = WideReceiverCombos[Rand.Next(0, WideReceiverCombos.Count)],
                    TightEnd = TightEnds[Rand.Next(0, TightEnds.Count)]
                };

                lineup.GetFlex(FLEX, Rand);

                if (lineup.Price <= Config.MaxSalary - Config.DefenseBudget)
                {
                    break;
                }
            } 

            return lineup;
        }

    }
}
