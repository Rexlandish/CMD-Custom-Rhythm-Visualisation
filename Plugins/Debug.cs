using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class Debug : Plugin, IPlugin
    {

        public override string pluginName { get => "Debug"; }

        public override void InitializeParameters() { }


        public override void Init() { }

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {

            List<List<OutputPixel>> finalArray = new();

            int xAmount = 32;
            int yAmount = 32;

            for (int offset = 0; offset < xAmount; offset++)
            {
                List<OutputPixel> currentList = new List<OutputPixel>();
                for (int i = 0; i < yAmount; i++)
                {
                    // 65 is A ascii
                    char currentChar = (char)(65 + (i + offset) % 26);
                    //char currentChar = ' ';
                    currentList.Add(new(currentChar));
                }
                finalArray.Add(currentList);
            }

            /*
             * YOUR CODE HERE
             */

            transparentChar = new OutputPixel(0);
            return finalArray;
        }

        public override string ShowParameterValues(double time)
        {
            return "";
        }
    }
}
