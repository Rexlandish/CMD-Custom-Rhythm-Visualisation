using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Scales the image about the center to draw the text.
    /// <b>X Scale Interpolation</b>: Dwisott. <i>(--xPositionInterpolation, -xSI)</i><br/>
    /// <b>X Scale Interpolation</b>: Dwisott. <i>(--yPositionInterpolation, -ySI)</i><br/>
    /// </summary>
    public class Scale : Effect
    {

        InterpolationGraph scaleFactorInterpolation;
        InterpolationGraph xScaleInterpolation;
        InterpolationGraph yScaleInterpolation;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("scaleFactorInterpolation", new string[] { "--scaleFactorInterpolation", "-sFI"}, ""),
                new PluginParameter("xScaleInterpolation", new string[] { "--xScaleInterpolation", "-xSI"}, ""),
                new PluginParameter("yScaleInterpolation", new string[] { "--yScaleInterpolation", "-ySI"}, ""),
            };
        }

        public override void Init()
        {
            scaleFactorInterpolation = new InterpolationGraph(GetPluginParameter("scaleFactorInterpolation").givenUserParameter);
            //xScaleInterpolation = new InterpolationGraph(GetPluginParameter("xScaleInterpolation").givenUserParameter);
            //yScaleInterpolation = new InterpolationGraph(GetPluginParameter("yScaleInterpolation").givenUserParameter);
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, Vector2 drawPoint, out char newTransparentChar, out Vector2 newDrawPoint)
        {
            List<List<char>> finalGrid = new List<List<char>>();

            // Unused for now
            //int xScaleCenter = (int)Math.Round(xScaleInterpolation.GetTime(beat));
            //int yScaleCenter = (int)Math.Round(yScaleInterpolation.GetTime(beat));

            float scaleFactor = 1/(float)scaleFactorInterpolation.GetTime(beat);
            

            int originalWidth = input[0].Count;
            int originalHeight = input.Count;

            int scaledWidth = (int)Math.Floor(originalWidth / scaleFactor);
            int scaledHeight = (int)Math.Floor(originalHeight / scaleFactor);

            // Y, X
            Vector2 center = new(scaledWidth / 2f, scaledHeight/ 2f);

            // Populate finalGrid with nearest neighbour interpolation
            for (int i = 0; i < scaledHeight; i++)
            {
                List<char> currentRow = new List<char>();
                for (int j = 0; j < scaledWidth; j++)
                {
                    int xUnscaled = (int)Math.Round(j * scaleFactor);
                    int yUnscaled = (int)Math.Round(i * scaleFactor);

                    char charToAdd;

                        
                    if (yUnscaled < originalHeight && xUnscaled < originalWidth)
                    {
                        charToAdd = input[yUnscaled][xUnscaled];
                    }
                    else
                    {
                        charToAdd = transparentChar;
                    }

                    currentRow.Add(charToAdd);
                }
                finalGrid.Add(currentRow);
            }

            newTransparentChar = transparentChar;
            Vector2 offset = center - drawPoint;
            newDrawPoint = drawPoint;
            // 1 -> -center
            // 2 -> 

            return finalGrid;
        }
    }
}

