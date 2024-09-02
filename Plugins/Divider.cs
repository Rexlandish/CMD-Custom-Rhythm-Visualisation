using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;

namespace ASCIIMusicVisualiser8
{
    public class Divider : Plugin, IPlugin
    {

        public override string pluginName {get => "Divider"; }

        Vector2 size;
        InterpolationGraph xPositionInterpolation;
        InterpolationGraph yPositionInterpolation;
        InterpolationGraph angleInterpolation;


        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */


        public Divider(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("xPositionInterpolation", new string[] {"--xPositionInterpolation", "-xPI"}, ""),
                new PluginParameter("yPositionInterpolation", new string[] {"--yPositionInterpolation", "-yPI"}, ""),
                new PluginParameter("angleInterpolation", new string[] {"--angleInterpolation", "-aI"}, ""),
                new PluginParameter("size", new string[] {"--size", "-s"}, "")
            };
        }

        public override void Init()
        {

            string[] sizeVector = GetPluginParameter("size").givenUserParameter.Split(',');
            size = new Vector2(
                float.Parse(sizeVector[0]),
                float.Parse(sizeVector[1])
            );

            xPositionInterpolation = new InterpolationGraph(GetPluginParameter("xPositionInterpolation").givenUserParameter);
            yPositionInterpolation = new InterpolationGraph(GetPluginParameter("yPositionInterpolation").givenUserParameter);

            angleInterpolation = new InterpolationGraph(GetPluginParameter("angleInterpolation").givenUserParameter);



            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);

        }

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(new OutputPixel(0), size);
            

            Vector2 centerPoint = new Vector2(
                (float)xPositionInterpolation.GetTime(beat),
                (float)yPositionInterpolation.GetTime(beat)
            );

            double currentAngleLimit = angleInterpolation.GetTime(beat);
            
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {

                    //swirlSpeed = 8 * Sin01(Math.Sin(beat)) + 8;

                    double _i = i;
                    double _j = j;
                    double opacity = 0;
                    Vector2 currentPoint = new Vector2(i, j);

                    double currentAngle = AngleBetweenCoordinates(centerPoint, currentPoint);
                    double scaledValue = (((currentAngle - currentAngleLimit) + 360) % 360) / 180;
                    
                    if (scaledValue < 1)
                        opacity = 1;
                    
                    //opacity = scaledValue / 1;


                    /*
                     * 
                     * YOUR CODE HERE
                     * 
                     */

                    finalArray[i][j] = new((float)opacity);
                }
            }

            transparentChar = new(' ');
            return finalArray;
        }
        
        public double AngleBetweenCoordinates(Vector2 c1, Vector2 c2)
        {
            float deltaX = c2.X - c1.X;
            float deltaY = c2.Y - c1.Y;

            return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

        }

        public override string ShowParameterValues(double time)
        {
            return $"xyCenter <{xPositionInterpolation.GetTime(time).ToString("0.0")},{yPositionInterpolation.GetTime(time).ToString("0.0")}> -a {angleInterpolation.GetTime(time).ToString("0.0")}";
        }

    }
}
