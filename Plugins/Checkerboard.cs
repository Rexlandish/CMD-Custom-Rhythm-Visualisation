using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Input;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class Checkerboard : Plugin, IPlugin
    {
        /// <summary>
        /// <b>Size</b>: Dimensions for the shader to render on. <i>(--size, -s)</i>
        /// <b>xSpeed</b>: X scroll speed. <i>(--xSpeed, -xS)</i>
        /// <b>ySpeed</b>: Y scroll speed. <i>(--ySpeed, -yS)</i>
        /// </summary>
        public override string pluginName {get => "Checkerboard"; }

        Vector2 size;

        float xSpeed;
        float ySpeed;

        // " 123456789";
        // " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@"
        // " .:-=+*#%@";


        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public Checkerboard(){}

        public Checkerboard(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }


        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("size", new string[] {"--size", "-s"}, ""),
                new PluginParameter("xSpeed", new string[] {"--xSpeed", "-xS"}, ""),
                new PluginParameter("ySpeed", new string[] {"--ySpeed", "-yS"}, "")
            };
        }

        public override void Init()
        {

            string[] vector = GetPluginParameter("size").givenUserParameter.Split(',');


            size = new Vector2(
                float.Parse(vector[0]),
                float.Parse(vector[1])
            );

            xSpeed = float.Parse(GetPluginParameter("xSpeed").givenUserParameter);
            ySpeed = float.Parse(GetPluginParameter("ySpeed").givenUserParameter);


            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);

        }

        public override string ShowParameterValues(double time)
        {
            return $"-s {size}";
        }


        public double Sin01(double value)
        {
            return (Math.Sin(value) + 1)/2;
        }

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(new OutputPixel(0), size);

            double scrollspeedX = xSpeed;
            double scrollspeedY = ySpeed;


            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {

                    //swirlSpeed = 8 * Sin01(Math.Sin(beat)) + 8;

                    double _i = i;
                    double _j = j;


                    double opacity =
                        Math.Sin((_i + (beat * scrollspeedX)) / 8) *
                        Math.Sin((_j + (beat * scrollspeedY)) / 8)
                    ;

                    opacity /= 2 ;
                    opacity += 0.25;
                    //Math.Pow(opacity, 2);
                    //opacity = Saturate(opacity, 0);

                    

                    finalArray[i][j] = new((float)opacity);
                }
            }

            transparentChar = new(' ');
            return finalArray;
        }

        double Saturate(double value, double limit)
        {
            return value > limit ? 1 : 0;
        }

        double Clamp(double value)
        {
            value =
            value < 0 ? 0 :
            value > 1 ? 1 :
                value;

            return value;
        }

        double HarshSin(double value)
        {
            float a = 0.5f;
            return Math.Sin(value) / (Math.Sqrt(a * a + Math.Sin(value) * Math.Sin(value)));
        }

    }
}
