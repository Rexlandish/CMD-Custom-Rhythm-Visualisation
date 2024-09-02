using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class Blank : Plugin, IPlugin
    {
        /// <summary>
        /// <b>Size</b>: Dimensions for the BLANK shader to render on. <i>(--size, -s)</i>
        /// </summary>
        public override string pluginName {get => "Blank"; }

        Vector2 size;


        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public Blank(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

        public Blank() {}


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

            List<List<OutputPixel>> finalArray = Utility.Creation.Create2DArray(new OutputPixel(0), size);

            /*
             * YOUR CODE HERE
             */

            transparentChar = new(0);
            return finalArray;
        }

        public override string ShowParameterValues(double time)
        {
            return "";
        }

    }
}
