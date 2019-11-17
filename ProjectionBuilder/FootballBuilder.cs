using DFSLibrary.Models;
using DFSLibrary.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectionBuilder
{
    public partial class FootballBuilder : Form
    {
        public List<FootballPlayer> Players { get; set; }
        public Dictionary<string, FootballTeam> Teams { get; set; }
        public Double AverageScore { get; set; }
        public List<Tuple<FootballTeam, FootballTeam>> Games { get; set; }
        public string Week;
        private readonly string UserBasepath = @"C:\Users\15133\Documents\dfs";
        public readonly string Football = "Football";
        public FootballBuilder(string week)
        {
            InitializeComponent();
            Week = $"Week {week}";
            lineup_bp_text.Text = FileBuilder.GetFilepath(DocumentFilepaths.TopLineups, UserBasepath, Week, Football);
            FileBuilder.CSVCleaner(FileBuilder.GetFilepath(DocumentFilepaths.PlayerList, UserBasepath, Week, Football));
            Players = BuilderService.SetUpNFLPlayers(FileBuilder.GetFilepath(DocumentFilepaths.PlayerList, UserBasepath, Week, Football));
            Teams = BuilderService.BuildNFLTeams(Players);
            Games = BuilderService.BuildNFLGames(Teams);
            ShowTeams();
        }

        private void ShowTeams()
        {
            for(int i = 0; i < 16; i++)
            {
                Label homeLabel = ((Label)this.Controls.Find($"Home{i + 1}", true)[0]);
                TextBox hometb = ((TextBox)this.Controls.Find($"Home{i + 1}_tb", true)[0]);
                Label awayLabel = ((Label)this.Controls.Find($"Away{i + 1}", true)[0]);
                TextBox awaytb = ((TextBox)this.Controls.Find($"Away{i + 1}_tb", true)[0]);

                if(i >= Games.Count)
                {
                    homeLabel.Visible = false;
                    hometb.Visible = false;
                    awayLabel.Visible = false;
                    awaytb.Visible = false;
                    continue;
                }

                homeLabel.Text = Games[i].Item1.TeamName;
                hometb.Name = Games[i].Item1.TeamName + "tb";

                awayLabel.Text = Games[i].Item2.TeamName;
                awaytb.Name = Games[i].Item2.TeamName + "tb";
            }
        }

        private void submit_scores_btn_Click(object sender, EventArgs e)
        {

            List<double> scores = new List<double>();
            bool badData = false;
            foreach (var team in Teams)
            {
                try
                {
                    double score = GetScore(team.Key);
                    if (score > 50 || score < 3)
                    {
                        MessageBox.Show("Invalid score for " + team.Key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("Invalid");
                    }
                    team.Value.SetImpliedScore(score);
                    scores.Add(score);
                }
                catch (Exception ex)
                {
                    badData = true;
                    if (ex.Message != "Invalid")
                    {
                        MessageBox.Show("There is a score missing from " + team.Key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                }
            }

            if (!badData)
            {
                AverageScore = scores.Average();
                AverageScore = AverageScore * .969;
                BuilderService.GenerateNFLProjections(AverageScore, Teams);
                Score_gb.Visible = false;
                SetUpConfigText();
                Generate_csv_gb.Visible = true;
            }

        }
        private double GetScore(string teamName)
        {
            TextBox teamTB = ((TextBox)this.Controls.Find($"{teamName}tb", true)[0]);
            return Convert.ToDouble(teamTB.Text);
        }
        
        private void Generate_CSV_btn_Click(object sender, EventArgs e)
        {
            string fp = FileBuilder.GetFilepath(DocumentFilepaths.PlayersWithProjections, UserBasepath, Week, Football);

            FileBuilder.BuildJSONConfig(Lineups_tb.Text, MaxPrice_tb.Text, WTNR_tb.Text, basepath_tb.Text, Week, defense_budget_tb.Text);

            FileBuilder.BuildNFLCSV(Players, fp);
            MessageBox.Show("Success! Your CSV is located at " + fp, "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Generate_Projections_btn_Click(object sender, EventArgs e)
        {
            string csharpPath = @"LineupBuilder.exe";
            Process lineupProc = new Process();
            //lineupProc.StartInfo.FileName = pythonPath;
            lineupProc.StartInfo.FileName = csharpPath;
            lineupProc.StartInfo.Arguments = $"\"{FileBuilder.GetFilepath(DocumentFilepaths.JsonPlayerList, UserBasepath, Week, Football)}\" Football";
            lineupProc.Start();
        }

        private void get_top_lineups_btn_Click(object sender, EventArgs e)
        {
            string fp = lineup_bp_text.Text;
            string topLineups = FileBuilder.GetTxtText(fp);
            top_lineups_tb.Text = topLineups;
        }
        private void SetUpConfigText()
        {
            DFSConfig config = Newtonsoft.Json.JsonConvert.DeserializeObject<DFSConfig>(File.ReadAllText(FileBuilder.GetFilepath(DocumentFilepaths.DFSConfigFile,UserBasepath,"",Football)));
            Lineups_tb.Text = config.Lineups.ToString();
            defense_budget_tb.Text = config.DefenseBudget.ToString();
            WTNR_tb.Text = config.WTNR.ToString();
            MaxPrice_tb.Text = config.MaxSalary.ToString();
            basepath_tb.Text = Path.Combine(UserBasepath, "Football");
        }
    }
}
