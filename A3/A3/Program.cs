using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3
{
    class Program
    {
        public class PROGRAM
        {
            double _estimatedProxySize { get; set; }
            double _planAddedModified { get; set; }
            double _actualAddedModified { get; set; }
            double _actualDevHours { get; set; }
        }

        enum COLUMN
        {
            EstimatedProxySize,
            PlanAddedModified,
            ActualAddedModified,
            ActualDevelopemoentHours
        };

        static void Main(string[] args)
        {
            string filename = args[0];
            List<PROGRAM> inputProgram = GetData(filename);
        }

        /*BASE*/
        static List<PROGRAM> GetData(string filename) //MODIFIED
        {
            string line;
            /*trigger for initial List<double> at first line.*/
            bool firstRow = true;
            /*count column in each line.*/
            int column = 0;
            List<PROGRAM> inputData = new List<PROGRAM>();  //MODIFIED
            COLUMN columnName = new COLUMN();
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@filename);
            while ((line = inputFile.ReadLine()) != null)
            {
                /*note: input file must use '\t' to seperate each column.*/ 
                string[] words = line.Split('\t');
                column = 0;
                if (firstRow)   //initial List<double> at first time.
                {
                    for (int i = 0; i < words.Count(); i++)
                    {
                        PROGRAM dataColumn = new PROGRAM(); //MODIFIED
                        inputData.Add(dataColumn);  //then add List<double> to parent(List<List<double>>)
                    }
                    firstRow = false;
                }
                foreach (string item in words)  //add converted data to each list.
                {
                    switch(columnName)
                    {
                        case columnName.
                    }
                    column++;
                }

            }
            inputFile.Close();
            return inputData;
        }
        /*BASED END*/

            /*REUSED*/
            static double Mean(List<double> data)
        {
            double sum = 0;
            foreach (double item in data)
            {
                sum += item;
            }
            return sum / data.Count;
        }
        /*REUSED END*/

        /*REUSED*/
        static double stdDev(List<double> data)
        {
            double tmp = 0;
            double mean = Mean(data);
            foreach (double item in data)
            {
                tmp += (item - mean) * (item - mean);
            }
            return Math.Sqrt(tmp / (data.Count - 1));
        }
        /*REUSED END*/
    }
}
