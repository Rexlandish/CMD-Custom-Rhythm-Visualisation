using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class Debug : Plugin, IPlugin
    {

        public override string pluginName { get => "Debug"; }

        public override void InitializeParameters() { }


        public override void Init() { }

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {

            List<List<char>> finalArray = new();

            int xAmount = 13;
            int yAmount = 13;

            for (int offset = 0; offset <= xAmount; offset++)
            {
                List<char> currentList = new List<char>();
                currentList.Add('|');
                for (int i = 0; i <= yAmount; i++)
                {
                    // 65 is A ascii
                    char currentChar = (char)(65 + i + offset);

                    currentList.Add(currentChar);
                }
                finalArray.Add(currentList);
            }

            /*
             * YOUR CODE HERE
             */

            transparentChar = new char();
            return finalArray;
        }
    }
}
