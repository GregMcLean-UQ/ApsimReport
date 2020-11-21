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
        ApsimOutputFile outFile;
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
            treeView.SelectedNode = treeView.Nodes[0];
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Open the output files in an apsim file reader.
            string fileName = e.ToString();
            string dir = Path.GetDirectoryName  (openFileDialog.FileName);
            fileName = Path.Combine(dir, e.Node.Text + ".out");
             outFile = new ApsimOutputFile(fileName);


            GraphData();
        }
        private void GraphData()
        {
            // Get the data for each graph and display.
           
            List<DateTime> dates = outFile.GetDates();
            PopulateSeries("biomass", dates);
            PopulateSeries("yield", dates);





        }
        private void PopulateSeries(String seriesName,List<DateTime> dates)
        {
            chart.Series[seriesName].Points.Clear();
            List<double> biomass = outFile.GetData(seriesName);
            for (int i = 0; i < biomass.Count; i++)
            {
                chart.Series[seriesName].Points.AddXY(dates[i], biomass[i]);

            }
        }
    }
}
