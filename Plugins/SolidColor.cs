using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class SolidColor : Plugin, IPlugin
    {
        /// <summary>
        /// <b>Size</b>: Dimensions for the shader to render on. <i>(--size, -s)</i>
        /// <b>Brightness</b>: How bright to render. <i>(--brightness, -b)</i>
        /// </summary>
        public override string pluginName {get => "SolidColor"; }

        Vector2 size;
        InterpolationGraph brightness;

        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("size", new string[] {"--size", "-s"}, ""),
                new PluginParameter("brightness", new string[] {"--brightnessInterpolation", "-bI"}, "")
            };
        }


        public override void Init()
        {

            brightness = new InterpolationGraph(GetPluginParameter("brightness").givenUserParameter);

            string[] vector = GetPluginParameter("size").givenUserParameter.Split(',');
            size = new Vector2(
                float.Parse(vector[0]),
                float.Parse(vector[1])
            );
            

            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);
            
        }

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            
            char fillChar = Utility.Conversion.GetCharFromDensity(brightness.GetTime(beat));
            List<List<char>> finalArray = Utility.Creation.Create2DArray(fillChar, size);

            /*
             * YOUR CODE HERE
             */

            transparentChar = ' ';
            return finalArray;
        }

    }
}
