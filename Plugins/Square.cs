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

        public Square() { }
        public Square(string parameters)
        {
            Console.WriteLine($"Processing {parameters}");
            ProcessParameterStringPlugin(parameters);
            
        }

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            double size = Math.Round(sizeInterpolation.GetTime(beat) * 40 + 5);
            size = size < 0 ? 0 : size;
            /*
            List<char> numberString = new List<char>(size.ToString().ToCharArray());

            List<List<OutputPixel>> finalArray = new List<List<OutputPixel>>
            {
                numberString,
                numberString,
                numberString,
                numberString
            };
            */
            
            List<List<OutputPixel>> finalArray = Create2DArray(new OutputPixel(character), new((int)size, (int)size));


            transparentChar = new(' ');
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
