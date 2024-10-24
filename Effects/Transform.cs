﻿using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Sets position to draw the text.
    /// <b>X Position Interpolation</b>: Dwisott. <i>(--xPositionInterpolation, -xPI)</i><br/>
    /// <b>y Position Interpolation</b>: Dwisott. <i>(--yPositionInterpolation, -yPI)</i><br/>
    /// <br/>
    /// 
    /// Rotates output by a given angle around a point.
    /// <b>Rotation Interpolation</b>: How much to rotate by. <i>(--rotationInterpolation, -rI)</i><br/>
    /// <br/>
    /// 
    /// Scales the image about the center to draw the text.
    /// <b>Scale Interpolation</b>: Dwisott. <i>(--scaleInterpolation, -sI)</i><br/>
    /// </summary>
    public class Transform : Effect
    {

        InterpolationGraph xPosInterpolation;
        InterpolationGraph yPosInterpolation;

        InterpolationGraph rotationInterpolation;

        InterpolationGraph scaleInterpolation;

        long posTime;
        long scaleTime;
        long rotTime;

        public Transform() : base() {}
        public Transform(string parameterString) : base(parameterString) {}

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("xPosInterpolation", new string[] { "--xPosInterpolation", "-xPI"}, ""),
                new PluginParameter("yPosInterpolation", new string[] { "--yPosInterpolation", "-yPI"}, ""),

                new PluginParameter("rotInterpolation", new string[] { "--rotInterpolation", "-rI"}, ""),

                new PluginParameter("scaleInterpolation", new string[] { "--scaleInterpolation", "-sI"}, ""),
            };
        }

        public override void Init()
        {
            xPosInterpolation = new InterpolationGraph(GetPluginParameter("xPosInterpolation").givenUserParameter);
            yPosInterpolation = new InterpolationGraph(GetPluginParameter("yPosInterpolation").givenUserParameter);

            var res = GetPluginParameter("rotInterpolation").givenUserParameter;
            rotationInterpolation = new InterpolationGraph(res);

            scaleInterpolation = new InterpolationGraph(GetPluginParameter("scaleInterpolation").givenUserParameter);

            name = "Transform";
        }

        public override List<List<OutputPixel>> ApplyTo(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {
            var watch = Stopwatch.StartNew();
            List<List<OutputPixel>> currentGrid = input;
            OutputPixel currentTransparentChar = transparentChar;
            Vector2 currentDrawPoint = drawPoint;

            // FIX APPLY SCALE NOT WORKING PROPERLY


            var scaleT = Stopwatch.StartNew();
            currentGrid = ApplyScale(currentGrid, beat, currentTransparentChar, currentDrawPoint, out currentTransparentChar, out currentDrawPoint);
            scaleT.Stop();
            scaleTime = scaleT.ElapsedTicks;

            // Position is fine
            var posT = Stopwatch.StartNew();
            currentGrid = ApplyPosition(currentGrid, beat, currentTransparentChar, currentDrawPoint, out currentTransparentChar, out currentDrawPoint);
            posT.Stop();
            posTime = posT.ElapsedTicks;

            // --Rotation is NOT fine--
            // IT IS NOW!!!
            var rotT = Stopwatch.StartNew();
            currentGrid = ApplyRotation(currentGrid, beat, currentTransparentChar, currentDrawPoint, out currentTransparentChar, out currentDrawPoint);
            rotT.Stop();
            rotTime = rotT.ElapsedTicks;

            newTransparentChar = currentTransparentChar;
            newDrawPoint = currentDrawPoint;

            watch.Stop();
            lastExecutedTime = watch.ElapsedTicks;
            return currentGrid;
        }

        List<List<OutputPixel>> ApplyPosition(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {

            newTransparentChar = transparentChar;

            int originalHeight = input.Count;
            int originalWidth = input[0].Count;

            int x = (int)Math.Floor(xPosInterpolation.GetTime(beat));
            int y = (int)Math.Floor(yPosInterpolation.GetTime(beat));

            Vector2 center = new Vector2(originalHeight / 2f, originalWidth / 2f);
            newDrawPoint = drawPoint + new Vector2(x, y);
            return input;
            
            /*

            List<List<OutputPixel>> finalGrid = new List<List<OutputPixel>>();

            int originalXLength = input[0].Count;
            int x = (int)Math.Round(xPositionInterpolation.GetTime(beat));
            int finalXLength = x + originalXLength;

            int originalYLength = input.Count;
            int y = (int)Math.Round(yPositionInterpolation.GetTime(beat));
            int finalYLength = y + originalYLength;


            //! IGNORE THE BELOW! Further operations may bring back the data from being out of bounds.
            
            // Don't bother rendering if nothing's going to appear on screen.
            // As such, don't bother changing the draw point.
            if (
                y < -input.Count ||// ------------------------ HERE
                x < -input[0].Count ||
                y >= gridSize.Y ||
                x >= gridSize.X
                )
            {
                newDrawPoint = drawPoint;
                return finalGrid;
            }
            

            // Caculate the draw point based on the center offset
            newDrawPoint = drawPoint - center;
            //newDrawPoint = drawPoint;

            // If the y position is positive...
            if (y >= 0)
            {
                // Add blank rows
                for (int i = 0; i < y; i++)
                {
                    finalGrid.Add(Utility.Repeat.RepeatNTimesToList(transparentChar, finalXLength));
                }
            }
            else // Otherwise...
            {
                // Chop the input list based on the y position
                input = input.GetRange(-y, input.Count - -y);
            }


            foreach (List<char> inputRow in input)
            {
                List<char> rowToAdd = new();

                if (x >= 0) // If x is positive...
                {
                    // Combine a row of blank chars before each row of the input chargrid
                    rowToAdd = Utility.Repeat.RepeatNTimesToList(transparentChar, x);
                    rowToAdd.AddRange(inputRow);
                }
                else // Otherwise...
                {
                    // Chop the row based on the x position
                    rowToAdd = inputRow.GetRange(-x, inputRow.Count - -x);
                }

                finalGrid.Add(rowToAdd);

            }
            */


            //return finalGrid;
        }


        List<List<OutputPixel>> ApplyRotation(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {

            float rotationAmount = (float)(rotationInterpolation.GetTime(beat) * Math.PI / 180);

            if (rotationAmount == 0)
            {
                newTransparentChar = transparentChar;
                newDrawPoint = drawPoint;
                return input;
            }


            int width = input[0].Count;
            int height = input.Count;

            int rotatedWidth = (int)Math.Round(Math.Abs(width * Math.Cos(rotationAmount)) + Math.Abs(height * Math.Sin(rotationAmount)));
            int rotatedHeight = (int)Math.Round(Math.Abs(height * Math.Cos(rotationAmount)) + Math.Abs(width * Math.Sin(rotationAmount)));

            List<List<OutputPixel>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(rotatedWidth, rotatedHeight));
            //List<List<OutputPixel>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(width * 16, height * 16));
            //Console.WriteLine($"{rotatedWidth} {rotatedHeight} {width} {height}");
            Vector2 center = new(width / 2f, height / 2f);
            Vector2 rotatedCenter = new(rotatedWidth / 2f, rotatedHeight / 2f);

            //Vector2 newDrawPointTemp = new Vector2(width, height) / 2f - new Vector2(rotatedCenter.X, rotationCenter.Y);

            //newDrawPoint = drawPoint;//-new Vector2((int)rotatedCenter.X, (int)rotatedCenter.Y)/2 + Vector2.One * 20;


            //! CALCULATE THE VECTOR2 DIMENSIONS BASED ON THE INPUT SIZE AND THE ROTATION AMOUNT
            // (done)
            //List<List<OutputPixel>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(10, 10));

            for (int y_ = 0; y_ < rotatedHeight; y_++)
            {
                for (int x_ = 0; x_ < rotatedWidth; x_++)
                {

                    int y = y_ + (int)center.Y - (int)rotatedCenter.Y;
                    int x = x_ + (int)center.X - (int)rotatedCenter.X;



                    Vector2 pointToSampleFrom = RotatePointAround(center, new(x, y), -rotationAmount);
                    
                    //pointToSampleFrom += rotationCenter/2;

                    int sampleX = (int)(Math.Round(pointToSampleFrom.X - 0.5f));
                    int sampleY = (int)(Math.Round(pointToSampleFrom.Y - 0.5f));
                    // Do these need rounding?
                    int targetY = y_;// - (int)(height) + (int)(rotatedHeight);
                    int targetX = x_;// - (int)(width) + (int)(rotatedWidth);

                    OutputPixel sampledChar;

                    /*
                    if (
                        targetY < 0 ||
                        targetX < 0 ||
                        targetY >= rotatedHeight ||
                        targetX >= rotatedWidth
                        )
                    {
                        //finalGrid[targetY][targetX] = '#';
                        continue;
                    }
                    */


                    /*
                    if (
                        sampleY < 0 ||
                        sampleX < 0 ||
                        sampleY >= height ||
                        sampleX >= width
                        )
                    {
                        continue;
                    }
                    else
                    {
                    }
                    */
                    if (
                        sampleY >= 0 &&
                        sampleX >= 0 &&
                        sampleY < height &&
                        sampleX < width
                        )
                    {
                        sampledChar = input[sampleY][sampleX];
                        /*
                        Console.WriteLine($"{sampleY} {sampleX} {input[sampleY][sampleX]}");
                        Console.ReadLine();
                        sampledChar = '.';
                        */

                    }
                    else
                    {
                        sampledChar = transparentChar;
                        /*
                        Console.WriteLine($"UH OH!!!");
                        Console.WriteLine($"{sampleY} {sampleX}");
                        Console.ReadLine();
                        */
                        // Happens if the sample is out of range of original datagrid BUT not if 
                    }


                    finalGrid[targetY][targetX] = sampledChar;


                    //finalGrid[y][x] = '';
                }
            }

            //finalGrid[(int)rotatedCenter.Y][(int)rotatedCenter.X] = '!';

            newTransparentChar = transparentChar;
            //newDrawPoint = drawPoint - new Vector2((int)rotatedCenter.Y, (int)rotatedCenter.X) + new Vector2(center.Y, center.X);
            newDrawPoint = drawPoint - new Vector2((int)rotatedCenter.Y, (int)rotatedCenter.X) + new Vector2(center.Y, center.X);


            //finalGrid[0][0] = '=';
            return finalGrid;
        }


        List<List<OutputPixel>> ApplyScale(List<List<OutputPixel>> input, double beat, OutputPixel transparentChar, Vector2 drawPoint, out OutputPixel newTransparentChar, out Vector2 newDrawPoint)
        {

            // Unused for now
            //int xScaleCenter = (int)Math.Round(xScaleInterpolation.GetTime(beat));
            //int yScaleCenter = (int)Math.Round(yScaleInterpolation.GetTime(beat));

            float scaleFactor = 1 / (float)scaleInterpolation.GetTime(beat);


            if (scaleFactor == -1)
            {
                newTransparentChar = transparentChar;
                newDrawPoint = drawPoint;
                return input;
            }


            List<List<OutputPixel>> finalGrid = new List<List<OutputPixel>>();

            int originalWidth = input[0].Count;
            int originalHeight = input.Count;

            int scaledWidth = (int)Math.Ceiling(originalWidth / scaleFactor);
            int scaledHeight = (int)Math.Ceiling(originalHeight / scaleFactor);

            // Y, X
            Vector2 center = new(scaledWidth / 2f, scaledHeight / 2f);



            // Populate finalGrid with nearest neighbour interpolation
            for (int i = 0; i < scaledHeight; i++)
            {
                List<OutputPixel> currentRow = new List<OutputPixel>();
                for (int j = 0; j < scaledWidth; j++)
                {
                    int xUnscaled = (int)Math.Floor(j * scaleFactor);
                    int yUnscaled = (int)Math.Floor(i * scaleFactor);

                    OutputPixel charToAdd;


                    if (yUnscaled < originalHeight && xUnscaled < originalWidth)
                    {
                        charToAdd = input[yUnscaled][xUnscaled];
                    }
                    else
                    {
                        charToAdd = transparentChar;
                    }

                    currentRow.Add(charToAdd);
                }
                finalGrid.Add(currentRow);
            }

            newTransparentChar = transparentChar;
            Vector2 offset = drawPoint - center;

            Vector2 scaleDrawPoint = new Vector2((originalWidth - scaledWidth)/2f, (originalHeight - scaledHeight)/2f);

            // Wait wait wait
            center = new Vector2(finalGrid.Count/2f, finalGrid[0].Count/2f);
            newDrawPoint = drawPoint - center;
            // 1 -> -center
            // 2 -> 

            return finalGrid;
        }

        public static Vector2 RotatePointAround(Vector2 originalPoint, Vector2 pointOfRotation, float degrees)
        {
            float deltaX = pointOfRotation.X - originalPoint.X;
            float deltaY = pointOfRotation.Y - originalPoint.Y;

            float newX = (float)Math.Cos(degrees) * deltaX - (float)Math.Sin(degrees) * deltaY;
            float newY = (float)Math.Sin(degrees) * deltaX + (float)Math.Cos(degrees) * deltaY;


            return new(originalPoint.X + newX, originalPoint.Y + newY);

        }

        public override string ShowParameterValues(double time)
        {
            return $"xyPos <{xPosInterpolation.GetTime(time).ToString("0.00")},{yPosInterpolation.GetTime(time).ToString("0.00")}> " +
                $"-rI {rotationInterpolation.GetTime(time).ToString("0.00")} " +
                $"-sI {scaleInterpolation.GetTime(time).ToString("0.00")} " +
                $"scaleTime: {scaleTime}t\t|" +
                $"posTime: {posTime}t\t|" +
                $"rotTime: {rotTime}t\t|" +
                $"totalTime: {lastExecutedTime}t\t|";
        }

    }
}

