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
            public double _estimatedProxy { get; set; }
            public double _planAddedModified { get; set; }
            public double _actualAddedModified { get; set; }
            public double _actualDevHours { get; set; }
            public void Clear()
            {
                _estimatedProxy = 0;
                _planAddedModified = 0;
                _actualAddedModified = 0;
                _actualDevHours = 0;
            }
        }
        /*ADDED END*/

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
            //bool firstRow = true;                                                 //DELETED

            /*count column in each line.*/
            int column = 0;

            List<PROGRAM> inputData = new List<PROGRAM>();                          //MODIFIED
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
                            tmpProgram._estimatedProxy = Convert.ToDouble(item);    //ADDED
                            break;                                                  //ADDED
                        case 1:                                                     //ADDED
                            tmpProgram._planAddedModified = Convert.ToDouble(item); //ADDED
                            break;                                                  //ADDED
                        case 2:                                                     //ADDED
                            tmpProgram._actualAddedModified = Convert.ToDouble(item);   //ADDED
                            break;                                                  //ADDED
                        case 3:                                                     //ADDED
                            tmpProgram._actualDevHours = Convert.ToDouble(item);    //ADDED
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

        /*ADDED*/
        static double B1(ref List<PROGRAM> inputData, )
        {

        }
    }
}
