using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ApsimReport
{
    public class ApsimOutputFile
    {
        //Members--------------------------------------------------------------
        List<DateTime> dates;
        List<string> variableNames;
        List<List<double?>> data;

        public ApsimOutputFile(string fileName)
        {
            dates = new List<DateTime>();
            data = new List<List<double?>>();
            string[] lines = File.ReadAllLines(fileName);

            int dataStart = GetVariableNames(lines);
            GetData(lines, dataStart);


        }
        private int GetVariableNames(string[] lines)
        {
            int lineNo = 0;
            // Look for Title. Next line is variables
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Title"))
                {
                    lineNo = i + 1;
                    break;
                }
            }
            variableNames = lines[lineNo].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            variableNames.ForEach(d => d.ToLower());

            return lineNo + 2;

        }
        private void GetData(string[] lines, int dataStart)
        {
            // read the lines fron dataStart into the data structure
            // Get dates. Date can be a header or a combination of year, doy and hour (could alse be y. m, d, h)
            int dateTimeIndx = 0;
            int dateTimeCol = variableNames.IndexOf("date");
            if(dateTimeCol != -1) dateTimeIndx = 1;
            int hourCol = variableNames.IndexOf("hour");
            int yearCol = variableNames.IndexOf("year");
            int doyCol = variableNames.IndexOf("day_of_year");
            if (yearCol != -1 && doyCol != -1) dateTimeIndx = 2;
            foreach (string line in lines.Skip(dataStart))
            {
                // data.Add(line.)
                var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<double?> vals = items.Select(s => Double.TryParse(s, out double n) ? n : (double?)null).ToList();
                data.Add(vals);

                // date
                DateTime dateTime = new DateTime(2000, 1, 1);
                if(dateTimeIndx == 1)




                
            }
        }

    }
}
