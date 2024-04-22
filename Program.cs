using System.Numerics;

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
            
            Display display = new Display(160, "Audio/Master.mp3", new Vector2(200, 50));

            Generator swirlingTubesGenerator = new Generator("Tubes", new SwirlingTubes());
            swirlingTubesGenerator.plugin.@class.ProcessParameterString("--size 20,60");
            //display.AddGenerator(swirlingTubesGenerator);

            return display;
            
            
        }
    }
}


 
