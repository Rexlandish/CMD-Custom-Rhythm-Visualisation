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

            Display display = new(100, "Audio/Disconnected.mp3", new Vector2(100, 100));
            
            Generator text = new Generator("Lyrics", new TextDisplay());
            text.plugin.@class.ProcessParameterStringPlugin($"-wI 0;0 --words " +
                "1632 Three hundred colonists bound for New France depart from Dieppe, France.\n" +
                "1677 Scanian War DenmarkNorway captures the harbor town of Marstrand from Sweden.\n" +
                "1793 Kingdom of Prussia reconquers Mainz from France.\n" +
                "1813 Sir Thomas Maitland is appointed as the first Governor of Malta.");
/*
1821 – While the Mora Rebellion continues, Greeks capture Monemvasia Castle. Turkish troops and citizens are transferred to Asia Minor's coasts.
1829 – In the United States, William Austin Burt patents the typographer, a precursor to the typewriter.
1840 – The Province of Canada is created by the Act of Union.
1862 – American Civil War: Henry Halleck becomes general-in-chief of the Union Army.
1874 – Aires de Ornelas e Vasconcelos is appointed the Archbishop of the Portuguese colonial enclave of Goa, India. ");
*/

            /*
            Generator text = new Generator("Lyrics", new ShaderTest());
            text.plugin.@class.ProcessParameterStringPlugin($"--size 100,100");
            */

            Effect loopEffect = new Loop();

            string param = RepeatPoints("0;0;linear 32;100;linear", 32, 32);

            loopEffect.SetParameterString($"-xOI {param}");
            text.AddEffect(loopEffect);

            display.AddGenerator(text);

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


 
