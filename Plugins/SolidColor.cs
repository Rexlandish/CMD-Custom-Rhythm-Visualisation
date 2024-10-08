﻿using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8.Plugins
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

        public SolidColor()
        {

        }

        public SolidColor(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

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

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            char fillChar = Utility.Conversion.GetCharFromBrightness(brightness.GetTime(beat));
            List<List<OutputPixel>> finalArray = Utility.Creation.Create2DArray(new OutputPixel(fillChar), size);

            /*
             * YOUR CODE HERE
             */

            transparentChar = new OutputPixel(' ');
            return finalArray;
        }

        public override string ShowParameterValues(double time)
        {
            return $"-s <{size.X.ToString("0.0")},{size.Y.ToString("0.0")}> -b {brightness.GetTime(time).ToString("0.00")}";
        }
    }
}
