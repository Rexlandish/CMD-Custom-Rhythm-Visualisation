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

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            throw new NotImplementedException();
        }

        public override void Init()
        {
            string[] allWords = Utility.StringToStringArray(GetPluginParameter("words").givenUserParameter);
            
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("words", new string[] {"--words", "-w"}, ""),

                // 10,0 10,10 0,10 0,0
                new PluginParameter("positions", new string[] {"--positions", "-p"}, "")

            };
        }
    }
}
