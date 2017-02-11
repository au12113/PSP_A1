using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3
{
    class Program
    {
        /*ADDED*/
        static void Main(string[] args)
        {
            //string filename = args[0];
            string filename = "../../testcase.txt";

            Console.Write("Xk: ");
            double xk = Convert.ToDouble(Console.Read());

            /*List[0]: Estimated Proxy Size, List[1]: Plan Added and Modified Size.*/
            /*List[2]: Actual Added and Nodified Size and List[3]: Actual Developemaent Hours.*/
            List<List<double>> inputProgram = GetData(filename);

            Console.WriteLine("B0: {0:0.000}, B1: {1:0.000}, \nrxy: {2:0.000}, r^2: {3:0.000}, yk: {4:0.000}.",
                B0(inputProgram[0], inputProgram[3]), B1(inputProgram[0], inputProgram[3]),
                Rxy(inputProgram[0], inputProgram[3]), R2(inputProgram[0], inputProgram[3]),
                Yk(inputProgram[0], inputProgram[3], xk));
            Console.ReadKey();
        }
        /*ADDED END*/

        /*BASE*/
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
                    for (int i = 0; i < words.Count() - 1; i++)                         //MODIFIED
                    {
                        List<double> dataColumn = new List<double>();
                        inputData.Add(dataColumn);  //then add List<double> to parent(List<List<double>>)
                    }
                    firstRow = false;
                }
                //foreach (string item in words)  //add converted data to each list.    //DELETED
                //{                                                                     //DELETED
                //   inputData[column].Add(Convert.ToDouble(item));                     //DELETED
                //    column++;                                                         //DELETED
                //}                                                                     //DELETED
                for(int i =1; i < words.Count(); i++)
                {
                    inputData[column].Add(Convert.ToDouble(words[i]));
                    column++;
                }
            }
            inputFile.Close();
            return inputData;
        }
        /*BASED END*/

        /*ADDED*/
        static double B1(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return (Sum(comparedColumn1, comparedColumn2)-(comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn2)))
                /(Sum(comparedColumn1, comparedColumn1) - (comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn1)));
        }
        /*ADDED END*/

        /*ADDED*/
        static double B0(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return Mean(comparedColumn2) - (B1( comparedColumn1,  comparedColumn2) * Mean(comparedColumn1));
        }
        /*ADDED END*/

        /*ADDED*/
        static double Yk(List<double> comparedColumn1, List<double> comparedColumn2, double Xk)
        {
            return B0(comparedColumn1, comparedColumn2) + (B1(comparedColumn1, comparedColumn2) * Xk);
        }
        /*ADDED END*/

        /*ADDED*/
        static double Rxy(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return ((comparedColumn1.Count * Sum(comparedColumn1, comparedColumn2)) - (Sum(comparedColumn1) * Sum(comparedColumn2)))
                / Math.Sqrt(((comparedColumn1.Count * Sum(comparedColumn1, comparedColumn1)) - (Sum(comparedColumn1) * Sum(comparedColumn1)))  
                * ((comparedColumn1.Count * Sum(comparedColumn2, comparedColumn2)) - (Sum(comparedColumn2) * Sum(comparedColumn2))));
        }
        /*ADDED END*/

        /*ADDED*/
        static double R2(List<double> comparedColumn1, List<double> comparedColumn2)
        {
            return Rxy(comparedColumn1, comparedColumn2) * Rxy(comparedColumn1, comparedColumn2);
        }

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

        /*ADDED*/
        static double Sum(List<double> a, List<double> b)
        {
            double sum = 0;
            for (int i = 0; i < a.Count; i++)
            {
                sum += (a[i] * b[i]);
            }
            return sum;
        }
        /*ADDED END*/

        /*ADDED*/
        static double Sum(List<double> a)
        {
            double sum = 0;
            for (int i = 0; i < a.Count; i++)
            {
                sum += a[i];
            }
            return sum;
        }
        /*ADDED END*/
    }
}
