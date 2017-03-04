using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4
{
    class Program
    {
        static void Main(string[] args)
        {
            //string filename = args[0];
            string filename = "testcase.txt";
            List<List<double>> inputData = GetData(filename);
            if(inputData.Count > 1)
            {
                inputData.Add(ratio(inputData));
            }
            inputData.Add(log(inputData.Last()));
            sizeRange(inputData.Last());
            Console.ReadKey();
        }

        /*BASED*/
        static List<List<double>> GetData(string filename)
        {
            string line;
            bool firstRow = true;   //trigger for initial List<double> at first line.
            int column = 0; //count column in each line.
            List<List<double>> inputData = new List<List<double>>();    //try to make 2-dimention list to keep data with dynamic table.
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@filename);
            while ((line = inputFile.ReadLine()) != null)
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
        /*BASED END*/

        /*ADDED*/
        static List<double> log(List<double> input)
        {
            List<double> logn = new List<double>();
            foreach(double item in input)
            {
                logn.Add(Math.Log(item, Math.E));
            }
            return logn;
        }
        /*ADDED END*/

        /*ADDED*/
        static double anti_log(double input)
        {
            return Math.Exp(input);
        }
        /*ADDED END*/

        /*ADDED*/
        static void sizeRange(List<double> input)
        {
            double mean = Mean(input);
            double SD = stdDev(input);
            Console.WriteLine("VS: {0:F4}, S: {1:F4}, M: {2:F4}, L: {3:F4}, VL: {4:F4}.",
                anti_log(mean - 2*SD), anti_log(mean - SD), anti_log(mean), anti_log(mean + SD), anti_log(mean + 2*SD));
        }
        /*ADDED END*/
        
        /*ADDED*/
        static List<double> ratio(List<List<double>> input)
        {
            List<double> ratio = new List<double>();
            for(int i = 0; i < input[0].Count; i++)
            {
                ratio.Add(input[0].ElementAt(i)/input[1].ElementAt(i));
            }
            return ratio;
        }
        /*ADDED END*/

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
