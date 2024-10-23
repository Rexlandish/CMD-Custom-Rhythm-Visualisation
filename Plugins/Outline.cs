using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Input;
using static ASCIIMusicVisualiser8.Utility;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class Outline : Plugin, IPlugin
    {
        /// <summary>
        /// <b>Size</b>: Dimensions for the shader to render on. <i>(--size, -s)</i>
        /// </summary>
        public override string pluginName {get => "Outline"; }

        InterpolationGraph xSizeGraph;
        InterpolationGraph ySizeGraph;
        char borderChar;

        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public Outline() { }

        public Outline(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("character", new string[] {"--character", "-c"}, ""),
                new PluginParameter("xSizeInterpolation", new string[] {"--xSizeInterpolation", "-xSI"}, ""),
                new PluginParameter("ySizeInterpolation", new string[] {"--ySizeInterpolation", "-ySI"}, "")
            };
        }


        public override void Init()
        {

            borderChar = GetPluginParameter("character").givenUserParameter[0]; // If the border char is a string, take the first character
            xSizeGraph = new InterpolationGraph(GetPluginParameter("xSizeInterpolation").givenUserParameter);
            ySizeGraph = new InterpolationGraph(GetPluginParameter("ySizeInterpolation").givenUserParameter);            
        }


        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {

            List<List<OutputPixel>> finalArray = new();
            int x = (int)Math.Round(xSizeGraph.GetTime(beat));
            int y = (int)Math.Round(ySizeGraph.GetTime(beat));

            // Initialize rows

            // ###########
            List<OutputPixel> capRow = Utility.Repeat.RepeatNTimesToList(new OutputPixel(borderChar), x);

            // #____________#
            List<OutputPixel> middleRow = new List<OutputPixel>();
            middleRow.Add(new OutputPixel(borderChar));
            middleRow.AddRange(Utility.Repeat.RepeatNTimesToList(new OutputPixel(0), x - 2));
            middleRow.Add(new OutputPixel(borderChar));


            finalArray.Add(capRow);
            for (int i = 0; i < y - 2; i++)
            {
                finalArray.Add(new List<OutputPixel>(middleRow));
            }
            finalArray.Add(capRow);

            /*
             * YOUR CODE HERE
             */

            transparentChar = new(0);
            return finalArray;
        }

        public override string ShowParameterValues(double time)
        {
            return "...";
        }

    }
}
