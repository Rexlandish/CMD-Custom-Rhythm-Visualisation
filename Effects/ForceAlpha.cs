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
    /// Forces a character to be rendered as transparent. Use '-c []' if you want the character to be a space.
    /// <b>Char</b>: The char to render as transparent. <i>(--char, -c)</i><br/>
    /// </summary>
    public class ForceAlpha : Effect
    {

        char forcedTransparentChar;

        public ForceAlpha() : base() { }
        public ForceAlpha(string parameterString) : base(parameterString) {}

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("char", new string[] {"--char", "-c"}, ""),
            };
        }

        public override string ShowParameterValues(double time)
        {
            return $"{forcedTransparentChar}";
        }

        public override void Init()
        {

            if (GetPluginParameter("char").givenUserParameter == "[]")
                forcedTransparentChar = '\0';
            else
                forcedTransparentChar = GetPluginParameter("char").givenUserParameter[0];
            name = $"Forced Alpha";
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();

            newTransparentChar = new('\0');
            newDrawPoint = drawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;
            return input;
        }
    }
}
