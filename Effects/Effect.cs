using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Effects
{
    public abstract class Effect : ParameterProcessor
    {
        public string parameterString { get; protected set; }
        public void SetParameterString(string input)
        {
            parameterString = input;
            InitializeParameters();
            ProcessParameterString(parameterString);
            Init();
        }

        public abstract void Init();
        public abstract List<List<char>> ApplyTo(List<List<char>> input, double beat, char transparentChar, out char newTransparentChar);
    }
}
