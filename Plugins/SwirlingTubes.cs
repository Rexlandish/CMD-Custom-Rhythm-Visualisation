using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility;

namespace ASCIIMusicVisualiser8
{
    public class SwirlingTubes : Plugin, IPlugin
    {

        public override string pluginName {get => "Swirling Tubes"; }

        Vector2 size;
        string charShadeString = " .:-=+*#%@";
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

        public double Sin01(double value)
        {
            return (Math.Sin(value) + 1)/2;
        }

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Utility.Create2DArray(' ', size);

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

        double HarshSin(double value)
        {
            float a = 0.5f;
            return Math.Sin(value) / (Math.Sqrt(a * a + Math.Sin(value) * Math.Sin(value)));
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
