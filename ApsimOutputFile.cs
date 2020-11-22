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
        readonly List<DateTime> dates;
        List<string> variableNames;
        readonly List<List<double>> data;

        public ApsimOutputFile(string fileName)
        {
            dates = new List<DateTime>();
            data = new List<List<double>>();
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
            variableNames = lines[lineNo].ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return lineNo + 2;

        }
        private void GetData(string[] lines, int dataStart)
        {
            // read the lines fron dataStart into the data structure
            // Get dates. Date can be a header or a combination of year, doy and hour (could alse be y. m, d, h)
            int dateTimeIndx = 0;
            int dateTimeCol = variableNames.IndexOf("date");
            int hourCol = variableNames.IndexOf("hour");
            int yearCol = variableNames.IndexOf("year");
            int doyCol = variableNames.IndexOf("day_of_year");

            if (dateTimeCol != -1) dateTimeIndx = 1;
            else if (yearCol != -1 && doyCol != -1) dateTimeIndx = 2;

            foreach (string line in lines.Skip(dataStart))
            {
                // data.Add(line.)
                var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<double> vals = items.Select(s => Double.TryParse(s, out double n) ? n : (double)0.0).ToList();
                data.Add(vals);

                // date
                DateTime dateTime = new DateTime(2000, 1, 1);
                if (dateTimeIndx == 1)   // Date
                {
                    dateTime = Convert.ToDateTime(items[dateTimeCol]);
                }
                else if (dateTimeIndx == 2)   // year, doy
                {
                    dateTime =   new DateTime(Convert.ToInt32(vals[yearCol]), 1, 1).AddDays(Convert.ToInt32(vals[doyCol]) - 1);
                }
                if (hourCol != -1) dateTime.AddHours(vals[hourCol]);

                dates.Add(dateTime);



            }

        }
        public List<double> GetData(string varName)
        {
            List<double> vals = new List<double>();

            int varCol = variableNames.IndexOf(varName);

            foreach (List<double> dataLine in data)
            {
                vals.Add(dataLine[varCol]);
            }


            return vals;
        }
        public List<DateTime> GetDates()
        {
            return dates;
        }

    }
}
