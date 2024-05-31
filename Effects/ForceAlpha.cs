using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASCIIMusicVisualiser8.Utility.Maths;

namespace ASCIIMusicVisualiser8.Effects
{
    /// <summary>
    /// Forces a character to be rendered as transparent. Use '-c []' if you want the character to be a space.
    /// <b>Char</b>: The char to render as transparent. <i>(--char, -c)</i><br/>
    /// </summary>
    public class ForceAlpha : Effect
    {

        char transparentChar;

        public override void InitializeParameters()
        {
            pluginParameters = new List<PluginParameter>
            {
                new PluginParameter("char", new string[] {"--char", "-c"}, ""),
            };
        }

        public override void Init()
        {
            // 
            if (GetPluginParameter("char").givenUserParameter == "[]")
                transparentChar = ' ';
            else
                transparentChar = GetPluginParameter("char").givenUserParameter[0];
        }

        public override List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar)
        {
            newTransparentChar = transparentChar;
            return input;
        }
    }
}
