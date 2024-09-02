using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8
{

    /// <summary>
    /// <b>Size</b>: Dimensions for the shader to render on. <i>(--size, -s)</i>
    /// <b>Threshold Interpolation</b>: Noise level. <i>(--thresholdInterpolation, -tI)</i>
    /// </summary>
    public class Noise : Plugin, IPlugin
    {
        Random r = new Random();
        public override string pluginName {get => "Noise"; }

        InterpolationGraph thresholdInterpolation;
        Vector2 size;

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
                new PluginParameter("thresholdInterpolation", new string[] {"--thresholdInterpolation", "-tI"}, "")
            };
        }

        public override void Init()
        {

            Seed(1641641641, 14388855, 1393858);

            string[] vector = GetPluginParameter("size").givenUserParameter.Split(',');
            thresholdInterpolation = new InterpolationGraph(GetPluginParameter("thresholdInterpolation").givenUserParameter);


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




            
            //double randomBetween = NextUInt();
            
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    //double opacity = randomBetween >= thresholdInterpolation.GetTime(beat) ? 1 * NextUInt() : 0;//1 * NextUInt() : 0;
                    double opacity = thresholdInterpolation.GetTime(beat) * NextUInt();//1 * NextUInt() : 0;
                    finalArray[i][j] = new((float)opacity);
                }
            }

            transparentChar = new(0);
            //transparentChar = new();
            return finalArray;
        }


        //https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        uint x;
        uint y;
        uint z;
        uint w;

        public void Seed(uint x, uint y, uint z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double NextUInt()
        {
            uint t = (x ^ (x << 11));
            x = y;
            y = z;
            z = w;
            uint finalUInt = (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
            return finalUInt / 3999999999f - 0.1f;
        }

        public override string ShowParameterValues(double time)
        {
            return $"-tI {thresholdInterpolation.GetTime(time)}";
        }
    }
}
