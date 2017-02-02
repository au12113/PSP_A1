using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = args[0];
            ReadFile("../../Program.cs");
            Console.ReadKey();
        }

        static void ReadFile(string fileName)
        {
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@fileName);
            string line;
            int count = 1;
            bool isBlockComment = false;
            List<List<int>> symbolOrder = new List<List<int>>(); //symbolOrder[0] is "//" order, symbolOrder[1] is "/*" order, symbolOrder[2] is "*/" order and symbolOrder[3] is """ order.
            while (( line = inputFile.ReadLine()) != null)
            {
                line = line.Trim();
                getstringOrder(line, ref symbolOrder);
                Console.WriteLine("Line: {0}, {1}\ncomment: {2}", count, line.Trim(), isComment(line,ref isBlockComment, ref symbolOrder));
                symbolOrder[0].ForEach(Console.WriteLine);
                symbolOrder.Clear();   //Clear List for new line.
                count++;
            }
        }

        static void getstringOrder(string line, ref List<List<int>> symbolOrder)
        {
            symbolOrder.Add(findOrder(line, "//"));
            symbolOrder.Add(findOrder(line, "/*"));
            symbolOrder.Add(findOrder(line, "*/"));
            symbolOrder.Add(findOrder(line, "\""));
        }
        
        static int isComment(string line,ref bool isBlockComment, ref List<List<int>> symbolOrder)
        {
            //if (!line.Contains("//") && !line.Contains("/*") && !isBlockComment) return 0; else return 1;
            if (line != null)
            {
                if (isBlockComment)
                {
                    if (symbolOrder[3].Count < 1)  
                    {
                        /* found the end of BlockComment */
                        if (symbolOrder[2].Count > 0)
                            isBlockComment = false;
                    }
                    else //If line contains '"', then must check comment doesn't in qoute.
                    {
                        if (symbolOrder[3].First() > symbolOrder[2].First())    
                            isBlockComment = false;
                    }
                    return 1;
                }
                else
                {
                    if (symbolOrder[3].Count > 0)  //If line contains '"', then must check comment doesn't in qoute.
                    {
                    }
                    else
                    {
                        if(symbolOrder[0].Count > 0 || symbolOrder[1].Count > 0)
                        {
                            /* Line contains // and /*, then this line maybe has the start of block  */
                            if (symbolOrder[0].Count > 0 && symbolOrder[1].Count > 0)
                            {
                                /* found the start of BlockComment  */
                                if (symbolOrder[1].First() < symbolOrder[0].First())
                                    isBlockComment = true;
                            }
                            if (symbolOrder[1].Count > 0)
                                isBlockComment = true;
                            return 1;
                        }
                        else
                            return 0;
                    }
                }
            }
            else 
                return 0;
        }
        
        static int isCode(string line,bool isBlockComment)
        {
            return 0;
        }

        public static List<int> findOrder(string line, string searchString)
        {
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += searchString.Length)
            {
                index = line.IndexOf(searchString, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
