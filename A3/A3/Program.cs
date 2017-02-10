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
        public class PROGRAM
        {
            public List<double> _estimatedProxy { get; set; }
            public List<double> _planAddedModified { get; set; }
            public List<double> _actualAddedModified { get; set; }
            public List<double> _actualDevHours { get; set; }
        }
        /*ADDED END*/

        static void Main(string[] args)
        {
            string filename = args[0];
            PROGRAM inputProgram = GetData(filename);
            Console.WriteLine("Test 1; B0: {0}, B1: {1}, rxy: {2}, r^2: {3}, yk: {4}",
                B1(ref inputProgram._estimatedProxy,ref inputProgram._actualAddedModified));
        }

        /*BASE*/
        static PROGRAM GetData(string filename) //MODIFIED
        {
            string line;
            /*trigger for initial List<double> at first line.*/
            //bool firstRow = true;                                                 //DELETED

            /*count column in each line.*/
            int column = 0;

            PROGRAM inputData = new PROGRAM();                          //MODIFIED
            PROGRAM tmpProgram = new PROGRAM();

            System.IO.StreamReader inputFile = new System.IO.StreamReader(@filename);
            while ((line = inputFile.ReadLine()) != null)
            {
                /*note: input file must use '\t' to seperate each column.*/ 
                string[] words = line.Split('\t');
                column = 0;
                //if (firstRow)   //initial List<double> at first time.             //DELETED
                //{                                                                 //DELETED
                //    for (int i = 0; i < words.Count(); i++)                       //DELETED
                //    {                                                             //DELETED
                //        List<double> dataColumn = new List<double>();             //DELETED
                //        inputData.Add(dataColumn);  //then add List<double> to parent(List<List<double>>)     //DELETED
                //    }                                                             //DELETED
                //    firstRow = false;                                             //DELETED
                //}                                                                 //DELETED
                foreach (string item in words)  //add converted data to each list. 
                {
                    switch(column)                                                  //ADDED
                    {                                                               //ADDED
                        case 0:                                                     //ADDED
                            tmpProgram._estimatedProxy.Add(Convert.ToDouble(item));    //ADDED
                            break;                                                  //ADDED
                        case 1:                                                     //ADDED
                            tmpProgram._planAddedModified.Add(Convert.ToDouble(item)); //ADDED
                            break;                                                  //ADDED
                        case 2:                                                     //ADDED
                            tmpProgram._actualAddedModified.Add(Convert.ToDouble(item));   //ADDED
                            break;                                                  //ADDED
                        case 3:                                                     //ADDED
                            tmpProgram._actualDevHours.Add(Convert.ToDouble(item));    //ADDED
                            break;                                                  //ADDED
                    }                                                               //ADDED
                    //inputData[column].Add(Convert.ToDouble(item));                //DELETED
                    column++;                                                       //DELETED
                }
            }
            inputFile.Close();
            return inputData;
        }
        /*BASED END*/

        /*ADDED*/
        static double B1(ref List<double> comparedColumn1, ref List<double> comparedColumn2)
        {
            return (Sum( ref comparedColumn1, ref comparedColumn2)-(comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn2)))
                /(Sum(ref comparedColumn1,ref comparedColumn1) - (comparedColumn1.Count * Mean(comparedColumn1) * Mean(comparedColumn1)));
        }
        /*ADDED END*/

        /*ADDED*/
        static double B0(ref List<double> comparedColumn1, ref List<double> comparedColumn2)
        {
            return Mean(comparedColumn1) - (B1(ref comparedColumn1, ref comparedColumn2) - Mean(comparedColumn2));
        }
        /*ADDED END*/

        /*ADDED*/
        static double Yk(ref List<double> comparedColumn1, ref List<double> comparedColumn2)
        {
            return B0(ref comparedColumn1, ref comparedColumn2) + (B1(ref comparedColumn1, ref comparedColumn2) * Mean(comparedColumn1));
        }
        /*ADDED END*/

        /*ADDED*/
        static double Rxy(ref List<double> comparedColumn1, ref List<double> comparedColumn2)
        {
            return ((comparedColumn1.Count * Sum(ref comparedColumn1, ref comparedColumn2)) - (Sum(ref comparedColumn1) * Sum(ref comparedColumn2)))
                / Math.Sqrt(((comparedColumn1.Count * Sum(ref comparedColumn1, ref comparedColumn1)) - (Sum(ref comparedColumn1) * Sum(ref comparedColumn1)))  
                * ((comparedColumn1.Count * Sum(ref comparedColumn2,ref comparedColumn2)) - (Sum(ref comparedColumn2) * Sum(ref comparedColumn2))));
        }
        /*ADDED END*/

        /*ADDED*/
        static double R2(ref List<double> comparedColumn1, ref List<double> comparedColumn2)
        {
            return Rxy(ref comparedColumn1, ref comparedColumn2) * Rxy(ref comparedColumn1, ref comparedColumn2);
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
        static double Sum(ref List<double> a, ref List<double> b)
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
        static double Sum(ref List<double> a)
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
