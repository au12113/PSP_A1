/************************************************************************************/
/* Program Assignment: A7                                                           */
/* Name: Wasupon Tangaskul                                                          */
/* Date 3/24/2017                                                                   */
/* Description: Predition Interval                                                  */
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A7
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "sample.txt";
            List<List<double>> input = GetData(filename);
            Console.Write("(0)Estimated Proxy Size, (1)Plan Added and Modified Size, (2)Actual Added and Modified Size, (3)Actual Development Hours\nx: ");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.Write("y: ");
            int y = Convert.ToInt32(Console.ReadLine());
            Console.Write("xk: ");
            double xk = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Rxy: {0:F9} \nR^2: {1:F9} \nTailArea: {2:F9}.", Rxy(input[x], input[y]),R2(input[x], input[y]), TailArea(input[x], input[y]));
            Console.WriteLine("B0: {0:F9} \nB1: {1:F9} \nyk: {2:F9} \nRange: {3:F9} \nUPI: {4:F9} \nLPI: {5:F9}.", B0(input[x], input[y]), B1(input[x], input[y]), Yk(input[x], input[y], xk), Range(input[x], input[y], xk), UPI(input[x], input[y], xk), LPI(input[x], input[y], xk));
            Console.ReadKey();
        }

        /*ADDED*/
        static double TailArea(List<double> x, List<double> y)
        {
            double xTail = Math.Abs(Rxy(x, y)) * Math.Sqrt((x.Count - 2) / (1 - R2(x ,y)));
            return 1 - (2 * (minimize_Error(xTail, x.Count - 2)));
        }
        /*ADDED END*/

        /*ADDED*/
        static double UPI(List<double> x, List<double> y, double xk)
        {
            return Yk(x, y, xk) + Range(x, y, xk);
        }
        /*ADDED END*/

        /*ADDED*/
        static double LPI(List<double> x, List<double> y, double xk)
        {
            return Yk(x, y, xk) - Range(x, y, xk);
        }
        /*ADDED END*/

        /*ADDED*/
        static double Range(List<double> x, List<double> y, double xk)
        {
            double sum_of_normalized = 0;
            double mean = Mean(x);
            foreach(int current_x in x)
            {
                sum_of_normalized = sum_of_normalized + ((current_x - mean) * (current_x - mean));
            }
            return Binary_search(0.35, x.Count - 2) * stdDev(x, y) * Math.Sqrt( 1 + (1 / x.Count) + ((xk - mean) * (xk - mean) /  sum_of_normalized));
        }
        /*ADDED END*/

        /*ADDED*/
        static double stdDev(List<double> x, List<double> y)
        {
            double sum_of_Error = 0;
            double b0 = B0(x, y);
            double b1 = B1(x, y);
            for (int index = 0; index < x.Count; index++)
            {
                sum_of_Error = sum_of_Error + ((y[index] - b0 - (b1 * x[index])) * (y[index] - b0 - (b1 * x[index])));
            }
            return Math.Sqrt(sum_of_Error / (x.Count - 2));
        }
        /*ADDED END*/

        /*REUSED*/
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
        /*REUSED END*/

        /*REUSED*/
        static double B1(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return (Sum(comparedColumn1, comparedColumn2) - (comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn2)))
                / (Sum(comparedColumn1, comparedColumn1) - (comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn1)));
        }
        /*REUSED END*/

        /*REUSED*/
        static double B0(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return Mean(comparedColumn2) - (B1(comparedColumn1, comparedColumn2) * Mean(comparedColumn1));
        }
        /*REUSED END*/

        /*REUSED*/
        static double Yk(List<double> comparedColumn1, List<double> comparedColumn2, double Xk)
        {
            return B0(comparedColumn1, comparedColumn2) + (B1(comparedColumn1, comparedColumn2) * Xk);
        }
        /*REUSED END*/

        /*REUSED*/
        static double Rxy(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return ((comparedColumn1.Count * Sum(comparedColumn1, comparedColumn2)) - (Sum(comparedColumn1) * Sum(comparedColumn2)))
                / Math.Sqrt(((comparedColumn1.Count * Sum(comparedColumn1, comparedColumn1)) - (Sum(comparedColumn1) * Sum(comparedColumn1)))
                * ((comparedColumn1.Count * Sum(comparedColumn2, comparedColumn2)) - (Sum(comparedColumn2) * Sum(comparedColumn2))));
        }
        /*REUSED END*/

        /*REUSED*/
        static double R2(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return Rxy(comparedColumn1, comparedColumn2) * Rxy(comparedColumn1, comparedColumn2);
        }
        /*REUSED END*/

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
        static double Sum(List<double> a, List<double> b)
        {
            double sum = 0;
            for (int i = 0; i < a.Count; i++)
            {
                sum += (a[i] * b[i]);
            }
            return sum;
        }
        /*REUSED END*/

        /*REUSED*/
        static double Sum(List<double> a)
        {
            double sum = 0;
            for (int i = 0; i < a.Count; i++)
            {
                sum += a[i];
            }
            return sum;
        }
        /*REUSED END*/

        /*REUSED*/
        static double minimize_Error(double x, double dof)
        {
            int num_seg = 10; //Default
            double previous = Simpson_rule(x, dof, num_seg);
            num_seg = 2 * num_seg;
            double current = Simpson_rule(x, dof, num_seg);
            while (current - previous > 0.00001)
            {
                previous = current;
                num_seg = 2 * num_seg;
                current = Simpson_rule(x, dof, num_seg);
            }
            return current;
        }
        /*REUSED END*/

        /*REUSED*/
        static double Simpson_rule(double x, double dof, int num_seg)
        {
            double width = Width(x, (double)num_seg);
            return (width / 3) * (T_Distribution(0, dof) + Sum_TD_Odd(num_seg - 1, width, dof)
                + Sum_TD_Even(num_seg - 2, width, dof) + T_Distribution(x, dof));
        }
        /*REUSED END*/

        /*REUSED*/
        static double Width(double x, double num_seg)
        {
            return x / num_seg;
        }
        /*REUSED END*/

        /*REUSED*/
        static double T_Distribution(double x, double dof)
        {
            return (Gamma_function((dof + 1) / 2) * Math.Pow((1 + (x * x / dof)), (-dof - 1) / 2))
                / (Math.Sqrt(dof * Math.PI) * Gamma_function(dof / 2));
        }
        /*REUSED END*/

        /*REUSED*/
        static double Gamma_function(double x)
        {
            if (x == 1)
                return 1;
            if (x == 0.5)
                return Math.Sqrt(Math.PI);
            return (x - 1) * Gamma_function(x - 1);
        }
        /*REUSED END*/

        /*REUSED*/
        static double Sum_TD_Odd(int n, double width, double dof)
        {
            double sum = 0;
            for (int i = 1; i <= n; i += 2)
            {
                sum += T_Distribution(i * width, dof);
            }
            return 4 * sum;
        }
        /*REUSED END*/

        /*REUSED*/
        static double Sum_TD_Even(int n, double width, double dof)
        {
            double sum = 0;
            for (int i = 2; i <= n; i += 2)
            {
                sum += T_Distribution(i * width, dof);
            }
            return 2 * sum;
        }
        /*REUSED END*/

        /*REUSED*/
        static double Binary_search(double pn, double dof)
        {
            double x = 1;
            double d = 0.5;
            double px = minimize_Error(x, dof);
            if (Math.Abs(px - pn) <= 0.0000000001)
            {
                return x;
            }
            else if (px > pn)
            {
                return Too_high(pn, x, d, dof);
            }
            else
            {
                return Too_low(pn, x, d, dof);
            }

        }
        /*REUSED END*/

        /*REUSED*/
        static double Too_high(double pn, double x, double d, double dof)
        {
            x = x - d;
            double px = minimize_Error(x, dof);
            if (Math.Abs(px - pn) <= 0.0000000001)
            {
                return x;
            }
            else if (px > pn)
            {
                return Too_high(pn, x, d / 2, dof);
            }
            else
            {
                return Too_low(pn, x, d, dof);
            }
        }
        /*REUSED END*/

        /*REUSED*/
        static double Too_low(double pn, double x, double d, double dof)
        {
            x = x + d;
            double px = minimize_Error(x, dof);
            if (Math.Abs(px - pn) <= 0.0000000001)
            {
                return x;
            }
            else if (px > pn)
            {
                return Too_high(pn, x, d / 2, dof);
            }
            else
            {
                return Too_low(pn, x, d, dof);
            }
        }
        /*REUSED END*/
    }
}
