﻿using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    public class Loop : Effect
    {

        InterpolationGraph xOffsetInterpolation;
        InterpolationGraph yOffsetInterpolation;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("xOffsetInterpolation", new string[] {"--xOffsetInterpolation", "-xOI"}, ""),
                new PluginParameter("yOffsetInterpolation", new string[] {"--yOffsetInterpolation", "-yOI"}, ""),
            };
        }

        public override void Init()
        {
            xOffsetInterpolation = new InterpolationGraph(GetPluginParameter("xOffsetInterpolation").givenUserParameter);
            yOffsetInterpolation = new InterpolationGraph(GetPluginParameter("yOffsetInterpolation").givenUserParameter);
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar)
        {
            List<List<char>> xLoopedGrid = new List<List<char>>();

            // Get each row and loop it around
            // mod the loop amount
            double xDouble = xOffsetInterpolation.GetTime(beat);
            int x = (int)Math.Floor(Repeat(xDouble, input[0].Count));

            for (int i = 0; i < input.Count; i++)
            {
                var currentRow = input[i].GetRange(x, input[i].Count - x);
                
                currentRow.AddRange(input[i].GetRange(0, x));

                xLoopedGrid.Add(currentRow.GetRange(0, currentRow.Count));
            }
            

            
            List<List<char>> yLoopedGrid;
            // Rotate the list vertically
            double yDouble = yOffsetInterpolation.GetTime(beat);
            int y = (int)Math.Floor(Repeat(yDouble, input[0].Count));

            yLoopedGrid = xLoopedGrid.GetRange(y, xLoopedGrid.Count - y);
            
            yLoopedGrid.AddRange(xLoopedGrid.GetRange(0, y));

            


            newTransparentChar = transparentChar;
            return yLoopedGrid;
        }
    }
}
