using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using NAudio.Dsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Rotates output by a given angle around a point.
    /// <b>Rotation Interpolation</b>: How much to rotate by. <i>(--rotationInterpolation, -rI)</i><br/>
    /// </summary>
    public class Rotate: Effect
    {
        
        // Variables here
        InterpolationGraph rotationInterpolation;
        Vector2 rotationCenter;

        public Rotate() : base() { }
        public Rotate(string parameterString) : base(parameterString) { }

        public override void InitializeParameters()
        {

            // Initialize variables here

        
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("rotationInterpolation", new string[] { "--rotationInterpolation", "-rI"}, ""),
            };
        
        }

        public override void Init()
        {
            rotationInterpolation = new InterpolationGraph(GetPluginParameter("rotationInterpolation").givenUserParameter);
            /*
            scaleFactorInterpolation = new InterpolationGraph(GetPluginParameter("scaleFactorInterpolation").givenUserParameter);
            xCenterInterpolation = new InterpolationGraph(GetPluginParameter("xCenterInterpolation").givenUserParameter);
            yCenterInterpolation = new InterpolationGraph(GetPluginParameter("yCenterInterpolation").givenUserParameter);
            */
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, Vector2 drawPoint, out char newTransparentChar, out Vector2 newDrawPoint)
        {

            float rotationAmount = (float)rotationInterpolation.GetTime(beat);

            // Convert degrees to radians
            rotationAmount /= 180f;
            rotationAmount *= (float)Math.PI;



            int width = input[0].Count;
            int height = input.Count;

            int rotatedWidth = (int)Math.Round(Math.Abs(width * Math.Cos(rotationAmount)) + Math.Abs(height * Math.Sin(rotationAmount)));
            int rotatedHeight = (int)Math.Round(Math.Abs(height * Math.Cos(rotationAmount)) + Math.Abs(width * Math.Sin(rotationAmount)));

            //List<List<char>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(100, 100));
            List<List<char>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(rotatedWidth, rotatedHeight));
            //List<List<char>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(width * 16, height * 16));
            //Console.WriteLine($"{rotatedWidth} {rotatedHeight} {width} {height}");
            Vector2 center = new(width / 2f, height / 2f);      
            Vector2 rotatedCenter = new(rotatedWidth / 2f, rotatedHeight / 2f);

            //Vector2 newDrawPointTemp = new Vector2(width, height) / 2f - new Vector2(rotatedCenter.X, rotationCenter.Y);

            //newDrawPoint = drawPoint;//-new Vector2((int)rotatedCenter.X, (int)rotatedCenter.Y)/2 + Vector2.One * 20;
            

            //! CALCULATE THE VECTOR2 DIMENSIONS BASED ON THE INPUT SIZE AND THE ROTATION AMOUNT
            //List<List<char>> finalGrid = Utility.Creation.Create2DArray(transparentChar, new(10, 10));

            
            // Find out why the data is not being written to the right place!!!!!!

            for (int y_ = 0; y_ < rotatedHeight; y_++)
            {
                for (int x_ = 0; x_ < rotatedWidth; x_++)
                {

                    int y = y_ + (int)center.Y - (int)rotatedCenter.Y;
                    int x = x_ + (int)center.X - (int)rotatedCenter.X;

                    Vector2 pointToSampleFrom = RotatePointAround(center, new(x, y), -rotationAmount);

                    //pointToSampleFrom += rotationCenter/2;

                    int sampleX = (int)(Math.Round(pointToSampleFrom.X));
                    int sampleY = (int)(Math.Round(pointToSampleFrom.Y));

                    // Do these need rounding?
                    int targetY = y_;// - (int)(height) + (int)(rotatedHeight);
                    int targetX = x_;// - (int)(width) + (int)(rotatedWidth);

                    char sampledChar;

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
                        sampledChar = '.';
                        /*
                        Console.WriteLine($"UH OH!!!");
                        Console.WriteLine($"{sampleY} {sampleX}");
                        Console.ReadLine();
                        */
                        // Happens if the sample is out of range of original datagrid BUT not if 
                    }


                    finalGrid[targetY][targetX] = sampledChar;
                    
                    //finalGrid[y][x] = '#';
                }
            }

            finalGrid[(int)rotatedCenter.Y][(int)rotatedCenter.X] = '!';

            newTransparentChar = new char();
            newDrawPoint = drawPoint - new Vector2(rotatedCenter.Y, rotatedCenter.X);
             
            return finalGrid;
        }

        public static Vector2 RotatePointAround(Vector2 originalPoint, Vector2 pointOfRotation, float degrees)
        {
            float deltaX = pointOfRotation.X - originalPoint.X;
            float deltaY = pointOfRotation.Y - originalPoint.Y;

            float newX = (float)Math.Cos(degrees) * deltaX - (float)Math.Sin(degrees) * deltaY;
            float newY = (float) Math.Sin(degrees) * deltaX + (float)Math.Cos(degrees) * deltaY;


            return new(originalPoint.X + newX, originalPoint.Y + newY);

            /*
            // Convert cartesian coordinates to polar coordinates
            double theta = Math.Atan2(deltaY, deltaX);
            double radius = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));

            // Convert from degrees input to radians and add it to the theta
            degrees *= (Math.Atan(1) * 4) / 180;
            theta -= degrees;

            // Convert back to cartesian coordinates
            double rotatedX = radius * -Math.Cos(theta);
            double rotatedY = radius * -Math.Sin(theta);

            double finalXPosition = pointOfRotation.X + rotatedX;
            double finalYPosition = pointOfRotation.Y + rotatedY;

            Vector2 finalPoint = new Vector2((float)finalXPosition, (float)finalYPosition);
            return finalPoint;
            */

        }
    }
}

