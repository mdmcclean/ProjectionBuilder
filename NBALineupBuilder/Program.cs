using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFSLibrary.Models;
using DFSLibrary.Services;

namespace NBALineupBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                BuildLineup.Build(args[0], Sport.Basketball);
            }
        }
    }
}
