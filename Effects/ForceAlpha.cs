using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.OutputPixel;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Forces a character to be rendered as transparent. Use '-c []' if you want the character to be a space.
    /// <b>Char</b>: The char to render as transparent. <i>(--char, -c)</i><br/>
    /// </summary>
    public class ForceAlpha : Effect
    {

        char forcedTransparentChar;
        OutputPixel.OutputPixelType outputPixelType;

        public ForceAlpha() : base() { }
        public ForceAlpha(string parameterString) : base(parameterString) {}

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("char", new string[] {"--char", "-c"}, ""),
                new PluginParameter("outputPixelMode", new string[] {"--outputPixelMode", "-oPM"}, ""),
            };
        }

        public override string ShowParameterValues(double time)
        {
            return $"{forcedTransparentChar} {outputPixelType}";
        }

        public override void Init()
        {

            if (GetPluginParameter("char").givenUserParameter == "[]")
                forcedTransparentChar = ' ';
            else
                forcedTransparentChar = GetPluginParameter("char").givenUserParameter[0];

            string pixelMode = GetPluginParameter("outputPixelMode").givenUserParameter;
            outputPixelType =
                pixelMode == "b" ? OutputPixelType.BRIGHTNESS :
                pixelMode == "c" ? OutputPixelType.CHARACTER :
                OutputPixelType.CHARACTER;

            name = $"Forced Alpha";
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();

            switch (outputPixelType)
            {
                case OutputPixelType.CHARACTER:
                    newTransparentChar = new OutputPixel(forcedTransparentChar);
                    break;
                case OutputPixelType.BRIGHTNESS:
                    newTransparentChar = new OutputPixel((float)Utility.Conversion.GetBrightnessFromChar(forcedTransparentChar));
                    break;
                default:
                    throw new Exception($"Can't convert given forced alpha char! {forcedTransparentChar}");
            }

            
            newDrawPoint = drawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;
            return input;
        }
    }
}
