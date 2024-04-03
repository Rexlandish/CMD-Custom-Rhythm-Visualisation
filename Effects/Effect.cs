using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Effects
{
    public abstract class Effect
    {
        public virtual List<List<char>> ApplyTo(List<List<char>> input, char? transparentChar = null)
        {
            return input;
        }
    }
}
