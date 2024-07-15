using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8.Plugins
{
    internal class Square : Plugin, IPlugin
    {
        public override string pluginName => "Square";

        InterpolationGraph sizeInterpolation;
        char character;

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            double size = Math.Round(sizeInterpolation.GetTime(beat) * 40 + 5);
            size = size < 0 ? 0 : size;
            /*
            List<char> numberString = new List<char>(size.ToString().ToCharArray());

            List<List<char>> finalArray = new List<List<char>>
            {
                numberString,
                numberString,
                numberString,
                numberString
            };
            */
            
            List<List<char>> finalArray = Create2DArray(character, new((int)size, (int)size));
            

            transparentChar = ' ';
            return finalArray;
        }

        public override void Init()
        {
            sizeInterpolation = new(GetPluginParameter("sizeInterpolation").givenUserParameter);
            character = GetPluginParameter("character").givenUserParameter[0];
            //sizeInterpolation.Print();
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("sizeInterpolation", new string[] {"--sizeInterpolation", "-sI"}, ""),
                new PluginParameter("character", new string[] {"--character", "-c"}, ""),
            };
        }

        public override string ShowParameterValues(double time)
        {
            return $"-sI {sizeInterpolation.GetTime(time)} -c {character}";
        }
    }
}
