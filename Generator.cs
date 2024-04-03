using ASCIIMusicVisualiser8.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8
{

    internal class GeneratorOutput
    {
        public List<List<char>> text;
        public char? transparentChar;
        public Vector2 position;

        public GeneratorOutput(List<List<char>> text, Vector2 position, char? transparentChar = null)
        {
            this.text = text;
            this.transparentChar = transparentChar;
            this.position = position;
        }

        public override string ToString()
        {
            return text.ToString();
        }
    }

    internal class Generator
    {

        string generatorName;

        public int layer = 0;

        public IPlugin plugin;
        public List<Effect> effects = new List<Effect>();

        public Generator(string generatorName, IPlugin plugin, int layer, List<Effect> effects = null)
        {
            this.generatorName = generatorName;
            this.plugin = plugin;
            this.layer = layer;
            this.effects = effects; 
        }

        public GeneratorOutput GetOutput(double currentBeat)
        {

            return new GeneratorOutput(
                plugin.Generate(currentBeat),
                Vector2.Zero //! ------------------------ ADD A VECTOR GENERATOR OR EFFECT OR ANYTHING HERE
            );

        }

        public override string ToString()
        {
            return $"[{generatorName}] {plugin.pluginName}, Layer {layer}";
        }

    }
}
