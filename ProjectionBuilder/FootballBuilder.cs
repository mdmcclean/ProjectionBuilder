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
    public partial class FootballBuilder : Form
    {
        public string Week;
        public FootballBuilder(string week)
        {
            Week = $"Week {week}";
            InitializeComponent();
        }
    }
}
