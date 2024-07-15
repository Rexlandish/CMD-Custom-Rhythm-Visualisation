using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility;
using static ASCIIMusicVisualiser8.Utility.Visualisation;

namespace ASCIIMusicVisualiser8
{

    public class GeneratorOutput
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

    public class Generator : IHierarchy
    {

        public string name { get; set; }

        public int layer = 0;

        public IPlugin plugin;
        public List<Effect> effects = new List<Effect>() { };
        public List<Generator> subgenerators = new List<Generator>();

        public InterpolationGraph activeInterpolationGraph;
        
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
            this.name = generatorName;
            this.plugin = plugin;
            this.blendingMode = blendingMode;
            this.layer = layer;
            this.effects = effects ?? new List<Effect>();
            this.activeInterpolationGraph = new InterpolationGraph(isActiveInterpolation);
        }

        public Generator(string generatorName, IPlugin plugin, InterpolationGraph isActiveInterpolation, BlendingMode blendingMode = BlendingMode.InFront, int layer = 0, List<Effect> effects = null)
        {
            this.name = generatorName;
            this.plugin = plugin;
            this.blendingMode = blendingMode;
            this.layer = layer;
            this.effects = effects ?? new List<Effect>();
            this.activeInterpolationGraph = isActiveInterpolation;
        }

        public GeneratorOutput GetOutput(double currentBeat)
        {
            // if the isActive interpolation graph is larger than 0.5, render the effect output.
            // Otherwise, return nothing.

            if (activeInterpolationGraph.GetTime(currentBeat) >= 0.5)
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

        public void AddEffects(params Effect[] effectList)
        {
            foreach (Effect e in effectList)
                effects.Add(e);
        }


        public override string ToString()
        {
            return $"[{name}] {plugin.pluginName}, Layer {layer}";
        }

        //https://stackoverflow.com/questions/1649027/how-do-i-print-out-a-tree-structure
        public void HandleNext(string indent, bool last, double time, bool isParentActive)
        {

            bool hasChildren = subgenerators.Count > 0 | effects.Count > 0;

            //{name}
            string newName = $"{plugin.pluginName} ({name}) {blendSymbolDictionary[blendingMode]} [{plugin.@class.ShowParameterValues(time)}]";

            // Show this generator as active only if it's parent is active too
            bool isGeneratorActive = activeInterpolationGraph.GetTime(time) >= 0.5 && isParentActive;
            ConsoleColor isActiveColor = isGeneratorActive ? ConsoleColor.Red : ConsoleColor.DarkGray;
            newName = isGeneratorActive ?  newName: newName;

            PrintHierarchy(newName, indent, last, hasChildren, false, isParentActive, out string newIndent, isActiveColor);

            for (int i = 0; i < effects.Count; i++)
                //effects[i].HandleNext(newIndent, i == effects.Count - 1);
                effects[i].HandleNext(newIndent, i == effects.Count - 1 && subgenerators.Count == 0, time, isGeneratorActive);
            // Make this node an end node if there aren't any subgenerators to come after

            
            for (int i = 0; i < subgenerators.Count; i++)
                subgenerators[i].HandleNext(newIndent, i == subgenerators.Count - 1, time, isGeneratorActive);


        }


    }
}
