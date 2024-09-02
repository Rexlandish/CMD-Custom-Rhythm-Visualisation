using ASCIIMusicVisualiser8.Types.Interpolation;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static ASCIIMusicVisualiser8.Generator;

namespace ASCIIMusicVisualiser8
{
    public static class Utility
    {
        public interface IStringable<T>
        {
            string ExportToString();
            T ImportFromString(string input);
        }

        public class ShaderOp
        {

            public static double HarshSin(double value)
            {
                float a = 0.5f;
                return Math.Sin(value) / (Math.Sqrt(a * a + Math.Sin(value) * Math.Sin(value)));
            }

            public static float step(float edge, float x)
            {
                return x > edge ? 1 : 0;
            }

            public static float clamp(float x, float min, float max)
            {
                return
                    x < min ? min
                    :
                    x > max ? max
                    :
                    x;
            }

            public static float atan(float y, float x)
            {
                return (float)Math.Atan2(y, x);
            }

            public static float cos(float x)
            {
                return (float)Math.Cos(x);
            }

            public static float sin(float x)
            {
                return (float)Math.Sin(x);
            }

            public static float abs(float x)
            {
                return (float)Math.Abs(x);
            }

            public static float max(float x, float y)
            {
                return x > y ? x : y;
            }

            public static float length(Vector2 v)
            {
                return (float)v.Length();
            }

            public static float pow(float x, float y)
            {
                return (float)Math.Pow(x, y);
            }


        }


        public class Repeat
        { 
            static string RepeatNTimes(string stringToRepeat, int repetitions)
            {
                return string.Concat(System.Linq.Enumerable.Repeat(stringToRepeat, repetitions));
            }

            public static List<T> RepeatNTimesToList<T>(T obj, int times)
            {
                List<T> finalList = new();
                for (int i = 0; i < times; i++)
                {
                    finalList.Add(obj);
                }
                return finalList;
            }

            public static T[] RepeatNTimesToArray<T>(T obj, int times)
            {
                T[] finalArray = new T[times];

                for (int i = 0; i < times; i++)
                {
                    finalArray[i] = obj;
                }
                return finalArray;
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


            public static List<InterpolationPoint> RepeatPoints(List<InterpolationPoint> points, int repeatAmount, double repeatTime)
            {
                List<InterpolationPoint> finalList = new();

                for (int i = 0; i < repeatAmount; i++)
                {
                    foreach (var point in points)
                    {
                        var newPoint = new InterpolationPoint(point.startTime + i * repeatTime, point.startValue, point.interpolationCurveName, point.curveParameters);

                        finalList.Add(newPoint);
                    }
                }

                return finalList;
            }


            public static string RepeatPoints(string interpolationPointsGraph, int repeatAmount, double repeatTime)
            {
                InterpolationGraph graph = new InterpolationGraph(interpolationPointsGraph);
                var repeatedPoints = RepeatPoints(graph.points, repeatAmount, repeatTime);
                graph.SetPoints(repeatedPoints);
                return graph.ExportToString();

            }
        }

        public class ConsoleOp
        { 
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
        
        }

        public class Conversion
        {


            static string charShadeString = " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@";

            static Dictionary<char, double> charShadeStringDict = new Dictionary<char, double>();
            
            public static bool initializeCharShadeStringDict = false;

            public static char GetCharFromBrightness(double density)
            {
                if (!initializeCharShadeStringDict)
                    throw new Exception("Char shade string dict not initialized! Please run Utility.Conversion.InitializeCharShadeStringDict() at the start of the program.");

                //"0123456789";
                //" `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@"

                //" .:-=+*#%@";


                density =
                    density < 0 ? 0 :
                    density > 1 ? 1 :
                    density;

                double index = Math.Round((charShadeString.Length - 1) * density);
                return charShadeString[(int)index];
            }

            public static void InitializeCharShadeStringDict()
            {

                initializeCharShadeStringDict = true;
            }
            

            public static double GetDensityFromChar(char c)
            {
                throw new Exception("PLEASE DO NOT USE GETDENSITYFROMCHAR ANYMORE; OUTPUTPIXEL HAS MADE THIS REDUNDANT!");
                if (!initializeCharShadeStringDict)
                    throw new Exception("Char shade string dict not initialized! Please run Utility.Conversion.InitializeCharShadeStringDict() at the start of the program.");

                double index = charShadeString.IndexOf(c);
                if (index == -1)
                {
                    return 1; // Default value if the character wasn't found in the charShadeString
                }
                // Round to the nearest 1/n

                double amount = index / (charShadeString.Length - 1);

                
                return amount;

            }

