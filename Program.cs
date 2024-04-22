using ASCIIMusicVisualiser8.Plugins;
using ASCIIMusicVisualiser8.Types.Interpolation;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Numerics;

namespace ASCIIMusicVisualiser8
{
    internal class Program
    {


        static void Main(string[] args)
        {
            /*
            graph.SetPoints(new System.Collections.Generic.List<InterpolationPoint>
                {
                    new InterpolationPoint(0, 0, "linear", new double[2] {2, 3}),
                    new InterpolationPoint(1, 0.5, "linear", new double[0]),
                    new InterpolationPoint(2, 1, "linear", new double[0])
                }
            );
            */

            
            
            Display display = CreateDisplay();
            display.Run();
            



            /*
            SwirlingTubes swirlingTubes = new SwirlingTubes();
            Console.WriteLine(StringifyCharlist(swirlingTubes.Generate(0)));
            */

        }

        public static Display CreateDisplay()
        {
            
            Display display = new Display(97*2, "Audio/level.wav", new Vector2(200, 50));

            
            
            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes());
            swirlingTubesGenerator.plugin.@class.ProcessParameterString("--size 120,60");
            display.AddGenerator(swirlingTubesGenerator);

            // Square 1
            Generator textGenerator = new Generator("Text Display Generator", new Square());

            string interpolationGraphString = "0>3.5;0>1;easeOutElastic;[] 3.5>7;1>0;easeOut;[4] 7>10.5;0>1;easeOutElastic;[] 10.5>14;1>0;easeOut;[4]";
            interpolationGraphString = Utility.RepeatPoints(interpolationGraphString, 8, 14);

            textGenerator.plugin.@class.ProcessParameterString($"--character . --sizeInterpolation {interpolationGraphString}");
            display.AddGenerator(textGenerator);

            // Square 2
            Generator textGenerator2 = new Generator("Text Display Generator 2", new Square());

            string interpolationGraphString2 = "0>3.5;1>0;easeOutElastic;[] 3.5>7;0>1;easeOut;[4] 7>10.5;1>0;easeOutElastic;[] 10.5>14;0>1;easeOut;[4]";
            interpolationGraphString2 = Utility.RepeatPoints(interpolationGraphString2, 8, 14);
            textGenerator2.plugin.@class.ProcessParameterString($"--character # --sizeInterpolation {interpolationGraphString2}");

            display.AddGenerator(textGenerator2);


            return display;
            
            
        }
    }
}


 
