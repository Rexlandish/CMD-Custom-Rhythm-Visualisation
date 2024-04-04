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



        public Display(double _BPM, string _audioFilepath, Vector2 _dimensions)
        {
            BPM = _BPM;
            audioFilepath = _audioFilepath;
            dimensions = _dimensions;
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
            graph.SetPoints(
                new List<InterpolationPoint>
                {
                    new InterpolationPoint(0, 0, "easeoutelastic"),
                    new InterpolationPoint(1, 1, "easeoutelastic"),
                    new InterpolationPoint(2, 2, "easeoutelastic"),
                    new InterpolationPoint(3, 3, "easeoutelastic"),
                    new InterpolationPoint(4, 4, "easeoutelastic"),
                    new InterpolationPoint(5, 3, "easeoutelastic"),
                    new InterpolationPoint(6, 2, "easeoutelastic"),
                    new InterpolationPoint(7, 1, "easeoutelastic"),
                });

            // ---------------------------------------------------

            graph.Print();

            Thread.Sleep(2000);

            // Set up audio
            WaveOutEvent waveOutEvent = PlayAudio(audioFilepath);             
            Conductor = new Conductor(BPM);





            while (isActivated)
            {
                int amount = 20 + (int)Math.Round(30 * graph.GetTime(Conductor.beatsPrecise));
                Console.WriteLine(RepeatChar('#', amount));
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
                    Thread.Sleep(updateTimeMilliseconds);
                }

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
