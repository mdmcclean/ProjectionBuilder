using ProjectionBuilder.Models;
using ProjectionBuilder.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectionBuilder
{
    public partial class Form1 : Form
    {
        public List<BasketballPlayer> Players { get; set; }
        public Dictionary<string, Team> Teams { get; set; }
        public double AverageScore { get; set; }
        public string Date { get; set; }

        public Form1()
        {
            InitializeComponent();
            BB_Teams_gb.Visible = false;
        }

        private void Generate_btn_Click(object sender, EventArgs e)
        {
            Date = date_textbox.Text;
            lineup_bp_text.Text = GetFilepath(DocumentFilepaths.TopLineups);
            var players = CSVBuilder.BuildPlayers(GetFilepath(DocumentFilepaths.PlayerList));
            players = players.Where(p => p.ProjectedCSV != null && p.PPGAvg != null && p.PPGFloor != null && p.PPGMax != null).ToList();
            players.ForEach(s => s.PreProjected = Convert.ToDouble(s.ProjectedCSV));
            Players = players;
            BuildTeams(players);
            ShowTeams();
            BB_Teams_gb.Visible = true;
            lineup_gb.Visible = true;
            Date_gb.Visible = false;
        }
        private string GetFilepath(DocumentFilepaths filepath)
        {
            if (filepath == DocumentFilepaths.PlayerList)
            {
                return $"C:\\Users\\15133\\Documents\\dfs\\Basketball\\{Date}\\Player List\\playerlist.csv";
            }
            else if(filepath == DocumentFilepaths.PlayersWithProjections)
            {
                return $"C:\\Users\\15133\\Documents\\dfs\\Basketball\\{Date}\\Player List\\nba.csv";
            }
            else if(filepath == DocumentFilepaths.TopLineups)
            {
                return $"C:\\Users\\15133\\Documents\\dfs\\Basketball\\{Date}\\Lineups\\top_lineups.txt";
            }
            return "";
        }

        private void Submit_scores_btn_Click(object sender, EventArgs e)
        {
            List<double> scores = new List<double>();
            bool badData = false;
            foreach(var team in Teams)
            {
                try
                {
                    double score = GetScore(team.Key);
                    team.Value.SetImpliedScore(score);
                    scores.Add(score);
                }
                catch(Exception ex)
                {
                    badData = true;
                    MessageBox.Show("There is a score missing from " + team.Key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            if (!badData)
            {
                AverageScore = scores.Average();
                AverageScore = AverageScore * .969;
                BB_Teams_gb.Visible = false;
                Generate_proj_gb.Visible = true;
            }

        }
        private void BuildTeams(List<BasketballPlayer> players)
        {
            Teams = new Dictionary<string, Team>();
            foreach (var player in players)
            {
                if (!Teams.ContainsKey(player.Team))
                {
                    Teams.Add(player.Team, new Team(player.Team));
                }

                switch (player.Position)
                {
                    case "PG":
                        Teams[player.Team].PointGuards.Add(player);
                        break;
                    case "SG":
                        Teams[player.Team].ShootingGuards.Add(player);
                        break;
                    case "SF":
                        Teams[player.Team].SmallForwards.Add(player);
                        break;
                    case "PF":
                        Teams[player.Team].PowerForwards.Add(player);
                        break;
                    case "C":
                        Teams[player.Team].Centers.Add(player);
                        break;
                }
            }
        }
        private void ShowTeams()
        {
            foreach(var team in Teams)
            {
                TextBox cnt = ((TextBox)this.Controls.Find($"{team.Key}_tb", true)[0]);
                cnt.Visible = true;
            }
        }

        private double GetScore(string teamName)
        {
            string score = ((TextBox)this.Controls.Find($"{teamName}_tb", true)[0]).Text;
            return Convert.ToDouble(score);
        }

        private void Projections_btn_Click(object sender, EventArgs e)
        {
            foreach(var team in Teams)
            {
                team.Value.GenerateProjections(AverageScore);
            }
            Generate_proj_gb.Visible = false;
            Generate_csv_gb.Visible = true;
        }

        private void Generate_CSV_btn_Click(object sender, EventArgs e)
        {
            string fp = GetFilepath(DocumentFilepaths.PlayersWithProjections);
            CSVBuilder.BuildCSV(Players, fp);
            CSVBuilder.BuildConfig(Lineups_tb.Text, MaxPrice_tb.Text, WTNR_tb.Text, basepath_tb.Text, Date);
            MessageBox.Show("Success! Your CSV is located at " + fp, "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Get_top_lineups_btn_Click(object sender, EventArgs e)
        {
            string fp = lineup_bp_text.Text;
            string topLineups = CSVBuilder.GetTxtText(fp);
            top_lineups_tb.Text = topLineups;
        }
    }
}
