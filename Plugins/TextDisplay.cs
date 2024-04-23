using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Plugins
{
    internal class TextDisplay : Plugin, IPlugin
    {


        public override string pluginName => "TextDisplay";

        string[] words;
        Vector2[] positions;
        InterpolationGraph positionInterpolation;

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            int index = (int)Math.Floor(positionInterpolation.GetTime(beat));
            string wordToDisplayAsString = words[index];
            List<char> wordToDisplayAsCharArray = new List<char>(wordToDisplayAsString.ToCharArray());
            transparentChar = new char();

            return Utility.RepeatNTimesToList(wordToDisplayAsCharArray, 8);
            
            
        }

        public override void Init()
        {
            Console.WriteLine(GetPluginParameter("words").givenUserParameter);
            words = Utility.StringToStringArray(GetPluginParameter("words").givenUserParameter, false);
            Console.WriteLine(Utility.ArrayToString(words));
            positionInterpolation = new InterpolationGraph(GetPluginParameter("wordsInterpolation").givenUserParameter);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("words", new string[] {"--words", "-w"}, ""),
                new PluginParameter("wordsInterpolation", new string[] {"--wordsInterpolation", "-wI"}, ""),
            };
        }
    }
}
