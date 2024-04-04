using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8
{
    public class Utility
    {
        static string RepeatNTimes(string stringToRepeat, int repetitions)
        {
            return string.Concat(System.Linq.Enumerable.Repeat(stringToRepeat, repetitions));
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void GoToTopLeft()
        {
            Console.SetCursorPosition(0, 0);
        }

        public static void PrintList<T>(List<T> list)
        {
            list.ForEach(item => Console.Write(item.ToString() + " "));
        }

        public static string RepeatChar(char c, int times)
        {
            string finalString = "";
            for (int i = 0; i < times; i++)
            {
                finalString += c;
            }
            return finalString;
        }

        public static List<List<T>> Create2DArray<T>(T fillChar, Vector2 dimensions)
        {
            List<T> row = new List<T>(Enumerable.Repeat(fillChar, (int)dimensions.X).ToList());

            List<List<T>> finalList = new();
            for (int i = 0; i < (int)dimensions.Y; i++)
            {
                finalList.Add(new List<T>(row));
            }
            return finalList;
        }


        public static List<List<char>> CharlistifyString(string str) 
        {
            if (str == null)
            {
                throw new Exception("String is null?");
            }
            List<List<char>> finalList = new();
            foreach (string substring in str.Split('\n'))
            {
                finalList.Add(substring.ToCharArray().ToList());
            }
            return finalList;
        }

        public static string StringifyCharlist(List<List<char>> charList)
        {
            List<string> finalString = new();

            foreach (List<char> c in charList)
            {
                string currentRow = "";
                foreach (char c2 in c)
                {
                    currentRow += c2;
                }

                finalString.Add(currentRow);
                
            }

            return string.Join("\n", finalString);

        }

        public static Vector2 StringToVector2(string str)
        {
            string[] splitParams = str.Split(' ');
            return new Vector2(int.Parse(splitParams[0]), int.Parse(splitParams[1]));
        }

        public static double StringToDouble(string str)
        {
            return double.Parse(str);
        }

        // Remaps -1 1 to 0 1
        public static double Convert_11to01(double num)
        {
            return (num + 1) / 2;
        }

        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static double InverseLerp(double a, double b, double x)
        {
            return (x - a) / (b - a);
        }


    }

    internal class Maths
    {
        // Inverse lerps between two doubles
        public static double InverseLerp(double a, double b, double x)
        {
            return (x - a) / (b - a);
        }

        // Lerps between two doubles
        public static double Lerp(double a, double b, double x)
        {
            x = Clamp01(x);
            return a + (b - a) * x;
        }

        // Lerps between two vectors
        public static Vector2 Lerp(Vector2 a, Vector2 b, double x)
        {
            x = Clamp01(x);
            return a + (b - a) * (float)x; // Can't multiply Vector2 with double, it must be converted to float
        }

        // Clamps input between 0 and 1
        public static double Clamp01(double x)
        {
            if (x > 1)
            {
                return 1;
            }
            else if (x < 0)
            {
                return 0;
            }

            return x;
        }

        public static Vector2 RoundVector2(Vector2 v)
        {
            return new((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        public static Vector2 FloorVector2(Vector2 v)
        {
            return new((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }

        

    }
}
