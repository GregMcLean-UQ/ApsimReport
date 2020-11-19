using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApsimReport
{
    public partial class ApsimReportForm : Form
    {
        public ApsimReportForm()
        {
            InitializeComponent();
        }

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = 1;
            Text = "APSIM Report";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            Text = "APSIM Report - " +  Path.GetFileName( openFileDialog.FileName);
        }
    }
}
