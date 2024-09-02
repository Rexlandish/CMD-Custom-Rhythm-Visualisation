using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Effect description
    /// <b>Variable name</b>: Explanation. <i>(flag, shortflag)</i><br/>
    /// </summary>
    public class EffectName : Effect
    {
        
        
        // Variables here
        InterpolationGraph interpolationGraph;

        public override void InitializeParameters()
        {

            // Initialize variables here

            /*
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("scaleFactorInterpolation", new string[] { "--scaleFactorInterpolation", "-sFI"}, ""),
                new PluginParameter("xCenterInterpolation", new string[] { "--xCenterInterpolation", "-xCI"}, ""),
                new PluginParameter("yCenterInterpolation", new string[] { "--yCenterInterpolation", "-yCI"}, ""),
            };
            */
        }

        public override string ShowParameterValues(double time)
        {
            return "...";
        }

        public override void Init()
        {
            name = "Effectname";

            /*
            scaleFactorInterpolation = new InterpolationGraph(GetPluginParameter("scaleFactorInterpolation").givenUserParameter);
            xCenterInterpolation = new InterpolationGraph(GetPluginParameter("xCenterInterpolation").givenUserParameter);
            yCenterInterpolation = new InterpolationGraph(GetPluginParameter("yCenterInterpolation").givenUserParameter);
            */
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();

            newTransparentChar = transparentChar;

            List<List<OutputPixel>> finalGrid = new List<List<OutputPixel>>();
            /*

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

            */
            newTransparentChar = transparentChar;
            newDrawPoint = drawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;

            return finalGrid;
        }
    }
}

