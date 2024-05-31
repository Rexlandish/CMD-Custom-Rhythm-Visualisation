using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Scales the image about the center to draw the text.
    /// <b>X Position Interpolation</b>: Dwisott. <i>(--xPositionInterpolation, -xPI)</i><br/>
    /// <b>y Position Interpolation</b>: Dwisott. <i>(--yPositionInterpolation, -yPI)</i><br/>
    /// </summary>
    public class Scale : Effect
    {

        InterpolationGraph scaleFactorInterpolation;
        InterpolationGraph xCenterInterpolation;
        InterpolationGraph yCenterInterpolation;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("scaleFactorInterpolation", new string[] { "--scaleFactorInterpolation", "-sFI"}, ""),
                new PluginParameter("xCenterInterpolation", new string[] { "--xCenterInterpolation", "-xCI"}, ""),
                new PluginParameter("yCenterInterpolation", new string[] { "--yCenterInterpolation", "-yCI"}, ""),
            };
        }

        public override void Init()
        {
            scaleFactorInterpolation = new InterpolationGraph(GetPluginParameter("scaleFactorInterpolation").givenUserParameter);
            xCenterInterpolation = new InterpolationGraph(GetPluginParameter("xCenterInterpolation").givenUserParameter);
            yCenterInterpolation = new InterpolationGraph(GetPluginParameter("yCenterInterpolation").givenUserParameter);
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar)
        {
            List<List<char>> finalGrid = new List<List<char>>();

            // Unused for now
            int xScaleCenter = (int)Math.Round(xCenterInterpolation.GetTime(beat));
            int yScaleCenter = (int)Math.Round(yCenterInterpolation.GetTime(beat));

            float scaleFactor = (float)scaleFactorInterpolation.GetTime(beat);
            

            int originalWidth = input[0].Count;
            int originalHeight = input.Count;

            int scaledWidth = (int)Math.Floor(originalWidth / scaleFactor);
            int scaledHeight = (int)Math.Floor(originalHeight / scaleFactor);

            // Populate finalGrid with nearest neighbour interpolation
            for (int i = 0; i < scaledHeight; i++)
            {
                List<char> currentRow = new List<char>();
                for (int j = 0; j < scaledWidth; j++)
                {
                    int xUnscaled = (int)Math.Round(j * scaleFactor);
                    int yUnscaled = (int)Math.Round(i * scaleFactor);

                    char charToAdd;
                    
                    try
                    {
                        
                        if (yUnscaled < originalHeight && xUnscaled < originalWidth)
                        {
                            charToAdd = input[yUnscaled][xUnscaled];
                        }
                        else
                        {
                            charToAdd = transparentChar;
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"{yUnscaled} {xUnscaled}");
                        throw new Exception();
                    }

                    currentRow.Add(charToAdd);
                }
                finalGrid.Add(currentRow);
            }

            newTransparentChar = transparentChar;
            return finalGrid;
        }
    }
}

