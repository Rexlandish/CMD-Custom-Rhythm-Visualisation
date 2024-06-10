
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using static ASCIIMusicVisualiser8.Generator;
using static ASCIIMusicVisualiser8.Utility.ConsoleOp;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using static ASCIIMusicVisualiser8.Utility.Creation;

namespace ASCIIMusicVisualiser8
{
    internal class Display
    {
        public Conductor Conductor { get; private set; }

        // Visual data
        public int updateTimeMilliseconds = 2;
        Vector2 dimensions;

        bool isActivated = true;



        public Display(Vector2 dimensions)
        { 
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

            var currentFrameDisplayBoard = Create2DArray(' ', dimensions);

            foreach (var generator in activeGenerators)
            {
                DrawLayer(currentFrameDisplayBoard, generator, beat);
            }

            string charlistToString = StringifyCharlist(currentFrameDisplayBoard);
            
            return charlistToString;
        }


        public void Stop()
        {
            isActivated = false;
        }

        public static void DrawLayer(List<List<char>> displayBoard, Generator generator, double currentBeat)
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
            DrawLayer(displayBoard, gOutput.position, gOutput.text, generator.blendingMode, gOutput.transparentChar);

        }

        static void DrawLayer(List<List<char>> _displayBoard, Vector2 topLeftPosition, List<List<char>> textToDraw, BlendingMode blendingMode, char? transparentChar = null)
        {

            int height = _displayBoard[0].Count;
            int width = _displayBoard.Count;


            // Iterate through each char in textToDraw and change it's corresponding 
            for (int i = 0; i < textToDraw.Count; i++)
            {
                for (int j = 0; j < textToDraw[0].Count; j++)
                {
                    Vector2 positionToDraw = new(i + (int)topLeftPosition.X, j + (int)topLeftPosition.Y);

                    // If the coordinates are in bounds, draw the character
                    if (positionToDraw.X < width && positionToDraw.Y < height && positionToDraw.X >= 0 && positionToDraw.Y >= 0)
                    {
                        // If the current character is not transparent, draw it onto the displayBoard
                        char charToDraw = textToDraw[i][j];

                        char originalChar;
                        if (i < width && j < height && i >= 0 && j >= 0)
                        {
                            originalChar = _displayBoard[i][j];
                        }
                        else
                        {
                            originalChar = ' ';
                        }
                        
                        char finalChar = CombineChars(originalChar, charToDraw, blendingMode);

                        if (charToDraw != transparentChar) _displayBoard[(int)positionToDraw.X][(int)positionToDraw.Y] = finalChar;

                    }
                }
            }
        }

        public static char CombineChars(char originalChar, char charToDraw, BlendingMode blendingMode)
        {
            double originalValue = GetDensityFromChar(originalChar);
            double valueToDraw = GetDensityFromChar(charToDraw);


            switch (blendingMode)
            {
                case BlendingMode.InFront:
                    return charToDraw;

                case BlendingMode.Behind:
                    if (originalValue == 0)
                    {
                        return charToDraw;
                    }
                    return originalChar;

                case BlendingMode.Multiply:
                    return GetCharFromDensity(originalValue * valueToDraw);

                case BlendingMode.Addition:
                    return GetCharFromDensity(originalValue + valueToDraw);

                case BlendingMode.Subtract:
                    return GetCharFromDensity(originalValue - valueToDraw);

                case BlendingMode.Without:
                    return GetCharFromDensity(valueToDraw - originalValue);

                default:
                    throw new Exception("No blending mode provided!");
            }
        }

    }
}
