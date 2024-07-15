using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Plugins;
using NAudio.Wave;
using NAudio.Utils;
using System;
using System.Threading;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using ASCIIMusicVisualiser8.Plugins;
using System.Runtime.InteropServices;
using NAudio.Dmo;

namespace ASCIIMusicVisualiser8
{
    internal class Program
    {


        static void Main(string[] args)
        {
            /*
            string res = Dotsies.GetDotsies("kaki kaki gavvala kaki", ' ', '█');
            Console.WriteLine(res);
            */

            
            Display display = ExampleDisplays.CreateDisplay();

            //display.HandleNext("", false, 0);

            Run(display);
            
            

        }


        static float bpm = 128;
        static int updateTimeMilliseconds = 10;
        static string audioFilepath = @"./audio/Tidy.mp3";
        static Conductor Conductor;
        public static void Run(Display display)
        {

            //Thread.Sleep(2000);

            // Set up audio

            Console.WriteLine("Press enter to go plds!");
            Console.ReadLine();

            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);
            Conductor = new Conductor(bpm);

            var watch = new System.Diagnostics.Stopwatch();
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
                
                string res = display.GetFrameOnBeat(Conductor.beatsPrecise);

                Console.WriteLine(res);
                watch.Stop();
                Console.WriteLine($"{watch.ElapsedMilliseconds}ms              ");
                
                Utility.ConsoleOp.GoToTopLeft();
                //Thread.Sleep(updateTimeMilliseconds);
            }
            */


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




        }

        public static void RunProcedural(Display display)
        {

            //Thread.Sleep(2000);

            // Set up audio

            Console.WriteLine("Press enter to go plds!");
            Console.ReadLine();

            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);
            Conductor = new Conductor(bpm);

            var watch = new System.Diagnostics.Stopwatch();

            while (true)
            {
                watch.Restart();
                //Console.WriteLine();
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


 
