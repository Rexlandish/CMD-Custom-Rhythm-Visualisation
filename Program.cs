﻿using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using NAudio.Wave;
using NAudio.Utils;
using System;
using System.Threading;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using ASCIIMusicVisualiser8.Plugins;
using System.Runtime.InteropServices;
using NAudio.Dmo;
using ASCIIMusicVisualiser8.Types.Interpolation;
using System.Diagnostics;
using System.Collections.Generic;

namespace ASCIIMusicVisualiser8
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Utility.Conversion.InitializeCharShadeStringDict();
            /*

            Generator tubes = new("tubes", new SwirlingTubes("-s 50,50"));
            //IPlugin tubes = new SwirlingTubes("-s 50,50");

            //Console.WriteLine(tubes.Generate(0, out _)[0][0]);
            Console.WriteLine(Utility.Conversion.StringifyOutputPixel2DArray(tubes.GetOutput(0).outputPixels));

            return;
            */

            //FontParser.GetData();


            //Display.GetAllPlugins();


            /*
            string res = Dotsies.GetDotsies("kaki kaki gavvala kaki", ' ', '█');
            Console.WriteLine(res);
            */

            //Console.WriteLine(Display.CombineChars(' ', '@', Generator.BlendingMode.Without));
            //Console.WriteLine(Display.CombineChars(' ', ' ', Generator.BlendingMode.Multiply));

            /*
            Generator dotsies = new Generator("Dotsies", new TextDisplay(), blendingMode: Generator.BlendingMode.Subtract);

            List<string> dotsiesTextList = new();
            foreach (string s in "bicycletta".Split(' '))
            {
                dotsiesTextList.Add(Dotsies.GetDotsies(s, '@', ' '));
            }
            string dotsiesText = string.Join(",", dotsiesTextList);

            dotsies.plugin.@class.ProcessParameterStringPlugin($"-w {dotsiesText} -d , -wI 0;0");

            Generator solid = new Generator("Solid", new SolidColor("-bI 0;0.5 -s 10,10"));

            Display display = new Display(new(20, 20));

            display.AddGenerators(solid, dotsies);

            Console.WriteLine(display.GetFrameOnBeat(0));
            */


            //Utility.Conversion.InitializeCharShadeStringDict();
            //Display display = ExampleDisplays.CreateMaskingDisplay();


            /*
            Display display = new Display(new(16, 16));
            //display.HandleNext("", false, 0);

            var g = new Generator("test", new SolidColor("-bI 0;0.5 -s 15,15"));
            g.AddEffect(new Transform("-xPI 0;8 -yPI 0;8 -rI 0;45 -sI 0;1"));

            var tubes = new Generator("test2", new SwirlingTubes("-s 20,20"));

            g.AddSubGenerators(tubes);

            display.AddGenerator(g);

            //Console.WriteLine(Utility.Conversion.StringifyCharlist(g.GetOutput(0).text));
            */

            //Run(display);

            Display d = ExampleDisplays.CreateBouDanDisplay();
            Run(d);

        }


        static float bpm = 115;
        static int updateTimeMilliseconds = 1;
        static float millisecondDelay = 0;
        static string audioFilepath = @".\audio\Bou Dan.wav";
        static Conductor Conductor;
        public static void Run(Display display)
        {

            //Thread.Sleep(2000);

            // Set up audio

            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);
            Conductor = new Conductor(bpm);
            Conductor.SetCurrentTime(0);
            
            var watch = new System.Diagnostics.Stopwatch();
            while (true)
                
            {
                watch.Restart();
                //Console.WriteLine(string.Join("\n", RepeatNTimesToArray(RepeatChar(' ', 50), 50)));
                Utility.ConsoleOp.GoToTopLeft();
                //Console.WriteLine();
                //Console.WriteLine($"Value: {graph.GetTime(Conductor.beatsPrecise)}\r");

                // Update conductor with current milliseconds
                Conductor.SetCurrentTime((long)waveOutEvent.GetPositionTimeSpan().TotalMilliseconds - (long)millisecondDelay);
                
                var generationTimer = Stopwatch.StartNew();
                double currentBeat = Conductor.beatsPrecise;
                string res = display.GetFrameOnBeat(currentBeat);
                Console.WriteLine(res);
                //display.PrettyPrint(Conductor.beatsPrecise);
                Console.WriteLine("Beat " + currentBeat + "                ");
                Console.WriteLine("FPS: " + generationTimer.ElapsedMilliseconds + "                ");
                generationTimer.Stop();

                watch.Stop();

                //Console.WriteLine($"Print time: {printSw.ElapsedMilliseconds}ms              ");
                //Console.WriteLine($"Total time: {watch.ElapsedMilliseconds}ms              ");
                
                
                Utility.ConsoleOp.GoToTopLeft();
                Thread.Sleep(updateTimeMilliseconds);
            }


            /*
            while (true)
            {
                watch.Restart();
                //Console.WriteLine(string.Join("\n", RepeatNTimesToArray(RepeatChar(' ', 50), 50)));
                Utility.ConsoleOp.GoToTopLeft();
                //Console.WriteLine();
                //Console.WriteLine($"Value: {graph.GetTime(Conductor.beatsPrecise)}\r");

                // Update conductor with current milliseconds
                Conductor.SetCurrentTime((long)waveOutEvent.GetPositionTimeSpan().TotalMilliseconds);

                display.PrettyPrint(Conductor.beatsPrecise);

                watch.Stop();

                Utility.ConsoleOp.GoToTopLeft();
                Thread.Sleep(updateTimeMilliseconds);
            }
            */




        }

        public static void RunProcedural(Display display)
        {

            //Thread.Sleep(2000);

            // Set up audio


            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);
            Conductor = new Conductor(bpm);

            var watch = new System.Diagnostics.Stopwatch();

            while (true)
            {
                watch.Restart();
                for (int i = 0;  i < 10; i++)
                {
                    Console.WriteLine("                                                                               ");
                }

                //Console.WriteLine($"Value: {graph.GetTime(Conductor.beatsPrecise)}\r");

                // Update conductor with current milliseconds
                Conductor.SetCurrentTime((long)waveOutEvent.GetPositionTimeSpan().TotalMilliseconds);

                string res = display.GetFrameOnBeat(Conductor.beatsPrecise);

                Console.WriteLine(res);
                watch.Stop();
                Console.WriteLine($"{watch.ElapsedMilliseconds}ms              ");

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


    }

}


 
