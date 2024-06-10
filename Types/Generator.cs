using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Policy;
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
        public List<Effect> effects = new List<Effect>() { };
        public List<Generator> subgenerators = new List<Generator>();

        public InterpolationGraph interpolationGraph;
        
        public BlendingMode blendingMode;

        public enum BlendingMode
        {
            InFront, // Default
            Behind,
            Multiply,
            Addition,
            Subtract,
            Without
        }


        public Generator(string generatorName, IPlugin plugin, string isActiveInterpolation = null, BlendingMode blendingMode = BlendingMode.InFront, int layer = 0, List<Effect> effects = null)
        {
            this.generatorName = generatorName;
            this.plugin = plugin;
            this.blendingMode = blendingMode;
            this.layer = layer;
            this.effects = effects ?? new List<Effect>();
            this.interpolationGraph = new InterpolationGraph(isActiveInterpolation);
        }

        public Generator(string generatorName, IPlugin plugin, InterpolationGraph isActiveInterpolation, BlendingMode blendingMode = BlendingMode.InFront, int layer = 0, List<Effect> effects = null)
        {
            this.generatorName = generatorName;
            this.plugin = plugin;
            this.blendingMode = blendingMode;
            this.layer = layer;
            this.effects = effects ?? new List<Effect>();
            this.interpolationGraph = isActiveInterpolation;
        }

        public GeneratorOutput GetOutput(double currentBeat)
        {
            // if the isActive interpolation graph is larger than 0.5, render the effect output.
            // Otherwise, return nothing.

            if (interpolationGraph.GetTime(currentBeat) >= 0.5)
            {
                List<List<char>> effectOutput = plugin.Generate(currentBeat, out char transparentChar);

                // Combine with the subgenerators
                
                for (int i = 0; i < subgenerators.Count; i++)
                {
                    Display.DrawLayer(effectOutput, subgenerators[i], currentBeat);
                }
                

                char newTransparentChar = transparentChar;

                Vector2 drawPoint = Vector2.Zero;


                foreach (Effect effect in effects)
                {
                    // Update the transparentChar and drawPoint each time
                    effectOutput = effect.ApplyTo(effectOutput, currentBeat, transparentChar, drawPoint, out newTransparentChar, out Vector2 newDrawPoint);
                    
                    drawPoint = newDrawPoint;
                    transparentChar = newTransparentChar;
                }

                return new GeneratorOutput(
                    effectOutput,
                    drawPoint, //! ------------------------ WHEN EFFECTS ARE ADDED, PUT THE EFFECT OUTPUT THROUGH THEM IN THIS FUNCTION AND PUT THE OUTPUT HERE
                    newTransparentChar
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

        public void SetPlugin(IPlugin plugin)
        {
            this.plugin = plugin;
        }

        public void AddSubGenerators(params Generator[] subgens)
        {
            foreach (var subgen in subgens)
            {
                subgenerators.Add(subgen);
            }
        }

        public void AddEffect(Effect effect)
        {
            effects.Add(effect);
        }


        public override string ToString()
        {
            return $"[{generatorName}] {plugin.pluginName}, Layer {layer}";
        }

    }
}
