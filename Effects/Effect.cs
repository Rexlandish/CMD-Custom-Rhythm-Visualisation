using ASCIIMusicVisualiser8.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ASCIIMusicVisualiser8.Utility.Visualisation;

namespace ASCIIMusicVisualiser8.Effects
{
    public abstract class Effect : ParameterProcessor, IHierarchy
    {
        public string name {get; set;}

        public Effect()
        {
        }

        public Effect(string parameterString)
        {
            SetParameterString(parameterString);
        }

        public string parameterString { get; protected set; }
        public void SetParameterString(string input)
        {
            parameterString = input;
            InitializeParameters();
            ProcessParameterString(parameterString);
            Init();
        }

        // Can this be linked to an inherited class?

        public abstract void Init();
        public abstract List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, Vector2 drawPoint, out char newTransparentChar, out Vector2 newDrawPoint);

        public void HandleNext(string indent, bool last, double time, bool isActive)
        {

            string newName = $"{name} [{ShowParameterValues(time)}]";
            ConsoleColor color;

            color = isActive ? ConsoleColor.Cyan : ConsoleColor.DarkGray;


            PrintHierarchy(newName, indent, last, false, false, isActive, out string newIndent, color);




            // Nothing else to print as a child
            /*
            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintPretty(indent, i == Children.Count - 1);
            */
        }

    }
}
