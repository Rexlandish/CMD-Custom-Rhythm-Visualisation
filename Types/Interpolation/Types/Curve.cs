﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Types.Interpolation
{ 

    // A library that hands interpolation between points
    public abstract class Curve
    {
        // All normalized from 0 to 1
        public static double GetInterpolation(double t, double[] curveVariables, string name)
        {
            switch (name)
            {
                case "hold":
                case "":
                    return 0;
                case "linear":
                    return Linear(t);
                case "easeOutElastic":
                    return EaseOutElastic(t, curveVariables);
                case "easeOut":
                    return EaseOut(t, curveVariables);
                case "easeIn":
                    return EaseIn(t, curveVariables);
                case "easeInOutSin":
                    return EaseInOutSin(t);
                case "cos":
                    return Cos(t, curveVariables);
                case "sin":
                    return Sin(t, curveVariables);
                case "bounce":
                    return Bounce(t, curveVariables);
                default:
                    throw new Exception($"Interpolation {name} not found.");
            }
        }

        public static double Cos(double t, double[] curveVariables)
        {
            if (curveVariables.Length == 0)
            {
                throw new Exception("No curve variables provided!");
            }

            double curveCount = curveVariables[0];
            return (Math.Cos(curveCount * Math.PI * t) + 1) / 2;
        }
        public static double Sin(double t, double[] curveVariables)
        {
            if (curveVariables.Length == 0)
            {
                throw new Exception("No curve variables provided!");
            }

            double curveCount = curveVariables[0];
            return (Math.Sin(curveCount * Math.PI * t) + 1) / 2;
        }

        public static double Linear(double t)
        {
            return t;
        }

        public static double EaseIn(double t, double[] curveVariables)
        {
            if (curveVariables.Length == 0)
            {
                throw new Exception("No curve variables provided!");
            }
            double power = curveVariables[0];

            return Math.Pow(t, power);
        }

        public static double EaseOut(double t, double[] curveVariables)
        {
            if (curveVariables.Length == 0)
            {
                throw new Exception("No curve variables provided!");
            }
            double power = curveVariables[0];
            return 1 - Math.Pow(1 - t, power);
        }

        public static double EaseOutElastic(double t, double[] curveVariables)
        {
            double c4 = (2 * Math.PI) / 3;

            return t == 0
              ? 0
              : t == 1
              ? 1
              : Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1;
        }

        public static double EaseInOutElastic(double t, double[] curveVariables)
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

        public static double EaseInOutSin(double t)
        {
            return -(Math.Cos(Math.PI * t) - 1) / 2;
        }

        public static double Bounce(double t, double[] curveVariables)
        {
            return 2 * Math.Abs(Sin(t, curveVariables) - 0.5);
        }
    }
}

