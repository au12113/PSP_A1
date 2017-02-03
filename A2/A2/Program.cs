/*Test Block Comment
sdfasdf
asdfasdfas
*/
/* ADDED */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    class Program
    {
        /* Define data type name and */
        static List<string> TYPENAME = new List<string>{ "string", "int", "double", "float", "bool", "List"};
        static List<string> ACCESSTYPE = new List<string> { "public", "private", "protected", "internal", "static"};

        public class Added
        {
            public bool _isAddLine { get; set; }
            public int _sizeOf { get; set; }
            public Added()
            {
                _isAddLine = false;
                TYPENAME.Add("Added");
            }
        }

        public class Modified
        {
            public bool _isModifyLine { get; set; }
            public int _sizeOf { get; set; }
            public Modified()
            {
                _isModifyLine = false;
                TYPENAME.Add("Modified");
            }
        }

        public class Deleted
        {
            public bool _isDeleteLine { get; set; }
            public int _sizeOf { get; set; }
            public Deleted()
            {
                _isDeleteLine = false;
                TYPENAME.Add("Deleted");
            }
        }

        public class Class
        {
            public string _name { get; set; }
            public int _item { get; set; }
            public int _sizeOf { get; set; }
            public bool _isclass { get; set; }
            public int _bracket { get; set; }
            public Class()
            {
                _isclass = false;
                _sizeOf = 0;
                TYPENAME.Add("Class");
            }
            public void Clear()
            {
                _name = String.Empty;
                _item = 0;
                _sizeOf = 0;
                _isclass = false;
                _bracket = 0;
            }
        }

        public class Function
        {
            public string _name { get; set; }
            public int _item { get; set; }
            public int _sizeOf { get; set; }
            public bool _isfunction { get; set; }
            public int _bracket { get; set; }
            public Function()
            {
                _isfunction = false;
                _sizeOf = 0;
                TYPENAME.Add("Function");
            }
            public void Clear()
            {
                _name = String.Empty;
                _item = 0;
                _sizeOf = 0;
                _isfunction = false;
                _bracket = 0;
            }
        }

        static void Main(string[] args)
        {
            /* Read File */
            //string fileName = args[0];
            string fileName = "../../Program.cs";
            System.IO.StreamReader inputFile = new System.IO.StreamReader(@fileName);

            string line;
            int count = 1;
            int commentLine = 0;
            int codeLine = 0;
            bool isBlockComment = false;
            //symbolOrder[0] is // order, symbolOrder[1] is "/*" order, symbolOrder[2] is "*/" order and symbolOrder[3] is " order.
            List<List<int>> symbolOrder = new List<List<int>>(); 
            Added addedLine = new Added();
            Modified modifiedLine = new Modified();
            Deleted deletedLine = new Deleted();
            Function functionLine = new Function();
            Class classLine = new Class();

            while ((line = inputFile.ReadLine()) != null)
            {
                line = line.Trim();
                Console.WriteLine(line);
                getstrOrder(line, ref symbolOrder);
                commentLine = commentLine + isComment(line, ref isBlockComment, ref symbolOrder);
                codeLine = codeLine + isCode(line, ref isBlockComment, ref symbolOrder,ref functionLine,ref classLine);
                
                //isClass(line, ref classLine, ref symbolOrder);

                /*only added,deleted and modified use no space string */
                line = line.Replace(" ", String.Empty);
                isAdded(line, ref addedLine);
                isDeleted(line, ref deletedLine);
                isModified(line, ref modifiedLine);
                /*Clear List for next line.*/
                symbolOrder.Clear();   
                count++;
            }
            Console.WriteLine("Code: {0} line(s), Comment: {1} line(s).", codeLine, commentLine);
            Console.WriteLine("Added: {0}.", addedLine._sizeOf);
            Console.ReadKey();
        }
        
        static void getstrOrder(string line, ref List<List<int>> symbolOrder)
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
        
        static int isCode(string line,ref bool isBlockComment, ref List<List<int>> symbolOrder, ref Function functionLine,ref Class classLine)
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
                    isFunction(line, ref functionLine);
                    isClass(line, ref classLine);
                    return 1;
                }
            }
            else
                return 0;
        }
        
        public static void isAdded(string line,ref Added addedLine)
        {
            if(addedLine._isAddLine)
            {
                if (line.Contains("/*ADDED*/"))
                    addedLine._isAddLine = false;
                else
                    addedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*ADDED*/"))
                    addedLine._isAddLine = true;
            }
        }

        public static void isModified(string line, ref Modified modifiedLine)
        {
            if (modifiedLine._isModifyLine)
            {
                if (line.Contains("/*MODIFIED*/"))
                    modifiedLine._isModifyLine = false;
                else
                    modifiedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*MODIFIED*/"))
                    modifiedLine._isModifyLine = true;
            }
        }

        public static void isDeleted(string line, ref Deleted deletedLine)
        {
            if (deletedLine._isDeleteLine)
            {
                if (line.Contains("/*DELETED*/"))
                    deletedLine._isDeleteLine = false;
                else
                    deletedLine._sizeOf++;
            }
            else
            {
                if (line.Contains("/*DELETED*/"))
                    deletedLine._isDeleteLine = true;
            }
        }

        public static void isClass(string line, ref Class classLine)
        {
            if (classLine._isclass)
            {
                if (line == "}")
                {
                    classLine._bracket--;
                    classLine._sizeOf++;
                    if (classLine._bracket == 0)
                    {
                        Console.WriteLine("class Name: {0}, Size: {1}", classLine._name, classLine._sizeOf);
                        classLine.Clear();
                    }
                }
                else
                {
                    if (line == "{")
                        classLine._bracket++;
                    classLine._sizeOf++;
                    classLine._item += isItem(line, classLine._isclass);
                }
            }
            else
            {
                /*found accesstype typename and (), so it's class*/
                if (findAccessTypeinStr(line) && line.Contains("class") && !line.Contains("("))
                {
                    string[] tmp = line.Split(' ');
                    classLine._name = tmp.Last();
                    classLine._isclass = true;
                    classLine._sizeOf++;
                }
            }
        }

        public static void isFunction(string line, ref Function functionLine)
        {
            if(functionLine._isfunction)
            {
                if (line == "}")
                {
                    functionLine._bracket--;
                    functionLine._sizeOf++;
                    if (functionLine._bracket == 0)
                    {
                        Console.WriteLine("function Name: {0}, Size: {1}",functionLine._name,functionLine._sizeOf);
                         functionLine.Clear();
                    }
                }
                else
                {
                    if (line == "{")
                        functionLine._bracket++;
                    functionLine._sizeOf++;
                    functionLine._item += isItem(line, functionLine._isfunction);
                }
            }
            else
            {
                /*found accesstype typename and (), so it's function*/
                if (findAccessTypeinStr(line) && line.Contains("(")) 
                {
                    string[] tmp = Convert.ToString(Regex.Match(line, @" .*?\(")).Split(' ');
                    functionLine._name = tmp.Last().Replace("(",String.Empty);
                    functionLine._isfunction = true;
                    functionLine._sizeOf++;
                    functionLine._item += isItem(line,functionLine._isfunction);
                }
                /*if this line isn't function, so didn't count item in this line.*/
            }
        }

        /*use bool isFunction to check passed data in fuction*/
        public static int isItem(string line, bool isfunction)
        {
            int tmp = 0;
            foreach(string type in TYPENAME)
            {
                //note: Can't use new in comment, that after code section in same line
                if (line.Contains("new") && !line.Contains("\"new\""))
                {
                    tmp++;
                    break;
                }
                else if (line.Contains(type)&&!line.Contains("\""+type))
                {
                    //note: can't use variable type name cantains function or class name)
                    /*variable that declare in function*/
                    if (line.IndexOf(type) < 1)
                        tmp++;
                    else if (isfunction)
                    {
                        if (type != "List" && line.Contains("("))    //ignore "List", so use variable type in List to identify this item.
                        {
                            //function has parameter(s), so parameter(s) maybe one or more in same variable type.
                            List<int> itemOrder = findOrder(line, type);
                            for (int i = 0; i < itemOrder.Count; i++)
                            {
                                if( itemOrder[i] > line.IndexOf("("))
                                {
                                    tmp++;
                                }
                            }
                        }
                    }
                }
            }
            return tmp;
        }

        /*ues search in 0 to search with TYPENAME, and 1 to search with ACCESSTYPE*/
        public static bool findAccessTypeinStr(string line)
        {
            foreach (string item in ACCESSTYPE)
            {
                if (line.Contains(item))
                    return true;
            }
            return false;
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