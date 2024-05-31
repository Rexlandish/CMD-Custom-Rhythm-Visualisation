using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class Noise : Plugin, IPlugin
    {
        Random r = new Random();
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


        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(' ', size);




            
            //double randomBetween = NextUInt();
            
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {
                    //double opacity = randomBetween >= thresholdInterpolation.GetTime(beat) ? 1 * NextUInt() : 0;//1 * NextUInt() : 0;
                    double opacity = 0.5 < thresholdInterpolation.GetTime(beat) ? 1 * NextUInt() : 0;//1 * NextUInt() : 0;
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

        public float NextUInt()
        {
            uint t = (x ^ (x << 11));
            x = y;
            y = z;
            z = w;
            uint finalUInt = (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
            return finalUInt / 3999999999f - 0.1f;
        }
    }
}
