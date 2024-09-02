using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Media3D;
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
    }
}
