/*Test Block Comment
sdfasdf
asdfasdfas
*/
/* ADDED */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    class Program
    {
        /* Define data type name and */
        public List<string> typeName = new List<string>{ "string", "int", "double", "float", "bool", "void", "const"};
        public List<string> accesstype = new List<string> { "public", "private", "protected", "internal", "static"};

        public class Added
        {
            public bool _isAddedLine { get; set; }
            public int _sizeOf { get; set; }
        }

        public class Modified
        {
            public bool _isModifiedLine { get; set; }
            public int _sizeOf { get; set; }
        }

        public class Deleted
        {
            public bool _isDeletedLine { get; set; }
            public int _sizeOf { get; set; }
        }

        public class Class
        {
            int item { get; set; }
            int sizeOf { get; set; }
        }

        public class Function
        {
            int item { get; set; }
            int sizeOf { get; set; }
        }

        static void Main(string[] args)
        {
            /* Read File */
            //string fileName = args[0];
            string fileName = "../../Program.cs";
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@fileName);
            

            string line;
            int count = 1;
            int commentLine = 0 , codeLine = 0;
            bool isBlockComment = false;
            List<List<int>> symbolOrder = new List<List<int>>(); //symbolOrder[0] is // order, symbolOrder[1] is /* order, symbolOrder[2] is */ order and symbolOrder[3] is " order.
            Added addedLine = new Added();
            Modified modifiedLine = new Modified();
            Deleted deletedLine = new Deleted();


            while ((line = inputFile.ReadLine()) != null)
            {
                line = line.Replace(" ",String.Empty);
                Console.WriteLine(line);
                getstringOrder(line, ref symbolOrder);
                commentLine = commentLine + isComment(line, ref isBlockComment, ref symbolOrder);
                codeLine = codeLine + isCode(line, ref isBlockComment, ref symbolOrder);
                isAdded(line,ref addedLine);
                isDeleted(line, ref deletedLine);
                isModified(line, ref modifiedLine);
                symbolOrder.Clear();   //Clear List for new line.
                count++;
            }
            Console.WriteLine("Code: {0} line(s), Comment: {1} line(s).", codeLine, commentLine);
            Console.WriteLine("Added: {0}.", addedLine._sizeOf);
            Console.ReadKey();
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
                    if (symbolOrder[0].Count > 0 && symbolOrder[1].Count > 0)
                    {
                        if (symbolOrder[3].First() < symbolOrder[0].First() && symbolOrder[3].First() < symbolOrder[1].First())
                            return 0;
                    }
                    else if (symbolOrder[1].Count > 0)
                    {
                        if (symbolOrder[3].First() < symbolOrder[1].First() && symbolOrder[3].Last() > symbolOrder[1].First())
                            return 0;
                    }
                    else if (symbolOrder[0].Count > 0)
                    {
                        if (symbolOrder[3].First() < symbolOrder[0].First() && symbolOrder[3].Last() > symbolOrder[0].First())
                            return 0;
                    }
                }
                if (symbolOrder[0].Count > 0 || symbolOrder[1].Count > 0)
                {
                    /* Line contains // and /*, then this line maybe has the start of block  */
                    if (symbolOrder[0].Count > 0 && symbolOrder[1].Count > 0)
                    {
                        /* found the start of BlockComment  */
                        if (symbolOrder[1].First() < symbolOrder[0].First())
                            isBlockComment = true;
                    }
                    else
                    {
                        if (symbolOrder[1].Count > 0)
                            isBlockComment = true;
                    }
                    if (symbolOrder[2].Count > 0)
                    {
                        if (symbolOrder[2].First() > symbolOrder[1].First())
                            isBlockComment = false;
                    }
                    return 1;
                }
                else
                    return 0;
            }
        }
        
        static int isCode(string line,ref bool isBlockComment, ref List<List<int>> symbolOrder)
        {
            if (line.Count() > 0)
            {
                if (isBlockComment)
                    return 0;
                else
                {
                    if (symbolOrder[0].Count > 0)
                    {
                        if (symbolOrder[0].First() < 2)
                            return 0;
                    }
                    else if (symbolOrder[1].Count > 0)
                    {
                        if (symbolOrder[1].First() < 2)
                            return 0;
                    }
                    else if (symbolOrder[3].Count > 0)
                    {
                        if (symbolOrder[3].First() == line.Count())
                            return 0;
                    }
                    return 1;
                }
            }
            else
                return 0;
        }
        
        public static void isAdded(string line,ref Added addedLine)
        {
            if(addedLine._isAddedLine)
            {
                if (line.Contains("/*ADDED*/"))
                    addedLine._isAddedLine = false;
                else
                    addedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*ADDED*/"))
                    addedLine._isAddedLine = true;
            }
        }

        public static void isModified(string line, ref Modified modifiedLine)
        {
            if (modifiedLine._isModifiedLine)
            {
                if (line.Contains("/*MODIFIED*/"))
                    modifiedLine._isModifiedLine = false;
                else
                    modifiedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*MODIFIED*/"))
                    modifiedLine._isModifiedLine = true;
            }
        }

        public static void isDeleted(string line, ref Deleted deletedLine)
        {
            if (deletedLine._isDeletedLine)
            {
                if (line.Contains("/*DELETED*/"))
                    deletedLine._isDeletedLine = false;
                else
                    deletedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*DELETED*/"))
                    deletedLine._isDeletedLine = true;
            }
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
/*ADDED*/