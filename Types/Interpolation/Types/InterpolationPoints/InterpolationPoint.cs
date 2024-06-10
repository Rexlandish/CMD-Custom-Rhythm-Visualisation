using System;
using System.Collections.Generic;
using static ASCIIMusicVisualiser8.Utility;
using static ASCIIMusicVisualiser8.Utility.Maths;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8.Types.Interpolation
{
    /*
    Process

    1, "60", easein
    2, "90", easein
    3, "180", easein
    4, "270", easin
    5, "359", linear
    6, "0", hold
    */

    // Hnadles individual interpolation points in an interpolation graph
    public class InterpolationPoint : IStringable<InterpolationPoint>
    {
        public double startTime { get; private set; }
        public double endTime { get; private set; }

        public double startValue { get; private set; }
        public double endValue { get; private set; }
        /*
        protected string startValue;
        protected string endValue;
        */
        public string interpolationCurveName { get; private set; }
        public double[] curveParameters { get; private set; }

        public InterpolationPoint(double startTime, double endTime, double startValue, double endValue, string interpolationCurveName, double[] curveParameters)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.startValue = startValue;
            this.endValue = endValue;
            this.interpolationCurveName = interpolationCurveName;
            this.curveParameters = curveParameters;
        }

        public InterpolationPoint(double startTime, double startValue, string interpolationCurveName, double[] curveParameters)
        {
            this.startTime = startTime;
            this.startValue = startValue;
            this.endTime = double.NaN;
            this.endValue = double.NaN;
            this.interpolationCurveName = interpolationCurveName;
            this.curveParameters = curveParameters;
        }

        public InterpolationPoint()
        {
            this.startTime = double.NaN;
            this.startValue = double.NaN;
            this.endTime = double.NaN;
            this.endValue = double.NaN;
            this.interpolationCurveName = "";
            this.curveParameters = new double[0] { };
        }

        public string ExportToString()
        {
            string outputString = $"{startTime}>{endTime};{startValue}>{endValue};{interpolationCurveName};{ArrayToString(curveParameters)}";
            return outputString;
        }

        public InterpolationPoint ImportFromString(string input)
        {
            // 0>1;0>0.5;linear;[2,3]
            // 1>2;0.5>1;linear;[]
            // 2>3;1>1;linear;[]
            Console.WriteLine(input);
            string[] parameters = input.Split(';');
            
            // Time
            string[] times = parameters[0].Split('>');
            
            startTime = float.Parse(times[0]);
            if (times.Length == 2) // Set the end time if it's been given
                endTime = float.Parse(times[1]);

            // Values
            string[] values = parameters[1].Split('>');
            startValue = float.Parse(values[0]);
            if (times.Length == 2) // Set the end value if it's been given
                endValue = float.Parse(values[1]);

            if (parameters.Length >= 3)
            {
                // Interpolation curve
                interpolationCurveName = parameters[2];
            }
            else
            {
                interpolationCurveName = "hold";
            }


            if (parameters.Length >= 4)
            {
                // Curve parameters.
                // If no variables given, put in a blank array. Otherwise, parse it.
                if (parameters[3] == "[]")
                {
                    curveParameters = new double[0] { };
                }
                else
                {
                    string curveParametersString = parameters[3].Substring(1, parameters[3].Length - 2); // Ignore first and last characters
                    List<double> curveParameterList = new();
                    foreach (string value in curveParametersString.Split(','))
                    {
                        curveParameterList.Add(double.Parse(value));
                    }

                    curveParameters = curveParameterList.ToArray();
                }
            }
            else
            {
                curveParameters = new double[0] { };
            }

            return this;

        }


        public void SetStartPoint(double time, double value)
        {
            startTime = time;
            startValue = value;
        }

        public void SetEndPoint(double time, double value)
        {
            endTime = time;
            endValue = value;
        }

        public void SetNext(InterpolationPoint nextInterpolationPoint)
        {
            SetEndPoint(
                nextInterpolationPoint.startTime,
                nextInterpolationPoint.startValue
            );
        }

        public double GetValue(double interpolationCurveAmount) // From 0 to 1
        {
            double newInterpolationCurveAmount = Curve.GetInterpolation(interpolationCurveAmount, curveParameters, interpolationCurveName);
            return Lerp(startValue, endValue, newInterpolationCurveAmount);
        }


    }
}
