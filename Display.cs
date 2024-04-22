using ASCIIMusicVisualiser8;
using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using ASCIIMusicVisualiser8.Types.Interpolation;
using NAudio.CoreAudioApi;
using NAudio.SoundFont;
using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ASCIIMusicVisualiser8.Utility;

namespace ASCIIMusicVisualiser8
{
    internal class Display
    {
        public Conductor Conductor { get; private set; }

        // Audio data
        public double BPM;
        public string audioFilepath;

        // Visual data
        public int updateTimeMilliseconds = 1;
        Vector2 dimensions;

        bool isActivated = true;



        public Display(double BPM, string audioFilepath, Vector2 dimensions)
        {
            this.BPM = BPM;
            this.audioFilepath = audioFilepath;
            this.dimensions = dimensions;
        }

        // All plugins active
        List<Generator> activeGenerators = new()
        {

        };

        // Adds a generator to the display's list based on the generator's layer number.
        public void AddGenerator(Generator g)
        {
            for (int i = 0; i < activeGenerators.Count; i++)
            {
                if (activeGenerators[i].layer > g.layer)
                {
                    activeGenerators.Insert(i, g);
                    break;
                }
            }
            activeGenerators.Add(g);
        }

        // Adds multiple generators.
        public void AddGenerators(List<Generator> gList)
        {
            foreach (Generator g in gList)
            {
                AddGenerator(g);
            }
        }

        public void PrintGenerators()
        {
            PrintList(activeGenerators);
        }

        public void Run()
        {
            

            bool generatorsExist = false;

            if (activeGenerators.Count > 0)
            {
                generatorsExist = true;
            }
            Console.WriteLine(generatorsExist);



            // REMOVE THIS CODE AFTERWARDS

            InterpolationGraph graph = new InterpolationGraph();
            List<double> times = new()
            {

                0,
                0.75,
                1.5,
                2.5,
                2.75



            };

            List<double> values = new()
            {
                
               1,

            };

            List<string> easing = new()
            {
                "hold",
                "hold",
                "hold",
                "hold",
                "hold",
                "hold",
                "easeIn",
                "hold",
                "hold",
                "hold",
                "hold",
                "hold",
                "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold",
                "hold", "hold"
            };

            List<double[]> parameters = new()
            {
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {3},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0},
                new double[] {0}
            };
            graph.SetPoints(
                Utility.RepeatPoints(
                  Utility.PointsFromLists(
                      times, values, easing, RepeatNTimesToList(new double[] { 3 }, easing.Count)
                    ),
                  32, 4
            ));

            // ---------------------------------------------------

            graph.Print();
            Console.WriteLine(graph.ExportToString());
            Console.ReadLine();

            Thread.Sleep(2000);

            // Set up audio
            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);             
            Conductor = new Conductor(BPM);





            while (isActivated)
            {
                double amount = graph.GetTime(Conductor.beatsPrecise);
                Console.WriteLine(RepeatChar('#', (int)(40 * amount)) + RepeatChar(' ', 40));
                //Console.WriteLine();
                //Console.WriteLine($"Value: {graph.GetTime(Conductor.beatsPrecise)}\r");

                // Update conductor with current milliseconds
                Conductor.SetCurrentTime((long)waveOutEvent.GetPositionTimeSpan().TotalMilliseconds);

                if (generatorsExist)
                {
                    var currentFrameDisplayBoard = Create2DArray(' ', dimensions);
                
                    foreach (var generator in activeGenerators)
                    {
                        DrawLayer(currentFrameDisplayBoard, generator);
                    }
                    string charlistToString = StringifyCharlist(currentFrameDisplayBoard);

                    Console.WriteLine(charlistToString);
                    GoToTopLeft();
                }
                //if (updateTimeMilliseconds != 0) Thread.Sleep(updateTimeMilliseconds);
            }
        }

        public void Stop()
        {
            isActivated = false;
        }

        WaveOutEvent PlayAudio(string filename)
        {
            var reader = new AudioFileReader(filename);
            var waveOutEvent = new WaveOutEvent();
            
            waveOutEvent.Init(reader);
            waveOutEvent.Play();
            return waveOutEvent;
        }

        void DrawLayer(List<List<char>> _displayBoard, Vector2 topLeftPosition, List<List<char>> textToDraw, char? transparentChar = null)
        {
            
            // Iterate through each char in textToDraw and change it's corresponding 
            for (int i = 0; i < textToDraw.Count; i++)
            {
                for (int j = 0; j < textToDraw[0].Count; j++)
                {
                    try
                    {
                        // If the current character is not transparent, draw it onto the displayBoard
                        char currentChar = textToDraw[i][j];
                        Vector2 positionToDraw = new(i + (int)topLeftPosition.X, j + (int)topLeftPosition.Y);
                        if (currentChar != transparentChar) _displayBoard[(int)positionToDraw.X][(int)positionToDraw.Y] = currentChar;
                        
                    }
                    catch
                    {
                        // Ignore out of bounds characters
                    }
                    
                }
            }
        }

        public void DrawLayer(List<List<char>> displayBoard, Generator generator)
        {
            GeneratorOutput gOutput = generator.GetOutput(Conductor.beatsPrecise);

            /*
            // Drawing a layer with effects
            if (generator.effects.Count != 0)
            {
                Vector2 displayBoardDimensions = new Vector2(displayBoard[0].Count, displayBoard.Count);
                List<List<char>> effectDisplayBoard = Utility.Create2DArray(' ', displayBoardDimensions);
                DrawLayer(effectDisplayBoard, gOutput.position, gOutput.text, gOutput.transparentChar);
                
                foreach (Effect effect in generator.effects)
                {
                    effectDisplayBoard = effect.ApplyTo(effectDisplayBoard, gOutput.transparentChar);
                }

                DrawLayer(displayBoard, Vector2.Zero, effectDisplayBoard, gOutput.transparentChar);
            }
            else
            {
            }
            */
            DrawLayer(displayBoard, gOutput.position, gOutput.text, gOutput.transparentChar);

        }
    }
}
