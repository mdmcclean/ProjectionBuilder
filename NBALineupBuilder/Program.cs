using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFSLibrary.Models;
using DFSLibrary.Services;
using NBALineupBuilder;

namespace LineupBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                if(args[1] == "Football")
                {
                    BuildLineup.Build(args[0], Sport.Football);
                }
                else if(args[1] == "Basketball")
                {
                    BuildLineup.Build(args[0], Sport.Basketball);
                }
                
            }
        }
    }
}
