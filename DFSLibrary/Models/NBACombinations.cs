using System;
using System.Collections.Generic;
using System.Text;

namespace DFSLibrary.Models
{
    public class NBACombinations
    {
        private List<Player> PointGuards { get; set; }
        private List<Player> ShootingGuards { get; set; }
        private List<Player> SmallForwards { get; set; }
        private List<Player> PowerForwards { get; set; }
        public List<Player> Centers { get; set; }
        private List<Player> TopPointGuards { get; set; }
        private List<Player> TopShootingGuards { get; set; }
        private List<Player> TopSmallForwards { get; set; }
        private List<Player> TopPowerForwards { get; set; }
        public List<Tuple<Player, Player>> PointGuardCombos { get; set; }
        public List<Tuple<Player, Player>> ShootingGuardCombos { get; set; }
        public List<Tuple<Player, Player>> SmallForwardCombos { get; set; }
        public List<Tuple<Player, Player>> PowerForwardCombos { get; set; }
        private DFSConfig Config { get; set; }
        private Random Rand { get; set; }



        public NBACombinations(List<Player> players, DFSConfig config)
        {
            PointGuards = new List<Player>();
            ShootingGuards = new List<Player>();
            SmallForwards = new List<Player>();
            PowerForwards = new List<Player>();
            Centers = new List<Player>();
            TopPointGuards = new List<Player>();
            TopPowerForwards = new List<Player>();
            TopShootingGuards = new List<Player>();
            TopSmallForwards = new List<Player>();
            PointGuardCombos = new List<Tuple<Player, Player>>();
            ShootingGuardCombos = new List<Tuple<Player, Player>>();
            SmallForwardCombos = new List<Tuple<Player, Player>>();
            PointGuardCombos = new List<Tuple<Player, Player>>();
            BuildInitialPositionLists(players);
            BuildCombinations();
            Config = config;
            Rand = new Random();
        }

        private void BuildInitialPositionLists(List<Player> players)
        {
            foreach(var player in players)
            {
                switch (player.PlayerPosition)
                {
                    case "PG":
                        PointGuards.Add(player);
                        TopPointGuards.Add(player);
                        break;
                    case "SG":
                        ShootingGuards.Add(player);
                        TopShootingGuards.Add(player);
                        break;
                    case "SF":
                        SmallForwards.Add(player);
                        TopSmallForwards.Add(player);
                        break;
                    case "PF":
                        PowerForwards.Add(player);
                        TopPowerForwards.Add(player);
                        break;
                    case "C":
                        Centers.Add(player);
                        break;
                }
            }
        }

        public void BuildCombinations()
        {
            PointGuardCombos = GetPositionCombo(TopPointGuards, PointGuards);
            ShootingGuardCombos = GetPositionCombo(TopShootingGuards, ShootingGuards);
            SmallForwardCombos = GetPositionCombo(TopSmallForwards, SmallForwards);
            PowerForwardCombos = GetPositionCombo(TopPowerForwards, PowerForwards);

            DisplayCombinations();
        }
        private List<Tuple<Player, Player>> GetPositionCombo(List<Player> topPlayers, List<Player> players)
        {
            List<Tuple<Player, Player>> rtnList = new List<Tuple<Player, Player>>();
            foreach(var topPlayer in topPlayers)
            {
                foreach(var player in players)
                {
                    if(topPlayer.PlayerName == player.PlayerName)
                    {
                        continue;
                    }

                    Tuple<Player, Player> playerCombo = new Tuple<Player, Player>(topPlayer, player);
                    rtnList.Add(playerCombo);
                }
            }
            return rtnList;
        }

        public NBALineup GetLineup()
        {
            NBALineup lineup;
            while (true)
            {
                Tuple<Player, Player> pgStarters = GetPlayerCombo(PointGuardCombos);
                Tuple<Player, Player> sgStarters = GetPlayerCombo(ShootingGuardCombos);
                Tuple<Player, Player> sfStarters = GetPlayerCombo(SmallForwardCombos);
                Tuple<Player, Player> pfStarters = GetPlayerCombo(PowerForwardCombos);
                Player center = Centers[Rand.Next(0, Centers.Count - 1)];

                lineup = new NBALineup()
                {
                    PointGuard1 = pgStarters.Item1,
                    PointGuard2 = pgStarters.Item2,
                    ShootingGuard1 = sgStarters.Item1,
                    ShootingGuard2 = sgStarters.Item2,
                    SmallForward1 = sfStarters.Item1,
                    SmallForward2 = sfStarters.Item2,
                    PowerForward1 = pfStarters.Item1,
                    PowerForward2 = pfStarters.Item2,
                    Center = center
                };

                if(lineup.Price <= Config.MaxSalary)
                {
                    break;
                }
            }
            return lineup;
        }
        private Tuple<Player, Player> GetPlayerCombo(List<Tuple<Player,Player>> playerCombos)
        {
            return playerCombos[Rand.Next(0, playerCombos.Count - 1)];
        }

        public void OptimizeCombinations(List<NBALineup> lineups)
        {
            Console.WriteLine("Before Optimization");
            DisplayCombinations();

            TopPointGuards.Clear();
            TopPowerForwards.Clear();
            TopShootingGuards.Clear();
            TopSmallForwards.Clear();

            foreach(var lineup in lineups)
            {
                if (!TopPointGuards.Contains(lineup.PointGuard1))
                {
                    TopPointGuards.Add(lineup.PointGuard1);
                }
                if (!TopPointGuards.Contains(lineup.PointGuard2))
                {
                    TopPointGuards.Add(lineup.PointGuard2);
                }
                if (!TopShootingGuards.Contains(lineup.ShootingGuard1))
                {
                    TopShootingGuards.Add(lineup.ShootingGuard1);
                }
                if (!TopShootingGuards.Contains(lineup.ShootingGuard2))
                {
                    TopShootingGuards.Add(lineup.ShootingGuard2);
                }
                if (!TopSmallForwards.Contains(lineup.SmallForward1))
                {
                    TopSmallForwards.Add(lineup.SmallForward1);
                }
                if (!TopSmallForwards.Contains(lineup.SmallForward2))
                {
                    TopSmallForwards.Add(lineup.SmallForward2);
                }
                if (!TopPowerForwards.Contains(lineup.PowerForward1))
                {
                    TopPowerForwards.Add(lineup.PowerForward1);
                }
                if (!TopPowerForwards.Contains(lineup.PowerForward2))
                {
                    TopPowerForwards.Add(lineup.PowerForward2);
                }
            }
            Console.WriteLine("After Optimaztion");
            BuildCombinations();
            
        }

        public void DisplayCombinations()
        {
            Console.WriteLine($"PG Combinations: {PointGuardCombos.Count}");
            Console.WriteLine($"SG Combinations: {ShootingGuardCombos.Count}");
            Console.WriteLine($"SF Combinations: {SmallForwardCombos.Count}");
            Console.WriteLine($"PF Combinations: {PowerForwardCombos.Count}");
            Console.WriteLine($"C Combinations:  {Centers.Count}");
        }

    }
}
