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
    public class Repeat : Effect
    {
        
        InterpolationGraph xRepeatInterpolation;
        InterpolationGraph yRepeatInterpolation;


        public Repeat() : base() { }
        public Repeat(string parameterString) : base(parameterString) { }

        public override void InitializeParameters()
        {

            // Initialize variables here

            
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("xRepeatInterpolation", new string[] { "--xRepeatInterpolation", "-xRI"}, ""),
                new PluginParameter("yRepeatInterpolation", new string[] { "--yRepeatInterpolation", "-yRI"}, ""),
            };
        }

        public override string ShowParameterValues(double time)
        {
            return "...";
        }

        public override void Init()
        {
            name = "Repeat";



            xRepeatInterpolation = new InterpolationGraph(GetPluginParameter("xRepeatInterpolation").givenUserParameter);
            yRepeatInterpolation = new InterpolationGraph(GetPluginParameter("yRepeatInterpolation").givenUserParameter);
            
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();

            newTransparentChar = transparentChar;

            List<List<OutputPixel>> finalGrid = new List<List<OutputPixel>>();

            int xRepeat = (int)Math.Round(xRepeatInterpolation.GetTime(beat));
            int yRepeat = (int)Math.Round(yRepeatInterpolation.GetTime(beat));

            // Horizontal Repeat
            List<OutputPixel> currentRow = new List<OutputPixel>();
            
            int originalHeight = input.Count;
            int originalWidth = input[0].Count;

            int targetHeight = originalHeight * yRepeat;
            int targetWidth = originalWidth * xRepeat;


            // Create a grid with the target size, and populate the items based on the original grid,
            // using mod of the width and height to fill in pixels in the extended regions

            finalGrid = Utility.Repeat.RepeatNTimesToListUnique(

                () => Utility.Repeat.RepeatNTimesToListUnique(() => new OutputPixel('.'), targetWidth),
                targetHeight

            );

            for (int i = 0; i < targetHeight; i++)
            {
                for (int j = 0; j < targetWidth; j++)
                {
                    var outputPixelToSampleFrom = input[i % originalHeight][j % originalWidth];
                    finalGrid[i][j] = new OutputPixel(outputPixelToSampleFrom.GetOutput());
                }
            }


            newTransparentChar = transparentChar;
            newDrawPoint = drawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;

            return finalGrid;
        }
    }
}

