using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Media3D;
using ASCIIMusicVisualiser8.Types.Interpolation;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using NAudio.Dmo;

namespace ASCIIMusicVisualiser8.Plugins
{
    public static class ExampleDisplays
    {

        public static Display CreateDisplay()
        {
            //Display display = new Display(128, @"C:\Users\glass\Downloads\GlassChaek - School Issues\GlassChaek - School Issues - 02 Cover Teachers.mp3", new(64,64));
            int sizeX = 96;
            int sizeY = 96;
            Display display = new Display(new Vector2(sizeX, sizeY));

            Generator bg = new Generator("Background", new SolidColor());
            bg.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY}");// -bI 0;0.5;cos;[128] 128;1");


            Generator checker1 = new Generator("Checker 1", new Checkerboard(), blendingMode: Generator.BlendingMode.Addition);
            checker1.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY} -xS 2 -yS 4");

            Generator checker2 = new Generator("Checker 2", new Checkerboard(), blendingMode: Generator.BlendingMode.Subtract);
            checker2.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY} -xS 5 -yS -3");

            Generator fade = new Generator("BG Fade", new SolidColor(), blendingMode: Generator.BlendingMode.Multiply);
            fade.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY} -bI 0;0.01");

            bg.AddSubGenerators(checker1, checker2, fade);



            Generator gen = new Generator("Square Bounce", new Debug());

            Effect transform = new Transform();
            string rotation = RepeatPoints("0;0;easeOut;[4] 2;180 2;180;easeOut;[4] 4;0", 128, 4);
            string scale = RepeatPoints("0;1.5;easeOut;[2] 1;1 1;1.5;easeOut;[2] 2;1", 128, 2); ;
            transform.SetParameterString($"-xPI 0;{sizeX/2} -yPI 0;{sizeY/2} -rI {rotation} -sI {scale}");

            gen.AddEffect(transform);

            Generator tubes = new Generator("Tubes FX", new SwirlingTubes(), blendingMode: Generator.BlendingMode.Multiply);
            tubes.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY}");

            Generator squareBG = new Generator("Plain Square", new SolidColor(), blendingMode: Generator.BlendingMode.Addition);
            squareBG.plugin.@class.ProcessParameterStringPlugin($"-s {sizeX},{sizeY} -bI 0;0.1;cos;[256] 256;0");


            gen.AddSubGenerators(tubes, squareBG);


            // ---------------------------------

            string activeInterpolation = RepeatPoints($"0;1 4;1 4.5;0 5;1 5.5;0 6;1 6.5;0 7;1 7.5;0", 128, 8);
            Generator blank = new Generator("Image Switcher", new Blank(), activeInterpolation);
            blank.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY}");

            blank.AddSubGenerators(bg, gen);
            
            /*
            Generator fadeScreen = new Generator("Fading", new SolidColor(), blendingMode: Generator.BlendingMode.Subtract);
            string activeInterpolation = RepeatPoints($"0;0 4;0 4.5;1 5;0 5.5;1 6;0 6.5;1 7;0 7.5;1", 128, 8);
            fadeScreen.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY} -bI {activeInterpolation}");

            blank.AddSubGenerators(fadeScreen);
            */
            // ---------------------------------

            string catActiveInterpolation = RepeatPoints($"0;0 4;0 4.5;1 5;0 5.5;1 6;0 6.5;1 7;0 7.5;1", 128, 8);
            Generator catDance = new Generator("Cat Dance", new TextDisplay(), catActiveInterpolation);
            string catRepeatInterpolation = RepeatPoints($"0;0;linear 2;32", 128, 2);
            catDance.plugin.@class.ProcessParameterStringPlugin($"-w {Dance.catDance} -wI {catRepeatInterpolation} -d ,");

            Effect catTransform = new Transform();
            catTransform.SetParameterString($"-xPI 0;{sizeX/2} -yPI 0;{sizeY/2} -rI 0;0 -sI 0;0.9");

            catDance.AddEffect(catTransform);

            // ---------------------------------


            string rectActive = RepeatPoints("0;0 16;1", 8, 32);
            Generator solidColor = new Generator("Turning rect", new SolidColor(), rectActive);
            solidColor.plugin.@class.ProcessParameterStringPlugin($"--size 4,{sizeY/1.25} -bI 0;1");

            Effect rectTransform = new Transform();
            string rectTransformRepeat = RepeatPoints("0;0 16;0;linear 32;-720", 8, 32);
            rectTransform.SetParameterString($"-xPI 0;{sizeX/2} -yPI 0;{sizeY/2} -rI {rectTransformRepeat} -sI 0;1");

            solidColor.AddEffect(rectTransform);


            // Master display

            Generator blankMasterDisplay = new Generator("Master Blank", new Blank());
            blankMasterDisplay.plugin.@class.ProcessParameterStringPlugin($"--size {sizeX},{sizeY}");

            blankMasterDisplay.AddSubGenerators(solidColor, blank, catDance);

            Effect masterOffset = new Offset($"-xOI 0;0 32;0;cos;[8] 64;{sizeX} -yOI 0;0 32;0;cos;[8] 66;{sizeY}");
            Effect masterTransform = new Transform($"-xPI 0;{sizeX / 2} -yPI 0;{sizeY / 2} -rI 0;-30;cos;[8] 64;30 -sI 0;1.414");
            blankMasterDisplay.AddEffects(masterOffset, masterTransform);

            // Master dotsies

            // Dotsies
            //Generator dotsies = new Generator("Dotsies", new TextDisplay(), "0;0 16;1 32;1 48;1", Generator.BlendingMode.Without);
            //
            Generator dotsies = new Generator("Dotsies", new TextDisplay(), RepeatPoints($"0;1 0.5;0 1;1 1.5;0 2;1 2.5;0 3;1 3.5;0", 128, 8), Generator.BlendingMode.Subtract);

            string wordsInterpolation = RepeatPoints("0.5;0;linear 31.5;31", 32, 32);
            
            List<string> dotsiesTextList = new();
            foreach (string s in "You found the secret You found the secret You found the secret You found the secret You found the secret You found the secret You found the secret You found the secret".Split(' '))
            {
                dotsiesTextList.Add(Dotsies.GetDotsies(s, ' ', '@'));
            }
            string dotsiesText = string.Join(",",dotsiesTextList);

            dotsies.plugin.@class.ProcessParameterStringPlugin($"-w {dotsiesText} -d , -wI {wordsInterpolation}");

            /*
            Effect dotsiesTransform = new Transform($"-xPI 0;{sizeX / 2} -yPI 0;{sizeY / 2} -rI 0.5;30;cos;[64] 64.5;-30 -sI 0;8;bounce;[64] 64;14");
            */
            Effect dotsiesTransform = new Transform($"-xPI 0;{sizeX / 2} -yPI 0;{sizeY / 2} -rI 0;0 -sI 0;8");
            

            dotsies.AddEffects(dotsiesTransform);
            


            display.AddGenerators(blankMasterDisplay, dotsies);

            return display;

        }

        public static Display CreatePositionTestDisplay()
        {
            Display display = new Display(new(100, 100));
            throw new Exception();
            
        }

        public static Display CreateProceduralDisplay()
        {
            Display display = new Display(new(64, 64));

            display.AddGenerator(new Generator(
                "Bouncing Square",
                new SolidColor("--size 10,10"),
                effects:
                    new List<Effect> {
                        new Transform("") 
                    }
                )
            );

            return display;
        }

        public static Display CreateSmallDisplay()
        {
            Display display = new Display(new Vector2(3,3));

            var realBackgorund = new Generator(
                "Plain background",
                new SolidColor("-bI 0;0.1 -s 3,3")
            );

            var background = new Generator(
                "Rotating Text",
                new TextDisplay("-w 123\n456\n789 -wI 0;0 -d ,")
            );

            background.AddEffect(new Transform("-rI 0;0 2;-360;linear 20;360 -sI 0;1 -xPI 0;1 -yPI 0;1"));

            display.AddGenerators(realBackgorund, background);

            return display;

        }

        public static Display CreateMaskingDisplay()
        {
            int width = 72;
            int height = 72;

            Display display = new Display(new Vector2(width, height));


            Generator maskGroup = new Generator("Mask Group", new Blank($"-s {width},{height}"), blendingMode: Generator.BlendingMode.Subtract);

            Generator gen = new Generator("Mask1", new Divider($"-xPI 0;{width * 0.25};cos;[32] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[14] 256;{height * 0.75} -aI 0;-180;cos;[16] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);
            Generator gen1 = new Generator("Mask2", new Divider($"-xPI 0;{width * 0.25};cos;[17] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[21] 256;{height * 0.75} -aI 0;-180;cos;[10] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);
            Generator gen2 = new Generator("Mask3", new Divider($"-xPI 0;{width * 0.25};cos;[12] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[9] 256;{height * 0.75} -aI 0;-180;cos;[4] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);


            maskGroup.AddSubGenerators(gen, gen1, gen2);
            maskGroup.AddEffect(new ForceAlpha("-c []"));

            Generator bouncingImage = new Generator("Image Bounce", new Blank($"-s {width},{height}"));

            Generator targetImage = new Generator("Target Image", new Checkerboard($"-s {width},{height} -xS 3 -yS 3"));
            targetImage.AddEffect(
                new Transform($"-xPI 0;{width / 2} -yPI 0;{height / 2} -rI 0;0 -sI {RepeatPoints("0;1;bounce;[2] 2;3", 128, 2)}")
            );


            Generator bouncingImage1 = new Generator("Image Bounce 1", new Blank($"-s {width},{height}"));

            Generator invertMaskGroup = new Generator("Invert Mask Group", new Blank($"-s {width},{height}"), blendingMode: Generator.BlendingMode.Subtract);

            invertMaskGroup.AddSubGenerators(gen, gen1, gen2);

            Generator invert = new Generator("Invert Solid Color", new SolidColor($"-bI 0;1 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);
            

            invertMaskGroup.AddSubGenerators(invert);
            

            Generator targetImage1 = new Generator("Target Image 1", new SwirlingTubes($"-s {width},{height}"));

            targetImage1.AddEffect(
                new Transform($"-xPI 0;{width / 2} -yPI 0;{height / 2} -rI 0;0 -sI {RepeatPoints("0;3;bounce;[2] 2;1", 128, 2)}")
            );

            bouncingImage.AddSubGenerators(targetImage, invertMaskGroup);
            bouncingImage1.AddSubGenerators(targetImage1, maskGroup);


            display.AddGenerators(bouncingImage, bouncingImage1);
            

            return display;
        }
    
        public static Display CreateOutputPixelTestDisplay()
        {

            int width = 96;
            int height = 96;

            Display display = new Display(new Vector2(width, height));

            Generator gen = new Generator("Mask1", new Divider($"-xPI 0;{width * 0.25};cos;[32] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[14] 256;{height * 0.75} -aI 0;-180;cos;[16] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.InFront);
            Generator gen1 = new Generator("Mask2", new Divider($"-xPI 0;{width * 0.25};cos;[17] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[21] 256;{height * 0.75} -aI 0;-180;cos;[10] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);
            Generator gen2 = new Generator("Mask3", new Divider($"-xPI 0;{width * 0.25};cos;[12] 256;{width * 0.75} -yPI 0;{height * 0.25};cos;[9] 256;{height * 0.75} -aI 0;-180;cos;[4] 256;180 -s {width},{height}"), blendingMode: Generator.BlendingMode.Without);


            display.AddGenerators(gen, gen1, gen2);

            return display;

        }

        public static Display CreateEverythingSandwichDisplay()
        {
            Display display = new Display(new(16, 16));

            var checker = new Generator("Checker", new ShaderTest("-s 16,16"), "0;1 4;0 8;1 12;0", Generator.BlendingMode.InFront);
            checker.AddEffect(new Offset("-xOI 0;0;linear 16;16 -yOI 0;0;linear 16;16"));

            var divider = new Generator("Divider", new SwirlingTubes("-s 16,16"), blendingMode: Generator.BlendingMode.Without);

            display.AddGenerators(divider, checker);
            return display;

        }
    
        public static Display CreateTextTestDisplay()
        {

            var fp = new FontParser();
            fp.LoadFontJSON("./fonts/output.json");

            string text = fp.GenerateText("LOL");
            Console.ReadLine();

            Utility.Conversion.InitializeCharShadeStringDict();


            string example = """
            PLUGIN Checkerboard : -sY 4 -sX 4 -s 200,200
            FX ForceAlpha : -c 1
            """;

            //Display d = ExampleDisplays.CreateDisplay();

            Display d = new(new(120, 120));


            // Create the scratch from a list of beats
            string scratchRhythmBeatString = "0 0.5 0.75 1 1.5 2.25 2.5 3 3.25 3.5 3.75";
            string[] scratchRhythmBeats = scratchRhythmBeatString.Split(' ');
            int stateTracker = 0; // Keeps track of whether the square should be spun to the left (0) or right (1)

            List<string> scratchRhythmBeatsWithValues = new();

            foreach (string b in scratchRhythmBeats)
            {
                float angle = stateTracker == 0 ? -30 : stateTracker == 1 ? 30 : 0;

                scratchRhythmBeatsWithValues.Add($"{b};{angle};hold;[2]");

                stateTracker++;
                if (stateTracker == 2)
                {
                    stateTracker = 0;
                }
            }

            string scratchRhythm = string.Join(" ", scratchRhythmBeatsWithValues);
            Console.WriteLine(scratchRhythm);

            // Plugins -----------------------------------

            // Bouncing Square
            Generator bouncingSquare = new Generator(
                "Bouncing Square",
                new TextDisplay($"-w {text} -wI 0;0 -d ."),
                effects: new List<Effect>
                {
                    new Transform($"-xPI 0;49.5 -yPI 0;49.5 -rI {scratchRhythm} -sI 0;2;bounce;[16] 15;4")
                }
            );

            // Fizzy Overlay
            Generator fizzyOverlay = new Generator(
                "Fizzy Overlay",
                new ShaderTest("-s 100,100"),
                blendingMode: Generator.BlendingMode.Subtract
            );




            d.AddGenerators(fizzyOverlay, bouncingSquare);

            return d;
        }

        public static Display CreateBouDanDisplay()
        {
            Display d = new Display(new(120, 120));

            //@"(ameri)KA WO BUN DAN TO IU KOTO NAN DESU GA MAZU WA CHOTTO KOCHIRA NO"

            string boudan1 =
@"0 BOU
1 DAN
2 BUN
2.5 TO
2.75 IU
3 KO
3.25 TO
3.5 NAN
3.625 ---
3.75 NAN
3.825 ---
4.5 BOU
5 DAN
5.5 TO
5.75 IU
6 BUN
6.5 TO
6.75 IU
7 BUN
7.5 NAN
7.625 ---
7.75 NAN
7.825 ---
8 BOU
8.5 NAN
8.75 BUN
9.25 DAN
9.75 BOU
10.25 DAN
10.5 NAN
10.625 ---
10.75 NAN
10.825 ---
11 BUN
11.5 D
11.625 -
11.75 D
11.825 -
12 BUN
12.5 DOU
13 BUN
13.5 DOU
14 BUN
14.5 DOU
15 BUN
15.5 DOU";

            string boudan2 =
@"0.5 KO
0.75 CHI
1 BOU
1.75 DAN
2.5 CHO
2.75 TO
3 BUN
3.5 KO
3.75 CHI
4.5 BOU
5 DAN
5.5 D
5.75 DAN
6.25 D
6.5 D
6.75 D
7 DAN
7.5 DAN
8 CHO
8.25 TO 
8.5 BOU
9 DAN
9.5 KO
9.75 CHI
10 KO
10.25 CHI
10.5 DAN
11 DAN
11.5 NAN
11.75 NAN
12 CHO
12.25 TO
12.5 KO 
12.75 CHI
13 CHO
13.25 TO
13.5 KO
13.75 CHI
14 CHO
14.25 TO
14.5 KO
14.75 CHI
15 CHO
15.25 TO
15.5 KO
15.75 CHI";

            #region SECTION 1

            #region BOUDAN PARSING
            // Parse the above into an interpolation graph
            InterpolationGraph section1BoudanGraph = new InterpolationGraph();
            InterpolationGraph section1BoudanScaleGraph = new InterpolationGraph("0;1");

            InterpolationGraph wholeBoudan = new InterpolationGraph(); // Create a graph for the whole boudan text for use in section 5
            List<string> wholeBoudanText = new List<string>();

            List<string> section1BoudanText = new List<string>();

            FontParser fontParser = new FontParser();
            fontParser.LoadFontJSON("./fonts/output.json");

            int counter = 0;


            foreach (var pointString in boudan1.Split('\n'))
            {

                string[] b = pointString.Split(' ');

                float beat = float.Parse(b[0]);
                string text = b[1].Trim();
                section1BoudanGraph.points.Add(new InterpolationPoint(beat, counter, "hold", []));
                wholeBoudan.points.Add(new InterpolationPoint(beat, counter, "hold", []));
                /*
                section1BoudanScaleGraph.points.Add(new InterpolationPoint(beat, 2, "easeOut", [2]));
                section1BoudanScaleGraph.points.Add(new InterpolationPoint(beat + 0.1f, 3, "hold", []));
                */
                section1BoudanText.Add(fontParser.GenerateText(text));
                wholeBoudanText.Add(fontParser.GenerateText(text));
                counter++;
            }

            // For wholeBoudan
            foreach (var pointString in boudan2.Split('\n'))
            {

                string[] b = pointString.Split(' ');

                float beat = float.Parse(b[0]) + 16;
                string text = b[1].Trim();
                wholeBoudan.points.Add(new InterpolationPoint(beat, counter, "hold", []));
                wholeBoudanText.Add(fontParser.GenerateText(text));
                counter++;
            }


            section1BoudanGraph.Initialize();
            section1BoudanScaleGraph.Initialize();

            wholeBoudan.Initialize();

            Generator textDisplay = new Generator(
                "TextDisplay (Boudan)",
                new TextDisplay($"-w {string.Join(",", section1BoudanText)} -wI {section1BoudanGraph.ExportToString()} -d ,"),
                isActiveInterpolation: "0;1 16;0",
                effects:
                    new List<Effect>
                    {
                        //new Transform($"-xPI 0;60 -yPI 0;60 -rI 0;0 -sI {section1BoudanScaleGraph}"),
                        new Transform($"-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;3;bounce;[16] 16;3"),
                        new ForceAlpha("-c []")
                    }
                );

            #endregion


            #region BASS PARSE

            string[] bassValues =
                "0 0.75 1.5 2 2.75 3.5 4 4.75 5.5 6.25 7 8 8.75 9.5 10 10.75 11.5 12 13 14 15".Split(' ');

            InterpolationGraph bassBrightnessGraph = new InterpolationGraph("0;0");
            for (int i = 0; i < bassValues.Length; i++)
            {
                float time = float.Parse(bassValues[i]);
                float nextTime;

                // Set the next value as 0 if we know the next value
                if (i < bassValues.Length - 1)
                    nextTime = float.Parse(bassValues[i + 1]); // Get the time of the next value in the array
                else
                    nextTime = time + 0.1f;

                bassBrightnessGraph.points.Add(new InterpolationPoint(time, 0.02, "easeOut", [5]));
                bassBrightnessGraph.points.Add(new InterpolationPoint(nextTime, 0.01, "hold", []));


            }

            bassBrightnessGraph.Initialize();

            // Repeat twice
            bassBrightnessGraph = new InterpolationGraph(RepeatPoints(bassBrightnessGraph.ExportToString(), 2, 16));


            Generator bg = new Generator(
                "Background",
                new Checkerboard("-s 120,120 -xS 4 -yS 4"),
                isActiveInterpolation: "0;1 33;0",
                effects: new List<Effect>()
                {
                    new Opacity($"-oI {bassBrightnessGraph}"),
                    new ForceAlpha("-c [] -oPM b"),
                }

            );

            d.AddGenerators(bg);

            #endregion


            #region HAT PARSE

            string[] hatValues =
            ("0.5_+ " +
            "1.25_+ " +
            "2.5_+ " +
            "3.25_+ " +
            "4.5_+ " +
            "5.25_+ " +
            "6_+ " +
            "6.75_+ " +
            "7_x " +
            "7.5_x " +
            "8.5_+ " +
            "9.25_+ " +
            "10.5_+ " +
            "11.25_+ " +
            "12_x " +
            "12.5_x " +
            "13_x " +
            "13.5_x " +
            "14_x " +
            "14.5_x " +
            "15_x " +
            "15.5_x").Split(' ');

            // Parse the above
            InterpolationGraph hatBrightnessGraph = new InterpolationGraph("0;0");
            InterpolationGraph hatRotationGraph = new InterpolationGraph("0;0");
            for (int i = 0; i < hatValues.Length; i++)
            {
                string[] splitVar = hatValues[i].Split('_');
                float time = float.Parse(splitVar[0]);
                char rotationValue = splitVar[1][0]; // String to char, the string will always be length one so take the first char in the string array
                float nextTime;

                // Set the next value as 0 if we know the next value
                if (i < hatValues.Length - 1)
                    nextTime = float.Parse(hatValues[i + 1].Split('_')[0]); // Get the time of the next value in the array
                else
                    nextTime = time + 0.1f;

                hatBrightnessGraph.points.Add(new InterpolationPoint(time, 1, "easeOut", [10]));
                hatBrightnessGraph.points.Add(new InterpolationPoint(nextTime, 0, "hold", []));

                float rotation =
                    rotationValue == '+' ? 0 :
                    rotationValue == 'x' ? 45 :
                    15; // Random rotation to tell if someonething's gone wrong

                hatRotationGraph.points.Add(new InterpolationPoint(time, rotation, "hold", []));

            }

            // Repeat twice
            hatBrightnessGraph = new InterpolationGraph(RepeatPoints(hatBrightnessGraph.ExportToString(), 2, 16));
            hatRotationGraph = new InterpolationGraph(RepeatPoints(hatRotationGraph.ExportToString(), 2, 16));




            Generator hatCross = new Generator(
                "Hat Cross",
                new Outline("-xSI 0;16 -ySI 0;16 -c #"),
                isActiveInterpolation: "0;1 33;0",
                effects: new List<Effect>()
                {
                    new Transform($"-xPI 0;60 -yPI 0;60 -rI {hatRotationGraph.ExportToString()} -sI 0;7"),
                    new Opacity($"-oI {hatBrightnessGraph.ExportToString()}")
                }
                );

            d.AddGenerators(hatCross);

            #endregion





            #region SHOT PARSE
            Generator blankSquare = new Generator(
                "Blank Square",
                new SolidColor("-s 120,120 -bI 0;0"),
                isActiveInterpolation: "0;1 32;0",
                effects:
                new List<Effect>()
                    {
                        new Offset("-xOI 0;60 -yOI 0;60"),
                        new Transform("-xPI 0;0 -yPI 0;0 -rI 0;0 -sI 0;-1"),
                    }
                );

            string[] shotValues =
                ("0 0.25 1 1.75 " +
                "2.25 3 3.75 " +
                "4.25 5 5.75 6.5 7.25 7.5 7.75 " +
                "8.25 9 9.75 " +
                "10.25 11 11.75 " +
                "12.25 12.5 12.75 " +
                "13.25 13.5 13.75 " +
                "14.25 14.5 14.75 " +
                "15.25 15.5 15.75").Split(' ');

            // Parse the above
            InterpolationGraph shotGraph = new InterpolationGraph("0;0");
            shotGraph.points.Add(new InterpolationPoint(0, 1, "hold", []));
            for (int i = 0; i < shotValues.Length; i++)
            {
                Console.WriteLine(shotValues[i]);
                float val = float.Parse(shotValues[i]);
                float nextVal;

                // Set the next value as 0 if we know the next value
                if (i < shotValues.Length - 1)
                    nextVal = float.Parse(shotValues[i + 1]);
                else
                    nextVal = val + 1;

                shotGraph.points.Add(new InterpolationPoint(val, 1, "easeOut", [10]));
                shotGraph.points.Add(new InterpolationPoint(nextVal, 0, "hold", []));
            }

            // Repeat twice
            shotGraph = new InterpolationGraph(RepeatPoints(shotGraph.ExportToString(), 2, 16));

            Generator shotSquare = new Generator(
                "ShotSquare",
                new SolidColor($"-s 30,30 -bI {shotGraph.ExportToString()}"),
                effects: new List<Effect>()
                {
                    new Transform("-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;1 16;2")
                }
                );
            //blankSquare.AddSubGenerators(shotSquare);
            d.AddGenerators(blankSquare, textDisplay);

            #endregion


            #endregion


            #region SECTION 2

            Generator box1 = new Generator(
                "Box",
                new Outline("-xSI 0;13 -ySI 0;14 -c +"),
                isActiveInterpolation: "0;0 16;1 33;0",
                effects: new List<Effect>()
                {
                    //new Transform("-xPI 0;59.5 -yPI 0;59.5 -rI 0;0 -sI 0;1")
                    new Transform("-xPI 0;60 -yPI 0;60 -rI 0;0 32;0;easeOut;[2] 34;360 -sI 0;1 32;1;easeIn;[2] 33;0.1"),
                    new Opacity("-oI 0;1 32;1;linear 33;0"),
                    new ForceAlpha("-c #")
                }
            );

            Generator box2 = new Generator(
                "Box",
                new Outline("-xSI 0;40 -ySI 0;80 -c +"),
                isActiveInterpolation: "0;1 32;1",
                effects: new List<Effect>()
                {
                    //new Transform("-xPI 0;59.5 -yPI 0;59.5 -rI 0;0 -sI 0;1")
                    new Transform("-xPI 0;20 -yPI 0;20 -rI 0;0 -sI 0;1")
                }
            );


            // Parse boudan 2
            string[] boudan2TimeSheet = boudan2.Split('\n');

            List<float> boudan2Timings = new List<float>();
            List<string> boudan2Values = new List<string>();

            // Split text into timings and values
            foreach (string b in boudan2TimeSheet)
            {
                string[] timingValuePair = b.Split(' ');
                boudan2Timings.Add(float.Parse(timingValuePair[0]));
                boudan2Values.Add(timingValuePair[1].Trim());
            }

            List<string> boudan2DisplayStrings = new List<string>(); // The strings to display
            InterpolationGraph boudan2DisplayInterpolation = new InterpolationGraph("0;0");

            string currentDisplayPhrase = "";
            for (int i = 0; i < boudan2Timings.Count; i++)
            {
                if (i % 4 == 0 && i != 0) currentDisplayPhrase += "\n";
                currentDisplayPhrase += boudan2Values[i];
                boudan2DisplayStrings.Add(currentDisplayPhrase);

                boudan2DisplayInterpolation.points.Add(new InterpolationPoint(boudan2Timings[i] + 16, i, "hold", []));

            }
            //Console.WriteLine(string.Join(",", boudan2DisplayStrings));

            // Fill out each interpolation point's end timing + value
            boudan2DisplayInterpolation.Initialize();
            Console.WriteLine(boudan2DisplayInterpolation.ExportToString());


            Generator textBoxDisplay = new Generator(
                "TextBoxDisplayLeft",
                //new TextDisplay($"-w {string.Join(",", boudan2DisplayStrings)} -wI {boudan2DisplayInterpolation} -d ,"));
                new TextDisplay($"-w {string.Join(",", boudan2DisplayStrings)} -wI {boudan2DisplayInterpolation.ExportToString()} -d ,"),
                isActiveInterpolation: "0;0 16;1",
                effects: new List<Effect>()
                {
                    new Transform("-xPI 0;1 -yPI 0;1 -rI 0;0 -sI 0;-1")
                }
            );

            box1.AddSubGenerators(textBoxDisplay);
            d.AddGenerators(box1);
            #endregion

            #region SECTION 3

            #region ARROW GENERATION


            Generator arrowMovementContainer = new Generator(
                "Arrow Movement Container",
                new SolidColor("-s 19,16 -bI 0;0"),
                isActiveInterpolation: "0;0 31;1 96;0",
                effects: new List<Effect>()
                {   
                    new Transform("-xPI 0;0 -yPI 0;0 -rI 32;0 44;0;linear 48;360 48;0 60;0;linear 64;360 64;0 -sI 0;1"), // Rotation transform
                    new Repeat("-xRI 0;10 -yRI 0;20"),
                    new Transform("-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;1"), // Position transform
                    new Offset("-xOI 32;0;linear 64;32 64;256;linear 96;0 -yOI 0;0"),
                    new Opacity("-oI 0;1 31;0;linear 32;1 62;1;linear 64;0 64;0;easeOut;[2] 66;0.2"),
                }

            );

            Generator syllables = new Generator(
                "Syllables",
                new TextDisplay($"-w bou_\ndan_\nbun_\ndou_\nn___\nto__\nui__\nko__\nnan_\ncho_\nchi_ -wI 0;0 -d ."),
                effects: new List<Effect>()
                {
                    new Transform("-xPI 0;1 -yPI 0;2 -rI 0;0 -sI 0;-1")
                }
            );

            Generator chords = new Generator(
                "Chords",
                new TextDisplay("-w ___Fm6/9\n___Gm6/9\n__F#maj9\nG#maj7/D -wI 0;0 -d ."),
                effects: new List<Effect>()
                {
                    new Transform("-xPI 0;2 -yPI 0;10 -rI 0;0 -sI 0;-1")
                }
            );


            string[] arrowMovement =
@"0 1
1 1
2 2
2.5 2
2.75 2
3 2
3.25 2
3.5 2
3.75 2
4.5 1
5 1
6 2
6.5 2
6.75 2
7 2
7.5 2
7.75 2
8 1
8.5 3
8.75 3
9.25 1
9.75 1
10.25 1
10.5 4
10.75 4
11 1
11.5 1
11.75 1
12 3
12.5 1
13 3
13.5 1
14 3
14.5 1
15 3
15.5 1".Split('\n');

            InterpolationGraph arrowMovementChordsGraph = new InterpolationGraph();

            Dictionary<char, int> valueToPosition = new Dictionary<char, int>()
            {
                {'1', 0 },
                {'2', 1 },
                {'3', 2 },
                {'4', 3 },
                {'-', -6 }
            };

            // Parse the above and iterate through each line, creating an interpolation graph based on it
            for (int i = 0; i < arrowMovement.Length; i++)
            {
                string[] splitItem = arrowMovement[i].Split(' ');
                float time = float.Parse(splitItem[0]);
                char value = splitItem[1].Trim()[0]; // splitItem[1] returns a char as a string, get the first char of that

                float nextTime;
                if (i == arrowMovement.Length - 1)
                {
                    nextTime = time + 0.1f;
                }
                else
                {
                    nextTime = float.Parse(arrowMovement[i + 1].Split(' ')[0]);
                }
                int targetPosition = valueToPosition[value];

                arrowMovementChordsGraph.points.Add(new InterpolationPoint(time + 32, targetPosition + 3, "hold", []));
                arrowMovementChordsGraph.points.Add(new InterpolationPoint(((time + nextTime) / 2) + 32, -1, "hold", []));

            }

            arrowMovementChordsGraph = new InterpolationGraph(RepeatPoints(arrowMovementChordsGraph.ToString(), 2, 16));
            arrowMovementChordsGraph.Initialize();

            Generator arrowMovementChords = new Generator(
                "Arrow Movement (Chords)",
                new TextDisplay("-w [        ] -wI 0;0 -d ."),
                effects: new List<Effect>()
                {
                    new Transform($"-xPI {arrowMovementChordsGraph} -yPI 0;14 -rI 0;0 -sI 0;1")
                }

                );




            InterpolationGraph arrowMovementSyllablesGraph = new InterpolationGraph();


            //new TextDisplay($"-w bou_\ndan_\nbun_\ndou_\nn___\nto__\nui__\nko__\nto__\nnan_ -wI 0;0 -d ."),
            Dictionary<string, int> syllableToPosition = new Dictionary<string, int>()
            {
                {"BOU", 0 },
                {"DAN", 1 },
                {"D", 1 },
                {"BUN", 2 },
                {"DOU", 3 },
                {"N", 4 },
                {"TO", 5 },
                {"IU", 6 },
                {"KO", 7 },
                {"NAN", 8 },
                {"CHO", 9 },
                {"CHI", 10 },
                {"-", -3 },
                {"---", -3 }
            };

            // Parse boudan1 and iterate through each line, creating an interpolation graph based on it
            string[] boudan1Split = boudan1.Split('\n');
            for (int i = 0; i < boudan1Split.Length; i++)
            {
                string[] splitItem = boudan1Split[i].Split(' ');
                float time = float.Parse(splitItem[0]);
                string value = splitItem[1].Trim(); // splitItem[1] returns a char as a string, get the first char of that

                float nextTime;
                if (i == boudan1Split.Length - 1)
                {
                    nextTime = time + 0.1f;
                }
                else
                {
                    nextTime = float.Parse(boudan1Split[i + 1].Split(' ')[0]);
                }


                int targetPosition = syllableToPosition[value];

                arrowMovementSyllablesGraph.points.Add(new InterpolationPoint(time + 32, targetPosition + 2, "hold", []));
                //arrowMovementSyllablesGraph.points.Add(new InterpolationPoint(((time + nextTime) / 2) + 1, -1, "hold", []));

            }

            // Parse boudan2
            string[] boudan2Split = boudan2.Split('\n');
            for (int i = 0; i < boudan2Split.Length; i++)
            {
                string[] splitItem = boudan2Split[i].Split(' ');
                float time = float.Parse(splitItem[0]);
                string value = splitItem[1].Trim(); // splitItem[1] returns a char as a string, get the first char of that

                float nextTime;
                if (i == boudan2Split.Length - 1)
                {
                    nextTime = time + 0.1f;
                }
                else
                {
                    nextTime = float.Parse(boudan2Split[i + 1].Split(' ')[0]);
                }


                int targetPosition = syllableToPosition[value];

                arrowMovementSyllablesGraph.points.Add(new InterpolationPoint(time + 32 + 16, targetPosition + 2, "hold", []));
                arrowMovementSyllablesGraph.points.Add(new InterpolationPoint(((time + nextTime) / 2) + 32 + 16, -1, "hold", []));

            }
            arrowMovementSyllablesGraph.Initialize();

            Generator arrowMovementSyllables = new Generator(
                "Arrow Movement (Syllables)",
                new TextDisplay("-w [    ] -wI 0;0 -d ."),
                effects: new List<Effect>()
                {
                    new Transform($"-xPI {arrowMovementSyllablesGraph} -yPI 0;4 -rI 0;0 -sI 0;1")
                }

                );





            arrowMovementContainer.AddSubGenerators(syllables, chords, arrowMovementChords, arrowMovementSyllables);
            d.AddGenerators(arrowMovementContainer);

            #endregion

            #endregion

            #region SECTION 4

            #region CHORD TEXT

            // Chord text display

            string chordText =
@"


                                                                                                   
88888888888                    ad8888ba,           d8   ad88888ba                                  
88                            8P'    ""Y8         ,8P'  d8""     ""88                                 
88                           d8                 d8""    8P       88                                 
88aaaaa  88,dPYba,,adPYba,   88,dd888bb,      ,8P'     Y8,    ,d88                                 
88""""""""""  88P'   ""88""    ""8a  88P'    `8b     d8""        ""PPPPPP""88                                 
88       88      88      88  88       d8   ,8P'                 8P                                 
88       88      88      88  88a     a8P  d8""          8b,    a8P                                  
88       88      88      88   ""Y88888P""  8P'           `""Y8888P'                


  ,ad8888ba,                         ad8888ba,           d8   ad88888ba                            
 d8""'    `""8b                       8P'    ""Y8         ,8P'  d8""     ""88                           
d8'                                d8                 d8""    8P       88                           
88             88,dPYba,,adPYba,   88,dd888bb,      ,8P'     Y8,    ,d88                           
88      88888  88P'   ""88""    ""8a  88P'    `8b     d8""        ""PPPPPP""88                           
Y8,        88  88      88      88  88       d8   ,8P'                 8P                           
 Y8a.    .a88  88      88      88  88a     a8P  d8""          8b,    a8P                            
  `""Y88888P""   88      88      88   ""Y88888P""  8P'           `""Y8888P'                 


88888888888  88   88                                    88   ad88888ba                             
88           88   88                                    """"  d8""     ""88                            
88         aa88aaa88aa                                      8P       88                            
88aaaaa    """"88""""""88""""  88,dPYba,,adPYba,   ,adPPYYba,  88  Y8,    ,d88                            
88""""""""""    aa88aaa88aa  88P'   ""88""    ""8a  """"     `Y8  88   ""PPPPPP""88                            
88         """"88""""""88""""  88      88      88  ,adPPPPP88  88           8P                            
88           88   88    88      88      88  88,    ,88  88  8b,    a8P                             
88           88   88    88      88      88  `""8bbdP""Y8  88  `""Y8888P'                              


  ,ad8888ba,     88   88                               ,88  88  888888888888    d8  88888888ba,    
 d8""'    `""8b    88   88                             888P""  """"          ,8P'  ,8P'  88      `""8b   
d8'            aa88aaa88aa                                             d8""   d8""    88        `8b  
88             """"88""""""88""""  88,dPYba,,adPYba,   ,adPPYYba,  88       ,8P'  ,8P'     88         88  
88      88888  aa88aaa88aa  88P'   ""88""    ""8a  """"     `Y8  88      d8""   d8""       88         88  
Y8,        88  """"88""""""88""""  88      88      88  ,adPPPPP88  88    ,8P'  ,8P'        88         8P  
 Y8a.    .a88    88   88    88      88      88  88,    ,88  88   d8""   d8""          88      .a8P   
  `""Y88888P""     88   88    88      88      88  `""8bbdP""Y8  88  8P'   8P'           88888888Y""'    
                                                           ,88                                     
                                                         888P""                                     


88888888ba   88                                           88  888888888888    d8  88888888888      
88      ""8b  88                                           """"          ,8P'  ,8P'  88               
88      ,8P  88                                                      d8""   d8""    88               
88aaaaaa8P'  88,dPPYba,   88,dPYba,,adPYba,   ,adPPYYba,  88       ,8P'  ,8P'     88aaaaa          
88""""""""""""8b,  88P'    ""8a  88P'   ""88""    ""8a  """"     `Y8  88      d8""   d8""       88""""""""""          
88      `8b  88       d8  88      88      88  ,adPPPPP88  88    ,8P'  ,8P'        88               
88      a8P  88b,   ,a8""  88      88      88  88,    ,88  88   d8""   d8""          88               
88888888P""   8Y""Ybbd8""'   88      88      88  `""8bbdP""Y8  88  8P'   8P'           88               
                                                         ,88                                       
                                                       888P""                                       


  ,ad8888ba,                                   88  888888888888                                    
 d8""'    `""8b                                  """"          ,8P'                                    
d8'                                                       d8""                                      
88             88,dPYba,,adPYba,   ,adPPYYba,  88       ,8P'                                       
88      88888  88P'   ""88""    ""8a  """"     `Y8  88      d8""                                         
Y8,        88  88      88      88  ,adPPPPP88  88    ,8P'                                          
 Y8a.    .a88  88      88      88  88,    ,88  88   d8""                                            
  `""Y88888P""   88      88      88  `""8bbdP""Y8  88  8P'                                             


  ,ad8888ba,     88   88                      ,88           88  888888888888                       
 d8""'    `""8b    88   88                    888P""           """"          ,8P'                       
d8'            aa88aaa88aa                                             d8""                         
88             """"88""""""88""""  88,dPYba,,adPYba,   ,adPPYYba,  88       ,8P'                          
88      88888  aa88aaa88aa  88P'   ""88""    ""8a  """"     `Y8  88      d8""                            
Y8,        88  """"88""""""88""""  88      88      88  ,adPPPPP88  88    ,8P'                             
 Y8a.    .a88    88   88    88      88      88  88,    ,88  88   d8""                               
  `""Y88888P""     88   88    88      88      88  `""8bbdP""Y8  88  8P'                                
                                                           ,88                                     
                                                         888P""                                     

";


            chordText = chordText.Replace("\r", "");

            //0-92 1-82 2-72 3-62 4-50 5-38 6-28

            Dictionary<string, int> chordIndexToPosition = new Dictionary<string, int>()
            {
                {"-1", 102 }, // Check if 102 value is accurate
                {"0", 92 },
                {"1", 82 },
                {"2", 72 },
                {"3", 62 },
                {"4", 50 },
                {"5", 38 },
                {"6", 28 }
            };

            string[] section2ChordTimings =
@"0 -1
0.5 0
2 1
2.5 0
3 1
4.5 2
6 3
6.5 2
7 3
8.5 4
10 5
10.5 4
11 5
12.5 6 
14 2
14.5 6
15 2".Split('\r');

            // Parse the above to an interpolation graph
            InterpolationGraph chordTextMovementGraph = new InterpolationGraph("64;102;hold");
            InterpolationGraph chordTextIndexGraph = new InterpolationGraph(); // For use in section 5, for displaying pure text

            foreach (string section in section2ChordTimings)
            {
                string[] splitText = section.Trim().Split(' ');

                Console.WriteLine($"[{splitText[0]}] <{splitText[1]}>");
                Console.WriteLine();
                float time = float.Parse(splitText[0]);
                string value = splitText[1]; // returns string, get first char of it


                int chordTextPosition = chordIndexToPosition[value];
                chordTextMovementGraph.points.Add(new InterpolationPoint(time + 64, chordTextPosition, "hold", [15]));
                chordTextIndexGraph.points.Add(new InterpolationPoint(time, float.Parse(value) + 1, "hold", []));
            }

            chordTextIndexGraph.Initialize();
            Console.WriteLine(chordTextIndexGraph);
            chordTextMovementGraph = new InterpolationGraph(RepeatPoints(chordTextMovementGraph.ToString(), 2, 16));
            chordTextMovementGraph.Initialize();



            var chord1Text = new Generator(
                "Chords",
                new TextDisplay($"-w {chordText} -wI 0;0 -d £"), // Random symbol to avoid splitting
                isActiveInterpolation: "0;0 64;1",
                effects: new List<Effect>()
                {
                    new Transform($"-xPI {chordTextMovementGraph} -yPI 0;60 -rI 0;0 -sI 0;1")
                }
            );

            var horizontalChordBarGeneratorContainer = new Generator(
                "Horiz. Chord Bar Gen. Container",
                new Blank("-s 120, 120"),
                isActiveInterpolation: "0;0 64;1",
                blendingMode: Generator.BlendingMode.Without
            );

            string horizontalChordBar =
                string.Join("\n",
                    RepeatNTimesToArray(
                        string.Join("", RepeatNTimesToArray("@", 120))
                    , 10)
                );

            // Add the blending mode on a parent object since transforming only changes the draw point.
            // The blending mode functions execute before the drawing position is handled.

            var horizontalChordBarGenerator = new Generator(
                "Horizontal Chord Bar",
                new TextDisplay($"-w {horizontalChordBar} -wI 0;0 -d ."),
                effects: new List<Effect>()
                {
                    new Transform("-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;1")
                }
            );

            var chordDisplayContainer = new Generator(
                "ChordDisplayContainer",
                new Blank("-s 120,120"),
                isActiveInterpolation: "0;0 64;1 96;0",
                effects: new List<Effect>()
                {
                    new Opacity("-oI 0;1 64;1 78;1;linear 80;0.05")
                }
            );

            // Extra background effect

            Generator section4FizzyBackground = new Generator(
                "Section 4 Fizzy Background",
                new ShaderTest("-s 120,120"),
                isActiveInterpolation: "0;0 64;1 80;0",
                blendingMode: Generator.BlendingMode.Behind,
                effects: new List<Effect>()
                {
                    new Opacity("-oI 64;0.001 78;0.001;linear 80;0")
                }
            );


            horizontalChordBarGeneratorContainer.AddSubGenerators(horizontalChordBarGenerator);
            chordDisplayContainer.AddSubGenerators(chord1Text, horizontalChordBarGeneratorContainer, section4FizzyBackground);
            d.AddGenerators(chordDisplayContainer);

            #endregion


            #region BASS NOTES

            string[] section4BassNotes =
@"0 .
0.5 F3
1 F3
1.5 C4
1.75 F4
2 G3
2.5 F3
3 G3
3.5 G3
4 .
4.5 F#3
5 F#3
5.5 C#4
5.75 F#4
6 G#3
6.5 A#3
7 G#3
7.5 G#3
7 .
8.5 F3
9 F3
9.5 C4
9.75 F4
10 G3
10.5 F3
11 G3
11.5 G3
11 .
12.5 G#3
13 G#3
13.5 D#4
13.75 G#4
14 A#3
14.5 G#3
15 A#3
15.5 A#3".Split('\n');


            /*
            
F4
C4
G#3
F3
G3
C#4
G#4
A#3
F#3
F#4
D#4

            */

            Dictionary<string, string> notesToBigText = new Dictionary<string, string>()
            {
                { ".", ""},
                { "F4", "███████ ██   ██ \r\n██      ██   ██ \r\n█████   ███████ \r\n██           ██ \r\n██           ██ \r\n                \r\n                "},
                { "C4", " ██████ ██   ██ \r\n██      ██   ██ \r\n██      ███████ \r\n██           ██ \r\n ██████      ██ \r\n                \r\n                "},
                { "G#3", " ██████   ██  ██  ██████  \r\n██       ████████      ██ \r\n██   ███  ██  ██   █████  \r\n██    ██ ████████      ██ \r\n ██████   ██  ██  ██████  \r\n                          \r\n                          "},
                { "F3", "███████ ██████  \r\n██           ██ \r\n█████    █████  \r\n██           ██ \r\n██      ██████  \r\n                \r\n                "},
                { "G3", " ██████  ██████  \r\n██            ██ \r\n██   ███  █████  \r\n██    ██      ██ \r\n ██████  ██████  \r\n                 \r\n                 " },
                { "C#4", " ██████  ██  ██  ██   ██ \r\n██      ████████ ██   ██ \r\n██       ██  ██  ███████ \r\n██      ████████      ██ \r\n ██████  ██  ██       ██ \r\n                         \r\n                         "},
                { "G#4", " ██████   ██  ██  ██   ██ \r\n██       ████████ ██   ██ \r\n██   ███  ██  ██  ███████ \r\n██    ██ ████████      ██ \r\n ██████   ██  ██       ██ \r\n                          \r\n                          "},
                { "A#3", " █████   ██  ██  ██████  \r\n██   ██ ████████      ██ \r\n███████  ██  ██   █████  \r\n██   ██ ████████      ██ \r\n██   ██  ██  ██  ██████  \r\n                         \r\n                         "},
                { "F#3", "███████  ██  ██  ██████  \r\n██      ████████      ██ \r\n█████    ██  ██   █████  \r\n██      ████████      ██ \r\n██       ██  ██  ██████  \r\n                         \r\n                         "},
                { "F#4", "███████  ██  ██  ██   ██ \r\n██      ████████ ██   ██ \r\n█████    ██  ██  ███████ \r\n██      ████████      ██ \r\n██       ██  ██       ██ \r\n                         \r\n                         "},
                { "D#4", "█████   ██  ██  ██   ██ \r\n██   ██ ████████ ██   ██ \r\n██   ██  ██  ██  ███████ \r\n██   ██ ████████      ██ \r\n██████   ██  ██       ██ \r\n                         \r\n                         "},
            };

            List<string> section4AllBassNotes = [".", "F4", "C4", "G#3", "F3", "G3", "C#4", "G#4", "A#3", "F#3", "F#4", "D#4"];

            // Parse the above and iterate through each line, creating an interpolation graph based on it
            InterpolationGraph section4BassNoteBrightnessGraph = new InterpolationGraph();
            InterpolationGraph section4BassNoteScaleGraph = new InterpolationGraph();
            InterpolationGraph section4BassNoteChordIndexGraph = new InterpolationGraph();


            for (int i = 0; i < section4BassNotes.Length; i++)
            {
                string[] splitItem = section4BassNotes[i].Split(' ');
                float time = float.Parse(splitItem[0]);
                string note = splitItem[1].Trim(); // splitItem[1] returns a char as a string, get the first char of that

                float nextTime;
                if (i == section4BassNotes.Length - 1)
                {
                    nextTime = time + 0.5f;
                }
                else
                {
                    nextTime = float.Parse(section4BassNotes[i + 1].Split(' ')[0]);
                }

                Console.WriteLine(note);
                int chordIndex = section4AllBassNotes.FindIndex(n => n == note);

                section4BassNoteScaleGraph.points.Add(new InterpolationPoint(time + 80, 4.5, "easeIn", [5]));
                section4BassNoteScaleGraph.points.Add(new InterpolationPoint(nextTime + 80, 1, "hold", []));

                section4BassNoteBrightnessGraph.points.Add(new InterpolationPoint(time + 80, 0.5, "easeOut", [10]));
                section4BassNoteBrightnessGraph.points.Add(new InterpolationPoint((time + nextTime)/2 + 80, 1, "hold", []));

                section4BassNoteChordIndexGraph.points.Add(new InterpolationPoint(time + 80, chordIndex, "hold", []));

            }

            List<string> allLargeChordText = new List<string>();
            foreach (string note in section4AllBassNotes)
            {
                allLargeChordText.Add(notesToBigText[note].Replace("\r", ""));
            }

            section4BassNoteBrightnessGraph.Initialize();
            section4BassNoteChordIndexGraph.Initialize();



            char delimiter = '_';
            Generator section4FlashingBassNotes = new Generator(
                "BassNotes",
                new TextDisplay($"-w {string.Join(delimiter.ToString(), allLargeChordText)} -wI {section4BassNoteChordIndexGraph} -d {delimiter}"),
                isActiveInterpolation: "0;0 80;1 96;0",
                effects: new List<Effect>()
                {
                    new Transform($"-xPI 0;60 -yPI 0;60 -rI 0;0 -sI {section4BassNoteScaleGraph}"),
                    new Opacity($"-oI {section4BassNoteBrightnessGraph}")
                }
            );


            d.AddGenerator(section4FlashingBassNotes);

            #endregion


            #endregion

            #region SECTION 5


            Generator everythingSandwichContainer = new Generator(
                "Everything Sandwich Container",
                new SolidColor("-s 120,18 -bI 0;0.0"),
                isActiveInterpolation: "0;0 96;1 128;0",
                effects: new List<Effect>()
                {
                    new Repeat("-xRI 0;1 -yRI 0;10"),
                    new Offset("-xOI 96;0;sin;[8] 128;128 -yOI 96;0;cos;[8] 128;128"),
                    new Transform("-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;1"),
                    new Opacity("-oI 0;1 124;1;linear 128;0")
                }
            );


            #region BOUDAN
            InterpolationGraph section5BoudanGraph = new InterpolationGraph("0>96;0>0;hold");

            // Create an interpolation graph based on the original boudan text, with an offset
            for (int i = 0; i < wholeBoudan.points.Count; i++)
            {
                var originalPoint = wholeBoudan.points[i];
                var IP = new InterpolationPoint(originalPoint.startTime + 96, originalPoint.endTime + 96, originalPoint.startValue, originalPoint.endValue, "hold", []);
                section5BoudanGraph.points.Add(IP);

            }

            Generator section5Boudan = new Generator(
                "Section 5 Boudan",
                new TextDisplay($"-w {string.Join(",", wholeBoudanText)} -wI {section5BoudanGraph.ExportToString()} -d ,"),
                isActiveInterpolation: "0;0 96;1",
                effects: new List<Effect>()
                {
                    new Repeat("-xRI 0;10 -yRI 0;1"),
                    new Offset("-xOI 96;0;linear 128;128 -yOI 0;0"),
                    new Transform($"-xPI 0;5 -yPI 0;60 -rI 0;0 -sI 0;1"),
                }
            );

            everythingSandwichContainer.AddSubGenerators(section5Boudan);

            #endregion

            #region REGULAR BASS NOTES

            InterpolationGraph section5BassNoteChordIndexGraph = new InterpolationGraph("0>96;0>0;hold");

            // Move this shifting function to InterpolationGraph
            for (int i = 0; i < section4BassNoteChordIndexGraph.points.Count; i++)
            {
                var originalPoint = section4BassNoteChordIndexGraph.points[i];
                var IP = new InterpolationPoint(originalPoint.startTime + 16, originalPoint.endTime + 16, originalPoint.startValue, originalPoint.endValue, "hold", []);
                section5BassNoteChordIndexGraph.points.Add(IP);

            }

            section5BassNoteChordIndexGraph = new InterpolationGraph(RepeatPoints(section5BassNoteChordIndexGraph.ToString(), 2, 16));
            Console.WriteLine(section5BassNoteChordIndexGraph);
            //section5BassNoteChordIndexGraph.Initialize();

            char delimiter2 = '_';
            Generator section5RegularBassNotes = new Generator(
                "RegularBassNotes",
                new TextDisplay($"-w {string.Join(delimiter2.ToString(), allLargeChordText)} -wI {section5BassNoteChordIndexGraph} -d {delimiter2}"),
                isActiveInterpolation: "0;0 96;1",
                effects: new List<Effect>()
                {
                    new Repeat("-xRI 0;10 -yRI 0;1"),
                    new Offset("-xOI 96;0;linear 128;-384 -yOI 0;0"),
                    new Transform($"-xPI 0;13 -yPI 0;60 -rI 0;0 -sI 0;1"),
                }
            );

            everythingSandwichContainer.AddSubGenerators(section5RegularBassNotes);

            #endregion

            #region CHORDS

            InterpolationGraph section5ChordIndexGraph = new InterpolationGraph();
            for (int i = 0; i < chordTextIndexGraph.points.Count; i++)
            {
                var originalPoint = chordTextIndexGraph.points[i];
                var IP = new InterpolationPoint(originalPoint.startTime + 96, originalPoint.startValue, "hold", []);
                section5ChordIndexGraph.points.Add(IP);

            }

            section5ChordIndexGraph.Initialize();

            section5ChordIndexGraph = new InterpolationGraph(RepeatPoints(section5ChordIndexGraph.ToString(), 2, 16));

            Console.WriteLine("HERE");
            Console.WriteLine(section5ChordIndexGraph);

            string allChords = " _Fm6/9  _Gm6/9  _F#maj9  _G#maj7/D  _Bbmaj7/F  _Gmaj7  _G#maj7  ";
            Generator chordTextDisplay = new Generator(
                "Section 5 Chord text display",
                new TextDisplay($"-w {allChords} -wI {section5ChordIndexGraph} -d _"),
                effects: new List<Effect>()
                {
                    new Repeat("-xRI 0;30 -yRI 0;1"),
                    new Offset("-xOI 96;0;linear 128;256 -yOI 0;0"),
                    new Transform($"-xPI 0;17 -yPI 0;60 -rI 0;0 -sI 0;1"),
                }
            );

            everythingSandwichContainer.AddSubGenerators(chordTextDisplay);

            d.AddGenerators(everythingSandwichContainer);

            #endregion

            #region DANCE

            Generator danceBorder = new Generator(
                "Dance Border",
                new Outline("-xSI 94;3;easeOut;[4] 95;25 96;25;bounce;[32] 128;30 -ySI 94;5 95;3;easeOut;[4] 96;25;bounce;[32] 128;30 -c #"),
                isActiveInterpolation: "0;0 94;1",
                effects: new List<Effect>()
                {
                    new Transform($"-xPI 0;60 -yPI 0;60 -rI 0;0 -sI 0;1"),
                    new ForceAlpha("-c ?")
                }
            );

            d.AddGenerators(danceBorder);

            #endregion

            #endregion

            Console.ReadLine();

            return d;

        }

    }
}
