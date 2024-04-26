using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class Noise : Plugin, IPlugin
    {

        public override string pluginName {get => "Noise"; }

        InterpolationGraph thresholdInterpolation;
        Vector2 size;
        string charShadeString = " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@";
        //" 123456789";
        // " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@"

        //" .:-=+*#%@";


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

            string[] vector = GetPluginParameter("size").givenUserParameter.Split(',');
            thresholdInterpolation = new InterpolationGraph(GetPluginParameter("thresholdInterpolation").givenUserParameter);


            size = new Vector2(
                float.Parse(vector[0]),
                float.Parse(vector[1])
            );
            

            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);
            
        }


        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(' ', size);




            var r = new Random();
            
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    double randomBetween = r.NextDouble();
                    double opacity = randomBetween >= thresholdInterpolation.GetTime(beat) ? 2 * r.NextDouble() : 0;
                    finalArray[i][j] = GetCharFromDensity(opacity);
                }
            }

            transparentChar = ' ';
            //transparentChar = new();
            return finalArray;
        }

        char GetCharFromDensity(double density)
        {
            density = 
                density < 0 ? 0 :
                density > 1 ? 1 :
                density;

            double index = Math.Round((charShadeString.Length - 1) * density);
            return charShadeString[(int)index];
        }
    }
}
