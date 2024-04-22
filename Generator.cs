using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        public InterpolationGraph interpolationGraph;

        public Generator(string generatorName, IPlugin plugin, string isActiveInterpolation = null, int layer = 0, List<Effect> effects = null)
        {
            this.generatorName = generatorName;
            this.plugin = plugin;
            this.layer = layer;
            this.effects = effects;
            this.interpolationGraph = new InterpolationGraph(isActiveInterpolation);
        }

        public Generator(string generatorName, IPlugin plugin, InterpolationGraph isActiveInterpolation, int layer = 0, List<Effect> effects = null)
        {
            this.generatorName = generatorName;
            this.plugin = plugin;
            this.layer = layer;
            this.effects = effects;
            this.interpolationGraph = isActiveInterpolation;
        }

        public GeneratorOutput GetOutput(double currentBeat)
        {
            // if the isActive interpolation graph is larger than 0.5, render the effect output.
            // Otherwise, return nothing.

            if (interpolationGraph.GetTime(currentBeat) >= 0.5)
            {
                List<List<char>> effectOutput = plugin.Generate(currentBeat, out char transparentChar);

                return new GeneratorOutput(
                    effectOutput,
                    Vector2.Zero, //! ------------------------ WHEN EFFECTS ARE ADDED, PUT THE EFFECT OUTPUT THROUGH THEM IN THIS FUNCTION AND PUT THE OUTPUT HERE
                    transparentChar
                );

            }
            else
            {
                return new GeneratorOutput(
                    new(),
                    Vector2.Zero
                );
            }


            

        }

        public override string ToString()
        {
            return $"[{generatorName}] {plugin.pluginName}, Layer {layer}";
        }

    }
}
