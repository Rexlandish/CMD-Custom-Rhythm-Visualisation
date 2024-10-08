﻿using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class ShaderPluginBoilerplate : Plugin, IPlugin
    {

        public override string pluginName {get => "Blank"; }

        Vector2 size;


        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public ShaderPluginBoilerplate() { }
        public ShaderPluginBoilerplate(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("size", new string[] {"--size", "-s"}, "")
            };
        }

        public override void Init()
        {

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
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(new OutputPixel(0), size);

            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {

                    //swirlSpeed = 8 * Sin01(Math.Sin(beat)) + 8;

                    double _i = i;
                    double _j = j;
                    double opacity = 0;

                    /*
                     * 
                     * YOUR CODE HERE
                     * 
                     */

                    finalArray[i][j] = new((float)opacity);
                }
            }

            transparentChar = new(' ');
            return finalArray;
        }
        public override string ShowParameterValues(double time)
        {
            return "...";
        }

    }
}
