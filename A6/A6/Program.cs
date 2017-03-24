/************************************************************************************/
/* Program Assignment: A6                                                           */
/* Name: Wasupon Tangaskul                                                          */
/* Date 3/17/2017                                                                   */
/* Description: Binary search find x for p input                                    */
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A6
{
    class Program
    {
        /*ADDED*/
        static void Main(string[] args)
        {
            Console.Write("Pn: ");
            double pn = Convert.ToDouble(Console.ReadLine());
            Console.Write("Degree of freedom: ");
            double dof = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("x for Pn is: {0:F5}.",Binary_search(pn, dof));
            Console.ReadKey();
        }
        /*ADDED END*/

        /*ADDED*/
        static double Binary_search(double pn, double dof)
        {
            double x = 0;
            double d = 0.5;
            double px = minimize_Error(x,dof);
            if(Math.Abs(px - pn) <= 0.00001)
            {
                return x;
            }
            else if( px > pn)
            {
                return Too_high(pn, x, d, dof);
            }
            else
            {
                return Too_low(pn, x, d, dof);
            }
        }
        /*ADDED END*/

        /*ADDED*/
        static double Too_high(double pn, double x, double d, double dof)
        {
            x = x - d;
            double px = minimize_Error(x, dof);
            if (Math.Abs(px - pn) <= 0.00001)
            {
                return x;
            }
            else if (px > pn)
            {
                return Too_high(pn, x, d, dof);
            }
            else
            {
                return Too_low(pn, x, d/2, dof);
            }
        }
        /*ADDED END*/

        /*ADDED*/
        static double Too_low(double pn, double x, double d, double dof)
        {
            x = x + d;
            double px = minimize_Error(x, dof);
            if (Math.Abs(px - pn) <= 0.00001)
            {
                return x;
            }
            else if (px > pn)
            {
                return Too_high(pn, x, d/2, dof);
            }
            else
            {
                return Too_low(pn, x, d, dof);
            }
        }
        /*ADDED END*/

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
    }
}
