using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Types.Interpolation
{ 

    // A library that hands interpolation between points
    public abstract class InterpolationCurve
    {
        // All normalized from 0 to 1
        public static double GetInterpolation(double t, string name)
        {
            switch (name)
            {
                case "linear":
                    return Linear(t);
                case "easeoutelastic":
                    return EaseOutElastic(t);
                default:
                    throw new Exception($"Interpolation {name} not found.");
            }
        }

        static double Linear(double t)
        {
            return t;
        }

        static double EaseIn(double t)
        {
            return t;
        }

        static double EaseOutElastic(double t)
        {
            double c4 = (2 * Math.PI) / 3;

            return t == 0
              ? 0
              : t == 1
              ? 1
              : Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1;
        }

        static double EaseInOutElastic(double t)
        {
            double c5 = (2 * Math.PI) / 4.5;

            return t == 0
              ? 0
              : t == 1
              ? 1
              : t < 0.5
              ? -(Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125) * c5)) / 2
              : (Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125) * c5)) / 2 + 1;
        }

    }
}

