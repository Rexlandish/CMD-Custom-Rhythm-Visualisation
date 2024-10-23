using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Sets position to draw the text.
    /// <b>X Position Interpolation</b>: Dwisott. <i>(--xPositionInterpolation, -xPI)</i><br/>
    /// <b>y Position Interpolation</b>: Dwisott. <i>(--yPositionInterpolation, -yPI)</i><br/>
    /// <br/>
    /// 
    /// Rotates output by a given angle around a point.
    /// <b>Rotation Interpolation</b>: How much to rotate by. <i>(--rotationInterpolation, -rI)</i><br/>
    /// <br/>
    /// 
    /// Scales the image about the center to draw the text.
    /// <b>Scale Interpolation</b>: Dwisott. <i>(--scaleInterpolation, -sI)</i><br/>
    /// </summary>
    public class Opacity : Effect
    {

        InterpolationGraph opacityInterpolation;


        public Opacity() : base() {}
        public Opacity(string parameterString) : base(parameterString) {}

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("opacityInterpolation", new string[] { "--opacityInterpolation", "-oI"}, ""),
            };
        }

        public override void Init()
        {
            opacityInterpolation = new InterpolationGraph(GetPluginParameter("opacityInterpolation").givenUserParameter);
            name = "Opacity";
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();

            float opacityScale = (float)opacityInterpolation.GetTime(beat);

            List<List<OutputPixel>> currentGrid = input;
            OutputPixel currentTransparentChar = transparentChar;
            Vector2 currentDrawPoint = drawPoint;

            // Don't bother scaling if the opacity scale is 1
            if (opacityScale != 1)
            {
                for (int i = 0; i < currentGrid.Count; i++)
                {
                    var currentRow = currentGrid[i];
                    for (int j = 0; j < currentRow.Count; j++)
                    {
                        if (opacityScale == 0f)
                        {
                            currentRow[j] = currentTransparentChar;
                            continue;
                        }

                        var pixel = currentRow[j];
                        // Change the opacity only if it's transparent
                        if (!pixel.IsTransparent(transparentChar))
                            currentRow[j] = new(pixel.brightness * opacityScale);
                    }
                }
            }






            newTransparentChar = currentTransparentChar;
            newDrawPoint = currentDrawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;
            return currentGrid;
        }

    }
}

