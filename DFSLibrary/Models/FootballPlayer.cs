using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace DFSLibrary.Models
{
    public class FootballPlayer : Player
    {

        [Name("Name")]
        public string Name { get; set; }
        [Name("Id")]
        public string Id { get; set; }
        [Name("teamName")]
        public string Team { get; set; }
        [Name("position")]
        public string Position { get; set; }
        [Name("games")]
        public string Opponent { get; set; }
        [Name("fantasyPoints")]
        public string FantasyPoints { get; set; }
        [Name("salary")]
        public int Salary { get; set; }

    }
}
