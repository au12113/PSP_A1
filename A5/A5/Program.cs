using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("X :");
            double x = Console.Read();
            Console.Write("Degree of freedom: ");
            double dof = Console.Read();
            Console.WriteLine("T_Distribution is {0:F5}.",minimize_Error(x, dof));
        }
        
        static double minimize_Error(double x, double dof)
        {
            int num_seg = 10; //Default
            double previous = Simpson_rule(x, dof, num_seg);
            double current = 0;
            do
            {
                num_seg = 2 * num_seg;
                current = Simpson_rule(x, dof, num_seg);
            }
            while (Math.Abs(current - previous) < 0.00001);
            return current;
        }

        static double Simpson_rule(double x, double dof,int num_seg) //Added num_seg, editted num_seg double -> int 3/9/60
        {
            double width = Width(x, (double)num_seg);
            return (width / 3) * (T_Distribution(0, dof) + Sum_TD_Odd(num_seg - 1, width, dof)
                + Sum_TD_Even(num_seg - 2, width, dof) + T_Distribution(x, dof));
        }

        static double Width(double x, double num_seg)
        {
            return x / num_seg;
        }

        static double T_Distribution(double x, double dof)
        {
            return (Gamma_function((dof + 1) / 2) * Math.Pow((1 + (x * x / dof)), (-dof - 1) / 2))
                / (Math.Pow(dof * Math.PI, 1 / 2) * Gamma_function(dof / 2));
        }

        static double Gamma_function(double x)
        {
            if ( x == 1)
                return 1;
            if (x == 1 / 2)
                return Math.PI;
            return (x - 1) * Gamma_function(x-1);
        }

        static double Sum_TD_Odd(int n, double width, double dof)
        {
            double sum = 0;
            for(int i = 1; i <= n; i+=2)
            {
                sum += T_Distribution(i*width, dof);
            }
            return sum;
        }

        static double Sum_TD_Even(int n, double width, double dof)
        {
            double sum = 0;
            for(int i = 2; i <= n; i+=2)
            {
                sum += T_Distribution(i*width, dof);
            }
            return sum;
        }
    }
}
