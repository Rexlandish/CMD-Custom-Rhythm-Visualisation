using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8
{
    public class SwirlingTubes : Plugin, IPlugin
    {

        public override string pluginName {get => "Swirling Tubes"; }

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

            size = Utility.Conversion.StringToVector2(GetPluginParameter("size").givenUserParameter, ',');

            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);
            
        }

        public double Sin01(double value)
        {
            return (Math.Sin(value) + 1)/2;
        }

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(' ', size);

            double opacity;

            double swirliness = 8; // Character span
            double swirlDensity = 16; // Character span
            double tubeSpacing = 6; // i.e. Tube size
            double swirlSpeed = 4; // Horizontal movement

            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {

                    //swirlSpeed = 8 * Sin01(Math.Sin(beat)) + 8;

                    double _i = i;
                    double _j = j;

                    // Scrolling
                    _j += beat*8;


                    double wibbleAmount = Math.Cos(beat * Math.PI * 0.25);

                    double asidenessSin = swirliness * Sin01(wibbleAmount * Math.Sin(Math.PI * ((swirlSpeed * beat + _i ) / swirlDensity)));
                    double asidenessCos = swirliness * Sin01(wibbleAmount * Math.Cos(Math.PI * ((swirlSpeed * beat + _i ) / swirlDensity)));

                    double currentDensity =
                        Math.Sin((_j / tubeSpacing)  - asidenessSin) * (asidenessCos * 0.2);


                    finalArray[i][j] = GetCharFromDensity(currentDensity);
                }
            }

            transparentChar = ' ';
            return finalArray;
        }


    }
}
