
using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using static ASCIIMusicVisualiser8.Generator;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Visualisation;

namespace ASCIIMusicVisualiser8
{
    public class Display : IHierarchy
    {
        public Conductor Conductor { get; private set; }
        public string name { get => "Display"; }

        // Visual data
        public int updateTimeMilliseconds = 2;
        Vector2 dimensions;

        bool isActivated = true;

        public float lastExecutionTime;

        public Display(Vector2 dimensions)
        { 
            this.dimensions = dimensions;
        }

        // All plugins active
        public List<Generator> activeGenerators = new()
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
        public void AddGenerators(params Generator[] gList)
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

        public string GetFrameOnBeat(double beat)
        {
            OutputPixel op = new(0);
            var currentFrameDisplayBoard = Create2DArray(op, dimensions);

            Stopwatch sw = new Stopwatch();
            lastExecutionTime = 0;

            foreach (var generator in activeGenerators)
            {
                sw.Start();
                DrawLayer(currentFrameDisplayBoard, generator, beat);
                sw.Stop();
                lastExecutionTime += sw.ElapsedMilliseconds;
                sw.Reset();
            }

            string charlistToString = StringifyOutputPixel2DArray(currentFrameDisplayBoard);
            
            return charlistToString;
        }


        public void Stop()
        {
            isActivated = false;
        }

        public static void DrawLayer(List<List<OutputPixel>> displayBoard, Generator generator, double currentBeat)
        {
            

            GeneratorOutput gOutput = generator.GetOutput(currentBeat);
            

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
            DrawLayer(displayBoard, gOutput.position, gOutput.outputPixels, gOutput.blendingMode, gOutput.transparentChar);

        }

        public static void DrawLayer(List<List<OutputPixel>> _displayBoard, Vector2 topLeftPosition, List<List<OutputPixel>> textToDraw, BlendingMode blendingMode, OutputPixel? transparentPixel = null)
        {

            int height = _displayBoard[0].Count;
            int width = _displayBoard.Count;


            // Iterate through each outputPixel in textToDraw and change it's corresponding 
            for (int i = 0; i < textToDraw.Count; i++)
            {
                for (int j = 0; j < textToDraw[0].Count; j++)
                {
                    Vector2 positionToDraw = new(i + (int)topLeftPosition.X, j + (int)topLeftPosition.Y);

                    // If the coordinates are in bounds, draw the character
                    if (positionToDraw.X < width && positionToDraw.Y < height && positionToDraw.X >= 0 && positionToDraw.Y >= 0)
                    {
                        // If the current character is not transparent, draw it onto the displayBoard
                        OutputPixel charToDraw = textToDraw[i][j];

                        OutputPixel originalChar;
                        
                        // Get the current pixel to combine it with the pixel to draw
                        if (i < width && j < height && i >= 0 && j >= 0)
                        {
                            originalChar = _displayBoard[i][j];
                        }
                        else
                        {
                            originalChar = new OutputPixel(0);
                        }
                        
                        OutputPixel finalChar = CombineChars(originalChar, charToDraw, blendingMode);

                        if (!charToDraw.IsTransparent(transparentPixel)) _displayBoard[(int)positionToDraw.X][(int)positionToDraw.Y] = finalChar;

                    }
                }
            }
        }

        public static OutputPixel CombineChars(OutputPixel originalPixel, OutputPixel pixelToDraw, BlendingMode blendingMode)
        {
            switch (blendingMode)
            {
                case BlendingMode.InFront:
                    return new(pixelToDraw);

                case BlendingMode.Behind:
                    if (originalPixel.brightness == 0)
                    {
                        return new(pixelToDraw.brightness);
                    }
                    return new(originalPixel.brightness);

                case BlendingMode.Multiply:
                    return new(originalPixel.brightness * pixelToDraw.brightness);

                case BlendingMode.Addition:
                    return new(originalPixel.brightness + pixelToDraw.brightness);

                case BlendingMode.Subtract:
                    return new(originalPixel.brightness - pixelToDraw.brightness);

                case BlendingMode.Without:
                    return new(pixelToDraw.brightness - originalPixel.brightness);

                default:
                    throw new Exception("No blending mode provided!");
            }
        }

        public void PrettyPrint(double time)
        {
            HandleNext("", false, time, true);
        }

        public void HandleNext(string indent, bool last, double time, bool isActive)
        {
            string newName = $"{name} {lastExecutionTime}ms";
            PrintHierarchy(newName, indent, last, true, false, isActive, out string newIndent);

            for (int i = 0; i < activeGenerators.Count; i++)
                activeGenerators[i].HandleNext(newIndent, i == activeGenerators.Count - 1, time, isActive);
        }

        public static void GetAllPlugins()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] allClasses = assembly.GetTypes();//.Where(t => String.Equals(t.Namespace, "ASCIIMusicVisualiser8", StringComparison.Ordinal)).ToArray();

            // Show plugins
            Console.WriteLine("Plugins---------------");
            foreach (Type t in allClasses)
            {
                
                bool isPlugin = t.IsSubclassOf(Type.GetType("ASCIIMusicVisualiser8.Plugin"));
                if (isPlugin)
                {
                    // Write all the parameters of the plugin
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(t.Name);
                    Console.ForegroundColor = ConsoleColor.White;

                    ParameterProcessor p = (ParameterProcessor)Activator.CreateInstance(t);

                    // Initialize their parameters, and print their flags
                    p.InitializeParameters();
                    if (p.pluginParameters == null) continue;
                    foreach (var parameter in p.pluginParameters)
                        Console.WriteLine(string.Join("\t", parameter.parameterFlags.Reverse()));

                    Console.WriteLine();

                }

            }
            Console.WriteLine();

            // Show effects
            Console.WriteLine("Effects---------------");
            foreach (Type t in allClasses)
            {
                bool isEffect = t.IsSubclassOf(Type.GetType("ASCIIMusicVisualiser8.Effect"));
                if (isEffect)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(t.Name);
                    Console.ForegroundColor = ConsoleColor.White;

                    ParameterProcessor p = (ParameterProcessor)Activator.CreateInstance(t);

                    // Initialize their parameters, and print their flags
                    p.InitializeParameters();
                    if (p.pluginParameters == null)
                    {
                        Console.WriteLine();
                        continue;
                    }
                    foreach (var parameter in p.pluginParameters)
                        Console.WriteLine(string.Join("\t", parameter.parameterFlags.Reverse()));

                    Console.WriteLine();
                }
            }
        }

    }
}
