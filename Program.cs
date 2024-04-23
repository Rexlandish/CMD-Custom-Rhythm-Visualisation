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

            
            
            Display display = CreateExampleDisplay();
            display.Run();
            



            /*
            SwirlingTubes swirlingTubes = new SwirlingTubes();
            Console.WriteLine(StringifyCharlist(swirlingTubes.Generate(0)));
            */

        }

        public static Display CreateExampleDisplay()
        {
            
            Display display = new Display(97*2, "Audio/level.wav", new Vector2(200, 50));
            
            // Square 2
            Generator square2 = new Generator("Square 2", new Square());

            string squareSizeInterpolation2 = "0>3.5;1>0;easeOutElastic;[] 3.5>7;0>1;easeOut;[4] 7>10.5;1>0;easeOutElastic;[] 10.5>14;0>1;easeOut;[4]";
            squareSizeInterpolation2 = Utility.RepeatPoints(squareSizeInterpolation2, 16, 14);
            square2.plugin.@class.ProcessParameterString($"--character ' --sizeInterpolation {squareSizeInterpolation2}");

            display.AddGenerator(square2);



            Generator checkerGenerator = new Generator("Checkerboard", new Checkerboard());
            checkerGenerator.plugin.@class.ProcessParameterString("--size 200,50");
            display.AddGenerator(checkerGenerator);

            // Square 1
            Generator square = new Generator("Square 1", new Square());

            string squareSizeInterpolation = "0>3.5;0>1;easeOutElastic;[] 3.5>7;1>0;easeOut;[4] 7>10.5;0>1;easeOutElastic;[] 10.5>14;1>0;easeOut;[4]";
            squareSizeInterpolation = Utility.RepeatPoints(squareSizeInterpolation, 16, 14);

            square.plugin.@class.ProcessParameterString($"--character . --sizeInterpolation {squareSizeInterpolation}");
            display.AddGenerator(square);

            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes());
            swirlingTubesGenerator.plugin.@class.ProcessParameterString("--size 200,50");
            display.AddGenerator(swirlingTubesGenerator);


            // Text
            Generator lyrics = new Generator("Text Display", new TextDisplay());
            string lyricsText = "just,just a,just a li,just a little,just a little bit,just a little bit of,just a little bit of pan,just a little bit of panda";


            string lyricsTextInterpolation =
                "0;" + "0 " +
                "0.5;" + "1 " +
                "1;" + "2 " +
                "1.5;" + "3 " +
                "2;" + "4 " +
                "3;" + "5 " +
                "3.5;" + "6 " +
                "4.5;" + "7";
            Console.WriteLine(lyricsTextInterpolation);
            lyricsTextInterpolation = Utility.RepeatPoints(lyricsTextInterpolation, 32, 7);
            lyrics.plugin.@class.ProcessParameterString($"--words {lyricsText} --wordsInterpolation {lyricsTextInterpolation}");

            display.AddGenerator(lyrics);
            



            return display;
            
            
        }
    }
}


 
