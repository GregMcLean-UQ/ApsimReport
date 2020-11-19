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
using System.Xml.Linq;

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
            ListSimulations();
        }
        private void ListSimulations()
        {
            treeView.Nodes.Clear();
            //
            // Go through the .apsim file and get the output report files.
            // Add the simulation name and output file names to the listbox.

            XElement doc = XElement.Load(openFileDialog.FileName);
            IEnumerable<XElement> sims =
                from el in doc.Elements("folder").Elements("simulation")
                select el;

            foreach (XElement s in sims)
            {
                string sim = s.Attribute("name").ToString().Substring(5);
                treeView.Nodes.Add(sim.Replace("\"", ""));
            }
        }

    }
}