            public static void PrintList<T>(List<T> list)
            {
                list.ForEach(item => Console.Write(item.ToString() + " "));
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

            public static string StringifyOutputPixel2DArray(List<List<OutputPixel>> op2DList)
            {
                List<string> finalString = new();

                foreach (List<OutputPixel> opList in op2DList)
                {
                    string currentRow = "";
                    foreach (OutputPixel op in opList)
                    {
                        currentRow += op.GetOutput();
                    }

                    finalString.Add(currentRow);

                }

                return string.Join("\n", finalString);

            }

            /*
            public static string StringifyCharlist(List<List<OutputPixel>> charList)
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
            */

            public static Vector2 StringToVector2(string str, char delimiter)
            {
                string[] splitParams = str.Split(delimiter);
                return new Vector2(float.Parse(splitParams[0]), float.Parse(splitParams[1]));
            }

            public static double StringToDouble(string str)
            {
                return double.Parse(str);
            }

            public static string ArrayToString<T>(T[] array, bool addBrackets = true)
            {
                string outputString =
                    addBrackets ? "[" + string.Join(",", array) + "]"
                    : string.Join(",", array);

                return outputString;

            }

            public static string List2DToString<T>(List<List<T>> array, bool addBrackets = false)
            {
                List<string> finalStringList = new();

                
                foreach (List<T> t in array)
                {
                    string currentString = string.Join("", t);
                    finalStringList.Add(currentString);
                }
                
                return string.Join("\n", finalStringList);
            }

            public static double[] StringToDoubleArray(string str, bool hasBrackets = true)
            {
                if (hasBrackets)
                    str = str.Substring(1, str.Length - 2);

                string[] elementsAsString = str.Split(',');
                double[] finalArray = new double[elementsAsString.Length];

                for (int i = 0; i < elementsAsString.Length; i++)
                {
                    finalArray[i] = double.Parse(elementsAsString[i]);
                }

                return finalArray;
            }

            public static string[] StringToStringArray(string str, bool hasBrackets = true, char delimiter = ',')
            {
                if (hasBrackets)
                    str = str.Substring(1, str.Length - 2);

                string[] elementsAsString = str.Split(delimiter);

                return elementsAsString;
            }

            public static List<InterpolationPoint> PointsFromLists(List<double> times, List<double> values, List<string> curveNames, List<double[]> curveVariables)
            {

                List<InterpolationPoint> finalList = new();
                foreach (var parameter in curveVariables)
                {
                    //Console.WriteLine(string.Join(",", parameter));
                }

                for (int i = 0; i < times.Count; i++)
                {
                    var newPoint = new InterpolationPoint(times[i], values[i], curveNames[i], curveVariables[i]);

                    foreach (var parameter in newPoint.curveParameters)
                    {
                        //Console.WriteLine(string.Join(",", parameter));
                    }

                    finalList.Add(newPoint);
                }

                return finalList;
            }

            // When the moon|hits your eye|like a big|pizza pie
            public static string ParseStringToTextDisplay(string text, char delimiter, char displayTextDelimiter)
            {

                List<string> phrases = new(text.Split(delimiter));
                return ParseStringToTextDisplay(phrases, displayTextDelimiter);


            }

            public static string ParseStringToTextDisplay(List<string> phrases, char displayTextDelimiter)
            {
                List<string> finalText = new();
                for (int i = 1; i <= phrases.Count; i++)
                {
                    string currentPhraseRange = string.Join("", phrases.GetRange(0, i));
                    //Console.WriteLine((i-1).ToString());
                    //Console.WriteLine(currentPhraseRange);
                    finalText.Add(currentPhraseRange);
                }

                return string.Join(displayTextDelimiter.ToString(), finalText);
            }
        }

        public class Creation
        {
            public static List<List<T>> Create2DArray<T>(T fillChar, Vector2 dimensions)
            {
                List<T> row = new List<T>(Enumerable.Repeat(fillChar, (int)dimensions.X).ToList());

                List<List<T>> finalList = new();
                for (int i = 0; i < (int)dimensions.Y; i++)
                {
                    // New list passes the list by value
                    finalList.Add(new List<T>(row));
                }
                return finalList;
            }

        }

        public class Maths
        {
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

            public static double Clamp(double value, double min, double max)
            {
                return 
                    value < min ? min
                    : value > max ? max
                    : value;
            }

            public static double Repeat(double t, double length)
            {
                return Clamp(t - Math.Floor(t / length) * length, 0, length);
            }
        }

        public class IO
        { 
            public static string ReadFromFile(string filepath)
            {
                using (StreamReader textFile = new StreamReader(filepath))
                {
                    return textFile.ReadToEnd();
                }
            }
        }

        public class Visualisation
        {
            public static Dictionary<BlendingMode, char> blendSymbolDictionary = new Dictionary<BlendingMode, char>()
            {
                {BlendingMode.Subtract, '-'},
                {BlendingMode.Multiply, '*'},
                {BlendingMode.Addition, '+'},
                {BlendingMode.Without, '!'},
                {BlendingMode.Behind, 'v'},
                {BlendingMode.InFront, ' '},
            };

            public static void PrintHierarchy(string name, string indent, bool last, bool hasChildren, bool addExtraLine, bool isActive, out string newIndent, ConsoleColor consoleColor = ConsoleColor.White)            {
                
                Console.Write(indent);

                newIndent = indent;

                if (last)
                {
                    Console.Write("└");
                    newIndent += "  ";
                }
                else
                {
                    Console.Write("├");
                    newIndent += "│ ";
                }

                if (isActive)
                {
                    Console.ForegroundColor = consoleColor;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                Console.WriteLine($"{name}                            ");
                Console.ForegroundColor = ConsoleColor.White;

                if (last & !hasChildren)
                {
                    Console.WriteLine(newIndent);
                }

            }
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
