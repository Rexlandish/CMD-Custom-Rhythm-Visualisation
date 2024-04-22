using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility;

namespace ASCIIMusicVisualiser8.Types.Interpolation.Types
{
    internal class InterpolationGraph : IStringable<InterpolationGraph>
    {
        List<InterpolationPoint> points = new();

        public void SetPoints(List<InterpolationPoint> points)
        {
            this.points = points;
            Initialize();
        }

        // Set the end values of a point to the next point's start values
        public void Initialize()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (double.IsNaN(points[i].endTime) || double.IsNaN(points[i].endValue))
                {
                    points[i].SetNext(points[i + 1]);
                }
            }

            // Final point
            var finalPoint = points[points.Count - 1];
            if (double.IsNaN(finalPoint.endTime) || double.IsNaN(finalPoint.endValue))
            {
                finalPoint.SetEndPoint(finalPoint.startTime + 1, finalPoint.startValue);
            }

            // Should there be a common function to check if point is missing an end value and end time?
        }

        public void Print()
        {
            foreach (var point in points)
            {
                string parameterText = point.curveParameters != null ? string.Join(",", point.curveParameters) : "";
                Console.WriteLine($"{point.startTime} -> {point.endTime}, {point.startValue} -> {point.endValue}, {point.interpolationCurveName}, {parameterText}");
            }
        }

        public double GetTime(double time)
        {
            
            // Find what region the value falls into, and calculate the value
            var regionHoldingValue = points.Find(point => point.endTime >= time);
            var regionHoldingIndex = points.FindIndex(point => point.endTime >= time);
            if (regionHoldingValue == null)
            {
                // set it to the last point in points
                regionHoldingValue = points[points.Count - 1];
            }

            //Console.WriteLine($"{regionHoldingValue.startTime}, {regionHoldingValue.endTime}");
            // How far the time is between the two points
            double inverseLerpValue = Utility.InverseLerp(regionHoldingValue.startTime, regionHoldingValue.endTime, time);

            return regionHoldingValue.GetValue(inverseLerpValue);
        }

        public string ExportToString()
        {
            List<string> finalString = new();

            foreach (var point in points)
            {
                string pointText = ((IStringable<InterpolationPoint>)point).ExportToString();
                finalString.Add(pointText);
            }
            return "[" + string.Join(",", finalString) + "]";
        }

        public InterpolationGraph ImportFromString(string input)
        {
            throw new NotImplementedException(); //! DO THIS NEXT TIME
        }

    }
}
