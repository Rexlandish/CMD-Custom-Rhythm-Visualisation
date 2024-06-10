using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using NAudio.Wave;
using NAudio.Utils;
using System;
using System.Threading;
using static ASCIIMusicVisualiser8.Utility.Repeat;

namespace ASCIIMusicVisualiser8
{
    internal class Program
    {


        static void Main(string[] args)
        {

            
            Display display = CreateDisplay();

            Run(display);
            

        }


        static float bpm = 128;
        static int updateTimeMilliseconds = 0;
        static string audioFilepath = @"audio/Tidy.mp3";
        static Conductor Conductor;
        public static void Run(Display display)
        {

            //Thread.Sleep(2000);

            // Set up audio
            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);
            Conductor = new Conductor(bpm);


            while (true)
            {
                //Console.WriteLine();
                //Console.WriteLine($"Value: {graph.GetTime(Conductor.beatsPrecise)}\r");

                // Update conductor with current milliseconds
                Conductor.SetCurrentTime((long)waveOutEvent.GetPositionTimeSpan().TotalMilliseconds);

                string res = display.GetFrameOnBeat(Conductor.beatsPrecise);

                Console.WriteLine(res);
                Utility.ConsoleOp.GoToTopLeft();

                Thread.Sleep(updateTimeMilliseconds);
            }
        }


        static WaveOutEvent PlayAudio(string filename)
        {
            var reader = new AudioFileReader(filename);
            var waveOutEvent = new WaveOutEvent();

            waveOutEvent.Init(reader);
            waveOutEvent.Play();
            return waveOutEvent;
        }


        public static Display CreateDisplay()
        {
            //Display display = new Display(128, @"C:\Users\glass\Downloads\GlassChaek - School Issues\GlassChaek - School Issues - 02 Cover Teachers.mp3", new(64,64));
            
            Display display = new Display(new(64, 64));

            Generator bg = new Generator("BG", new SolidColor());
            bg.plugin.@class.ProcessParameterStringPlugin("--size 64,64");// -bI 0;0.5;cos;[128] 128;1");


            Generator checker1 = new Generator("Checker", new Checkerboard(), blendingMode: Generator.BlendingMode.Subtract);
            checker1.plugin.@class.ProcessParameterStringPlugin("--size 64,64 -xS 2 -yS 4");

            Generator checker2 = new Generator("Checker", new Checkerboard(), blendingMode: Generator.BlendingMode.Subtract);
            checker2.plugin.@class.ProcessParameterStringPlugin("--size 64,64 -xS 3 -yS -3");

            Generator fade = new Generator("Fade", new SolidColor(), blendingMode: Generator.BlendingMode.Multiply);
            bg.plugin.@class.ProcessParameterStringPlugin("--size 64,64 -bI 0;0.25");

            bg.AddSubGenerators(checker1, checker2);



            Generator gen = new Generator("Bounce", new Debug());

            Effect transform = new Transform();
            string rotation = RepeatPoints("0;0;easeOut;[4] 2;180 2;180;easeOut;[4] 4;0", 128, 4);
            string scale = RepeatPoints("0;1.5;easeOut;[2] 1;1 1;1.5;easeOut;[2] 2;1", 128, 2); ;
            transform.SetParameterString($"-xPI 0;32 -yPI 0;32 -rI {rotation} -sI {scale}");

            gen.AddEffect(transform);

            Generator tubes = new Generator("tubes", new SwirlingTubes(), blendingMode: Generator.BlendingMode.Multiply);
            tubes.plugin.@class.ProcessParameterStringPlugin("--size 64,64");

            Generator squareBG = new Generator("Square BG", new SolidColor(), blendingMode: Generator.BlendingMode.Subtract);
            squareBG.plugin.@class.ProcessParameterStringPlugin("-s 64,64 -bI 0;0.2;cos;[256] 256;0");


            gen.AddSubGenerators(tubes, squareBG);


            // ---------------------------------


            Generator blank = new Generator("Blank", new Blank());
            blank.plugin.@class.ProcessParameterStringPlugin("--size 64,64");

            Generator fadeScreen = new Generator("Fading", new SolidColor(), blendingMode: Generator.BlendingMode.Subtract);
            string activeInterpolation = RepeatPoints($"0;0 4;0 4.5;1 5;0 5.5;1 6;0 6.5;1 7;0 7.5;1", 128, 8);
            fadeScreen.plugin.@class.ProcessParameterStringPlugin($"--size 64,64 -bI {activeInterpolation}");


            blank.AddSubGenerators(bg, gen);

            blank.AddSubGenerators(fadeScreen);

            // ---------------------------------

            string catActiveInterpolation = RepeatPoints($"0;0 4;0 4.5;1 5;0 5.5;1 6;0 6.5;1 7;0 7.5;1", 128, 8);
            Generator catDance = new Generator("Cat Dance", new TextDisplay(), catActiveInterpolation);
            string catRepeatInterpolation = RepeatPoints($"0;0;linear 2;30", 128, 2);
            catDance.plugin.@class.ProcessParameterStringPlugin($"-w {Dance.catDance} -wI {catRepeatInterpolation} -d ,");

            Effect catTransform = new Transform();
            catTransform.SetParameterString("-xPI 0;20 -yPI 0;45 -rI 0;0 -sI 0;0.75");

            catDance.AddEffect(catTransform);

            display.AddGenerators(blank, catDance);

            return display;

        }

    }

}


 
