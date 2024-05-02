using ASCIIMusicVisualiser8.Plugins;
using System;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using System.Collections.Generic;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using ASCIIMusicVisualiser8.Effects;

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


            Display display = CreateEffectTestDisplay();
            //Display display = CreateDigitalDirectionsDisplay();
            //Display display = CreateCatDisplay();
            Console.ReadLine();
            display.Run();
            



            /*
            SwirlingTubes swirlingTubes = new SwirlingTubes();
            Console.WriteLine(StringifyCharlist(swirlingTubes.Generate(0)));
            */

        }


        public static Display CreateEffectTestDisplay()
        {

            Display display = new(100, "Audio/Disconnected.mp3", new Vector2(150, 150));

            
            Generator lyrics = new Generator("Lyrics", new Debug());
            //lyrics.plugin.@class.ProcessParameterStringPlugin("");


            Effect position = new Position();
            string xPosition = RepeatPoints("0;0;easeOutElastic 4;120;easeOutElastic", 10, 7);
            string yPosition = RepeatPoints("0;0;easeOutElastic 4;120;easeOutElastic", 10, 8);
            position.SetParameterString($"-xPI {xPosition} -yPI {yPosition}");
            lyrics.AddEffect(position);

            display.AddGenerator(lyrics);
            




            Generator shader = new Generator("Shader", new ShaderTest());
            shader.plugin.@class.ProcessParameterStringPlugin($"--size 150,150");
            
            /*
            Effect loopEffect = new Loop();

            string xParam = RepeatPoints("0;0;easeInOutSin 4;100;easeOutElastic;[5]", 32, 8);
            string yParam = RepeatPoints("2;0;easeInOutSin 6;50;easeInOutSin", 32, 8);

            loopEffect.SetParameterString($"-xOI {xParam} -yOI {yParam}");
            shader.AddEffect(loopEffect);
            */


            Effect loopEffectLyrics = new Loop();
            string xParamText = RepeatPoints("0;0;easeInOutSin 2;100;easeInOutSin 3;50;easeInOutSin", 32, 4);
            string yParamText = RepeatPoints("0;0;easeInOutSin 1;8;easeInOutSin 2.5;85;easeInOutSin", 32, 4);

            loopEffectLyrics.SetParameterString($"-yOI {xParamText} -xOI {yParamText}");
            shader.AddEffect(loopEffectLyrics);

            display.AddGenerator(shader);


            return display;
        }

        public static Display CreateCatDisplay()
        {
            Display display = new(100, "Audio/Disconnected.mp3", new Vector2(50, 50));

            Generator catDisplay = new Generator("Cat", new TextDisplay());
            string cat = "tuna\ntest\ntime";
            catDisplay.plugin.@class.ProcessParameterStringPlugin($"--words {cat} -wI 0;0");
            display.AddGenerator(catDisplay);

            return display;

        }

        public static Display CreateDigitalDirectionsDisplay()
        {
            
            Display display = new Display(158, "Audio/DigitalDirections.mp3", new Vector2(50, 50));


            /*
            // Square 2
            Generator square2 = new Generator("Square 2", new Square());

            string squareSizeInterpolation2 = "0>3.5;1>0;easeOutElastic;[] 3.5>7;0>1;easeOut;[4] 7>10.5;1>0;easeOutElastic;[] 10.5>14;0>1;easeOut;[4]";
            squareSizeInterpolation2 = RepeatPoints(squareSizeInterpolation2, 16, 14);
            square2.plugin.@class.ProcessParameterString($"--character ' --sizeInterpolation {squareSizeInterpolation2}");

            display.AddGenerator(square2);
            */

            /*
            Generator checkerGenerator = new Generator("Checkerboard", new Checkerboard());
            checkerGenerator.plugin.@class.ProcessParameterString("--size 200,50");
            display.AddGenerator(checkerGenerator);
            */

            /*
            // Square 1
            Generator square = new Generator("Square 1", new Square());

            string squareSizeInterpolation = "0>3.5;0>1;easeOutElastic;[] 3.5>7;1>0;easeOut;[4] 7>10.5;0>1;easeOutElastic;[] 10.5>14;1>0;easeOut;[4]";
            squareSizeInterpolation = Utility.RepeatPoints(squareSizeInterpolation, 16, 14);

            square.plugin.@class.ProcessParameterString($"--character . --sizeInterpolation {squareSizeInterpolation}");
            display.AddGenerator(square);

            */



            string isActiveInterpolationShader = new InterpolationGraph("0;1 41;0").ExportToString();
            Generator shaderTest = new Generator("Shader Test", new ShaderTest(), isActiveInterpolationShader);


            shaderTest.plugin.@class.ProcessParameterStringPlugin($"--size 50,50");
            display.AddGenerator(shaderTest);



            string isActiveInterpolationtTubes = new InterpolationGraph("0;0 41;1").ExportToString();
            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes(), isActiveInterpolationtTubes);
            swirlingTubesGenerator.plugin.@class.ProcessParameterStringPlugin("--size 50,50");
            display.AddGenerator(swirlingTubesGenerator);
            

            


            // Text




            Generator lyrics = new Generator("Text Display", new TextDisplay());

            string lyricsText = ParseStringToTextDisplay(
                new List<string>()
                    {   "",
                        "Above",
                        " (arriba)",
                        "\nOn",
                        " (encima)",
                        "\nBelow",
                        " (debajo)",
                        "\nBeside",
                        " (al lado)"
                        //beside al lado behind detras in front delante in dentro out afuera left a la izquierdas "
                    },
                '_'
                );
            Console.WriteLine(lyricsText);

            string lyricsTextInterpolation =
                "0;" + "0 " +
                "11;" + "1 " +
                "15;" + "2 " +
                "19;" + "3 " +
                "23;" + "4 " +
                "27;" + "5 " +
                "31;" + "6 " +
                "35;" + "7 " +
                "39;" + "8 " +
                "41;" + "0";



            Console.WriteLine(lyricsTextInterpolation);
            //lyricsTextInterpolation = RepeatPoints(lyricsTextInterpolation, 4, 16);
            lyrics.plugin.@class.ProcessParameterStringPlugin($"--words {lyricsText} --wordsInterpolation {lyricsTextInterpolation}");
            display.AddGenerator(lyrics);


            Generator noise = new Generator("Noise", new Noise());

            string noiseInterpolation =
            "0;" + "1;" + "easeIn;[32] " +
            "41;" + "0;" + "linear " +
            "42;" + "0.9;" + "easeIn;[10] " +
            "74;" + "0;" + "linear";

            noise.plugin.@class.ProcessParameterStringPlugin($"--size 50,50 -tI {noiseInterpolation}");
            display.AddGenerator(noise);

            return display;
            
        }
    }
}


 
