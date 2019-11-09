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
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            if (football_rb.Checked)
            {
                FootballBuilder fb = new FootballBuilder(DateTB.Text);
                fb.Show();
            }
            else if (basketball_rb.Checked)
            {
                BasketballBuilder bb = new BasketballBuilder(DateTB.Text);
                bb.Show();
            }
        }
    }
}
