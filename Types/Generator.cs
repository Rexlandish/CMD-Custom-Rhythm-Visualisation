using ASCIIMusicVisualiser8.Effects;
using ASCIIMusicVisualiser8.Types;
using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Visualisation;
using System.Threading.Tasks;
using NAudio.SoundFont;
using static ASCIIMusicVisualiser8.Generator;

namespace ASCIIMusicVisualiser8
{

    public class GeneratorOutput
    {
        public List<List<OutputPixel>> outputPixels;
        public OutputPixel? transparentChar;
        public Vector2 position;
        public BlendingMode blendingMode;
        public bool isShader;

        public GeneratorOutput(List<List<OutputPixel>> outputPixels, Vector2 position, BlendingMode blendingMode, OutputPixel? transparentChar = null)
        {
            this.outputPixels = outputPixels;
            isShader = false;
            this.transparentChar = transparentChar;
            this.position = position;
            this.blendingMode = blendingMode;
        }


        public override string ToString()
        {
            return outputPixels.ToString();
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

        public float totalExecutionTime;
        public float generationTime;
        public float effectTime;
        public float subgenerationTime;

        public enum BlendingMode
        {
            InFront, // Default
            Behind,
            Multiply,
            Addition,
            Subtract,
            Without
        }

        public Generator Clone()
        {
            return (Generator)this.MemberwiseClone();
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

            Stopwatch sw = Stopwatch.StartNew();

            if (activeInterpolationGraph.GetTime(currentBeat) >= 0.5)
            {
                // Generate main plugin
                Stopwatch genSw = Stopwatch.StartNew();
                List<List<OutputPixel>> effectOutput = plugin.Generate(currentBeat, out OutputPixel transparentPixel);
                genSw.Stop();
                generationTime = genSw.ElapsedTicks;

                // Combine with the subgenerators
                Stopwatch subGenSw = Stopwatch.StartNew();

                foreach (var subgen in subgenerators)
                {
                    Display.DrawLayer(effectOutput, subgen, currentBeat);
                }

                /*
                List<GeneratorOutput> gOuts = new List<GeneratorOutput>();
                Parallel.ForEach(subgenerators, subgen =>
                {
                    var out_ = subgen.GetOutput(currentBeat);
                    
                    lock(gOuts)
                    {
                        gOuts.Add(out_);
                    }
                });
                
                foreach (var gOutput in gOuts)
                {
                    //Display.DrawLayer(effectOutput, subgen, currentBeat);
                    Display.DrawLayer(effectOutput, gOutput.position, gOutput.text, gOutput.blendingMode, gOutput.transparentChar);
                }
                */
                subGenSw.Stop();
                subgenerationTime = subGenSw.ElapsedTicks;



                Stopwatch effectSw = Stopwatch.StartNew();

                OutputPixel newTransparentChar = transparentPixel;
                Vector2 drawPoint = Vector2.Zero;

                // Combine with the effects
                foreach (Effect effect in effects)
                {
                    // Update the transparentChar and drawPoint each time
                    effectOutput = effect.ApplyTo(effectOutput, currentBeat, transparentPixel, drawPoint, out newTransparentChar, out Vector2 newDrawPoint);
                    
                    drawPoint = newDrawPoint;
                    transparentPixel = newTransparentChar;
                }

                effectSw.Stop();
                effectTime = effectSw.ElapsedTicks;

                sw.Stop();
                totalExecutionTime = sw.ElapsedTicks;
                
                return new GeneratorOutput(
                    effectOutput,
                    drawPoint, //! ------------------------ WHEN EFFECTS ARE ADDED, PUT THE EFFECT OUTPUT THROUGH THEM IN THIS FUNCTION AND PUT THE OUTPUT HERE
                    blendingMode,
                    newTransparentChar
                );

            }
            else
            {

                sw.Stop();
                totalExecutionTime = sw.ElapsedTicks;

                generationTime = 0;
                subgenerationTime = 0;
                effectTime = 0;

                return new GeneratorOutput(
                    new(),
                    Vector2.Zero,
                    BlendingMode.InFront
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

            string blendChar = blendingMode == BlendingMode.InFront ? "" : blendSymbolDictionary[blendingMode].ToString();

            //{name}
            string parameterValues = $"{plugin.@class.ShowParameterValues(time)}";

            string newName = $"{plugin.pluginName} (\"{name}\") {blendChar} {parameterValues ?? ""} [ Main: {generationTime}t \t| Effect: {effectTime}t \t| Subgen: {subgenerationTime}t \t| --Total: {totalExecutionTime}t-- ]";

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
