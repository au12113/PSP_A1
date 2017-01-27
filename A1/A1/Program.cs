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
            //List<double> input = new List<double> { 160,591,114,229,230,270,128,1657,624,1503 };
            //List<double> input2 = new List<double> { 15, 69.9, 6.5, 22.4, 28.4, 65.9, 19.4, 198.7, 38.8, 138.2};
            string inputFile = "input.txt";
            List<List<double>> inputData = GetData(inputFile);
            foreach(List<double> item in inputData)
            {
                Console.WriteLine(inputData.IndexOf(item));
                item.ForEach(Console.WriteLine);
            }
            //Console.WriteLine("{0:N2}",stdDev());            
            Console.ReadKey();
        }

        static List<List<double>> GetData(string filename)
        {
            string line;
            bool firstRow = true;
            int column = 0;
            List<List<double>> inputData = new List<List<double>>();
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@filename);
            while (( line = inputFile.ReadLine()) != null)
            {
                Console.WriteLine(line);
                string[] words = line.Split('\t');
                column = 0;
                if (firstRow)   //initial List<double> at first time.
                {
                    for (int i = 0; i < words.Count(); i++)
                    {
                        List<double> dataColumn = new List<double>();
                        inputData.Add(dataColumn);
                    }
                    firstRow = false;
                }
                foreach (string item in words)
                {
                    //Console.WriteLine(item);
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
