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
            double x = Convert.ToDouble(Console.ReadLine());
            Console.Write("Degree of freedom: ");
            double dof = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("p is {0:F5}.",minimize_Error(x, dof));
            Console.ReadKey();
        }
        
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

        static double Simpson_rule(double x, double dof,int num_seg)
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
                / (Math.Sqrt(dof * Math.PI) * Gamma_function(dof / 2));
        }

        static double Gamma_function(double x)
        {
            if ( x == 1)
                return 1;
            if (x == 0.5)
                return Math.Sqrt(Math.PI);
            return (x - 1) * Gamma_function(x - 1);
        }

        static double Sum_TD_Odd(int n, double width, double dof)
        {
            double sum = 0;
            for(int i = 1; i <= n; i+=2)
            {
                sum += T_Distribution(i*width, dof);
            }
            return 4*sum;
        }

        static double Sum_TD_Even(int n, double width, double dof)
        {
            double sum = 0;
            for(int i = 2; i <= n; i+=2)
            {
                sum += T_Distribution(i*width, dof);
            }
            return 2*sum;
        }
    }
}
