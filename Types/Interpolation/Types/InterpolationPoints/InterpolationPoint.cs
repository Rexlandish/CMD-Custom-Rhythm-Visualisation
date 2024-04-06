﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility;

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
        public double[] curveVariable { get; private set; }

        public InterpolationPoint(double startTime, double endTime, double startValue, double endValue, string interpolationCurveName, double[] curveVariable = null)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.startValue = startValue;
            this.endValue = endValue;
            this.interpolationCurveName = interpolationCurveName;
            this.curveVariable = curveVariable;
        }

        public InterpolationPoint(double startTime, double startValue, string interpolationCurveName, double[] curveVariable = null)
        {
            this.startTime = startTime;
            this.startValue = startValue;
            this.endTime = double.NaN;
            this.endValue = double.NaN;
            this.interpolationCurveName = interpolationCurveName;
            this.curveVariable = curveVariable;
        }

        public string ExportToString()
        {
            string outputString = $"{startTime},{startValue}:{endTime},{endValue}:{interpolationCurveName},{Utility.ArrayToString(curveVariable)}";
            return outputString;
        }

        public InterpolationPoint ImportFromString(string input)
        {
            throw new NotImplementedException(); //! DO THIS NEXT TIME
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
            double newInterpolationCurveAmount = Curve.GetInterpolation(interpolationCurveAmount, curveVariable, interpolationCurveName);
            return Utility.InverseLerp(startValue, endValue, newInterpolationCurveAmount);
        }


    }
}
