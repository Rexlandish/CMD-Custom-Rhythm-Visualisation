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
            InterpolationGraph graph = new InterpolationGraph();
            /*
            graph.SetPoints(new System.Collections.Generic.List<InterpolationPoint>
                {
                    new InterpolationPoint(0, 0, "linear", new double[2] {2, 3}),
                    new InterpolationPoint(1, 0.5, "linear", new double[0]),
                    new InterpolationPoint(2, 1, "linear", new double[0])
                }
            );
            */

            graph.ImportFromString("0>1;0>0.5;linear;[2,3] 1>2;0.5>1;linear;[] 2>3;1>1;linear;[]");
            Console.WriteLine(graph.ExportToString());

            /*
            Display display = CreateDisplay();
            display.Run();
            */



            /*
            SwirlingTubes swirlingTubes = new SwirlingTubes();
            Console.WriteLine(StringifyCharlist(swirlingTubes.Generate(0)));
            */

        }

        public static Display CreateDisplay()
        {
            
            Display display = new Display(97, "Audio/level.wav", new Vector2(200, 50));

            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes());
            swirlingTubesGenerator.plugin.@class.ProcessParameterString("--size 20,60");
            //display.AddGenerator(swirlingTubesGenerator);

            return display;
            
            
        }
    }
}


 
