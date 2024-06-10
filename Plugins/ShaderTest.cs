using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using static ASCIIMusicVisualiser8.Utility.ShaderOp;

namespace ASCIIMusicVisualiser8
{
    /// <summary>
    /// <b>Size</b>: Dimensions for the shader to render on. <i>(--size, -s)</i>
    /// </summary>
    public class ShaderTest : Plugin, IPlugin
    {

        public override string pluginName { get => "Shader Test"; }

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


        // https://www.shadertoy.com/view/mtVfR1
        float mainImage(Vector2 fragCoord, double beat)
        {
            /*
            fragCoord = new(fragCoord.Y, fragCoord.X);

            // Normalized pixel coordinates (from 0 to 1)
            Vector2 uv = (fragCoord * 2f - size) / size.Y;




            float d = sin(length(uv) * 12 - (float)beat);
            d = abs(d);
            d = sin(d + (cos(uv.X)) + (sin(uv.Y) + (float)beat));
            d = pow(d, 2);
            d = 1 - d;
            d = step(d, 0.5f);


            // Output to screen
            return d/30;
            */


            Vector2 uv = (fragCoord * 2.0f - size) / size.Y;

            float d = length(uv);

            d = sin(d * 50000 + (float)beat) / 10f;
            d = abs(d);

            d = 0.004f / d;

            return 2 * (d - 0.1f);
        }


        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(' ', size);

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

                    opacity = mainImage(new((float)_i, (float)_j), beat/2f);

                    finalArray[i][j] = GetCharFromDensity(opacity);
                }
            }

            transparentChar = ' ';
            return finalArray;
        }
    }
}
