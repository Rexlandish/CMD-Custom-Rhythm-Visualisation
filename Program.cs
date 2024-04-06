using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility;
using System.Numerics;
using System.IO;
using System.IO.Pipes;
using ASCIIMusicVisualiser8.Types.Interpolation;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;

namespace ASCIIMusicVisualiser8
{
    internal class Program
    {


        static void Main(string[] args)
        {



            
            Display display = CreateDisplay();
            display.Run();
            


            /*
            SwirlingTubes swirlingTubes = new SwirlingTubes();
            Console.WriteLine(StringifyCharlist(swirlingTubes.Generate(0)));
            */

        }

        public static Display CreateDisplay()
        {
            
            Display display = new Display(117, "Audio/happyCropped.mp3", new Vector2(200, 50));

            /*
            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes(), 0);
            swirlingTubesGenerator.plugin.pluginAsClass.ProcessParameterString("--size 20,60");
            display.AddGenerator(swirlingTubesGenerator);
            */

            return display;
            
            
        }
    }
}


 
