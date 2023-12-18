using System;
using System.Linq;
using FunctionParser;

namespace Пассивный_алгоритм
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = new string[] { "x" };
            string expression;
            Console.Write("Введите функцию: f(x) = ");
            bool property = false;
            do
            {
                expression = Console.ReadLine();
                if (!(property = Expression.IsExpression(expression, arg)))
                    Console.Write("Некорректное выражение! Попробуйте ещё: f(x) = ");
            }
            while (!property);
            Expression exp = new Expression(expression, arg, null);
            Console.Write("Интервал [a, b]:\na = ");
            double a = protect_double_input();
            Console.Write("b = ");
            double b = -double.MaxValue;
            while (b <= a) b = protect_double_input();
            Console.Write("Количество вычислений N = ");
            int N = 0;
            while (N <= 0) N = protect_int_input();
            double epsilon = 0;
            if (N % 2 == 0)
            {
                Console.Write("epsilon = ");
                while (epsilon <= 0) epsilon = protect_double_input();
            }
            double[] points = CreatePoints(a, b, N, epsilon);
            double[] values = CalcValues(points, exp);
            Console.WriteLine("Результаты: ");
            for (int j = 0; j < N + 2; j++)
                Console.WriteLine($"({Math.Round(points[j], 6)};\t{Math.Round(values[j], 6)})");
            int indexMin = 0;
            for (int j = 1; j <= N; j++)
                if (values[j] < values[indexMin]) indexMin = j;
            Console.WriteLine($"Отрезок локализации: [{Math.Round(points[indexMin - 1], 6)}; {Math.Round(points[indexMin + 1], 6)}]");
            double avg = (points[indexMin - 1] + points[indexMin + 1]) / 2;
            double result = exp.CalculateValue(new double[] { avg });
            Console.WriteLine($"Аппроксимация x*= {Math.Round(avg, 6)}");
            Console.WriteLine($"Результат f(x*) = {Math.Round(result, 6)}");
        }
        static double[] CreatePoints(double a, double b, int N, double epsilon = 0)
        {
            double[] points = new double[N + 2];
            points[0] = a;
            points[N + 1] = b;
            double interval = 0;
            if (N % 2 == 1)
            {
                interval = (b - a) / (N + 1);
                for (int j = 1; j <= N; j++)
                    points[j] = points[j - 1] + interval;
            }
            else
            {
                int k = N / 2;
                interval = (b - a) / (k + 1);
                for (int j = 1; j <= k; j++)
                {
                    points[2 * j] = points[2 * (j - 1)] + interval;
                    points[2 * j - 1] = points[2 * j] - epsilon;
                }
            }
            return points;
        }
        static double[] CalcValues(double[] points, Expression exp)
        {
            double[] values = new double[points.Length];
            for (int j = 0; j < points.Length; j++)
                values[j] = exp.CalculateValue(new double[] { points[j] });
            //values[j] = -points[j]*Math.Exp(-Math.Pow((points[j]-1), 2));
            return values;
        }
        static double protect_double_input()
        {
            double num = 0;
            while (true)
            {
                string text = Console.ReadLine();
                if (double.TryParse(text, out num)) break;
                Console.Write("Неверный формат! Попробуйте ещё: ");
            }
            return num;
        }
        static int protect_int_input()
        {
            int num = 0;
            while (true)
            {
                string text = Console.ReadLine();
                if (int.TryParse(text, out num)) break;
                Console.Write("Неверный формат! Попробуйте ещё: ");
            }
            return num;
        }
    }
}
