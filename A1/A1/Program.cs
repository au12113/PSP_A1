using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace A1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0];
            List<List<double>> inputData = GetData(inputFile);
            foreach(List<double> item in inputData)
            {
                Console.WriteLine("Column {0}\tMean: {1:N2}\tStd.Dev: {2:N2}", inputData.IndexOf(item),Mean(item) ,stdDev(item));
            }     
            Console.ReadKey();
        }

        static List<List<double>> GetData(string filename)
        {
            string line;
            bool firstRow = true;   //trigger for initial List<double> at first line.
            int column = 0; //count column in each line.
            List<List<double>> inputData = new List<List<double>>();    //try to make 2-dimention list to keep data with dynamic table.
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@filename);
            while (( line = inputFile.ReadLine()) != null)
            {
                string[] words = line.Split('\t');
                column = 0;
                if (firstRow)   //initial List<double> at first time.
                {
                    for (int i = 0; i < words.Count(); i++)
                    {
                        List<double> dataColumn = new List<double>();
                        inputData.Add(dataColumn);  //then add List<double> to parent(List<List<double>>)
                    }
                    firstRow = false;
                }
                foreach (string item in words)  //add converted data to each list.
                {
                    inputData[column].Add(Convert.ToDouble(item));
                    column++;
                }
                
            }
            inputFile.Close();
            return inputData;
        }

        static double Mean(List<double> data)
        {
            double sum = 0;
            foreach(double item in data)
            {
                sum += item;
            }
            return sum / data.Count;
        }

        static double stdDev(List<double> data)
        {
            double tmp = 0;
            double mean = Mean(data);
            foreach(double item in data)
            {
                tmp += ( item - mean) * ( item - mean);
            }
            return Math.Sqrt(tmp/(data.Count - 1));
        }
    }
}
