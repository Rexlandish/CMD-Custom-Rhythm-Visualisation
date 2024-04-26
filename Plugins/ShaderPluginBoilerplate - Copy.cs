using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    public class ShaderPluginBoilerplate : Plugin, IPlugin
    {

        public override string pluginName {get => "Swirling Tubes"; }

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
                new PluginParameter("size", new string[] {"--size", "-s"}, "")
            };
        }


        float step(float edge, float x)
        {
            return x > edge ? 1 : 0;
        }

        float clamp(float x, float min, float max)
        {
            return
                x < min ? min
                :
                x > max ? max
                :
                x;
        }

        float atan(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        float cos(float x)
        {
            return (float)Math.Cos(x);
        }

        float sin(float x)
        {
            return (float)Math.Sin(x);
        }

        float abs(float x)
        {
            return (float)Math.Abs(x);
        }

        float max(float x, float y)
        {
            return x > y ? x : y;
        }

        float length(Vector2 v)
        {
            return (float)v.Length();
        }

        float pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
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

                    finalArray[i][j] = GetCharFromDensity(opacity);
                }
            }

            transparentChar = ' ';
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
