using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using static ASCIIMusicVisualiser8.Utility.Repeat;

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
            Generator dotsies = new Generator("Dotsies", new TextDisplay(), RepeatPoints($"0;1 0.5;0 1;1 1.5;0 2;1 2.5;0 3;1 3.5;0", 128, 8), Generator.BlendingMode.Without);

            string wordsInterpolation = RepeatPoints("0.5;0;linear 31.5;31", 32, 32);
            
            List<string> dotsiesTextList = new();
            foreach (string s in "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua enim ad minim veniam quis nostrud exercitation ullamco laboris nisi aliquip commodo consequat".Split(' '))
            {
                dotsiesTextList.Add(Dotsies.GetDotsies(s, 'x', ' '));
            }
            string dotsiesText = string.Join(",",dotsiesTextList);

            dotsies.plugin.@class.ProcessParameterStringPlugin($"-w {dotsiesText} -d , -wI {wordsInterpolation}");

            /*
            Effect dotsiesTransform = new Transform($"-xPI 0;{sizeX / 2} -yPI 0;{sizeY / 2} -rI 0.5;30;cos;[64] 64.5;-30 -sI 0;8;bounce;[64] 64;14");
            */
            Effect dotsiesTransform = new Transform($"-xPI 0;{sizeX / 2} -yPI 0;{sizeY / 2} -rI 0;0 -sI 0;8");
            Effect dotsiesForcedAlpha = new ForceAlpha($"-c x");

            dotsies.AddEffects(dotsiesForcedAlpha, dotsiesTransform);
            


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
            Display display = new Display(new Vector2(5, 5));

            var background = new Generator(
                "background",
                new SolidColor("--size 5,5 -bI 0;0.1")
            );

            display.AddGenerators(background);

            return display;

        }

    }
}
