using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    public class Position : Effect
    {

        InterpolationGraph xPositionInterpolation;
        InterpolationGraph yPositionInterpolation;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("xPositionInterpolation", new string[] { "--xPositionInterpolation", "-xPI"}, ""),
                new PluginParameter("yPositionInterpolation", new string[] { "--yPositionInterpolation", "-yPI"}, ""),
            };
        }

        public override void Init()
        {
            xPositionInterpolation = new InterpolationGraph(GetPluginParameter("xPositionInterpolation").givenUserParameter);
            yPositionInterpolation = new InterpolationGraph(GetPluginParameter("yPositionInterpolation").givenUserParameter);
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar)
        {
            List<List<char>> finalGrid = new List<List<char>>();

            int x = (int)Math.Round(xPositionInterpolation.GetTime(beat));
            int finalXLength = x + input[0].Count;

            int y = (int)Math.Round(yPositionInterpolation.GetTime(beat));
            int finalYLength = y + input.Count;

            
            for (int i = 0; i < finalYLength; i++)
            {
                finalGrid.Add(Utility.Repeat.RepeatNTimesToList(' ', finalXLength));
            }

            foreach (List<char> inputRow in input)
            {
                List<char> rowToAdd = Utility.Repeat.RepeatNTimesToList(' ', x);
                rowToAdd.AddRange(inputRow);

                finalGrid.Add(rowToAdd);
            }

            newTransparentChar = transparentChar;
            return finalGrid;
        }
    }
}

