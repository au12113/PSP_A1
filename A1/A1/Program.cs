using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> input = new List<double> { 160,591,114,229,230,270,128,1657,624,1503 };
            List<double> input2 = new List<double> { 15, 69.9, 6.5, 22.4, 28.4, 65.9, 19.4, 198.7, 38.8, 138.2 };
            Console.WriteLine("{0:N2}",stdDev(input));
            Console.ReadKey();
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
