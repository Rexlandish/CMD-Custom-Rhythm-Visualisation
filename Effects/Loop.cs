using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    public class Loop : Effect
    {

        InterpolationGraph xOffsetInterpolation;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("xOffsetInterpolation", new string[] {"--xOffsetInterpolation", "-xOI"}, ""),
            };
        }

        public override void Init()
        {
            xOffsetInterpolation = new InterpolationGraph(GetPluginParameter("xOffsetInterpolation").givenUserParameter);
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar)
        {
            List<List<char>> newChargrid = new List<List<char>>();

            // Get each row and loop it around
            double xDouble = Math.Round(xOffsetInterpolation.GetTime(beat));
            // mod the loop amount by 
            int x = (int)Math.Round(Repeat(xDouble, input[0].Count));
            //int x = (int)(xDouble % input[0].Count);
            //int x = (int)(xDouble);

            for (int i = 0; i < input.Count; i++)
            {
                var currentRow = input[i].GetRange(x, input[i].Count - x - 1);
                currentRow.AddRange(input[i].GetRange(0, x));

                newChargrid.Add(currentRow.GetRange(0, currentRow.Count));
            }

            newTransparentChar = transparentChar;
            return newChargrid;
        }
    }
}
