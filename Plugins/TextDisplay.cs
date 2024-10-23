using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8.Plugins
{
    /// <summary>
    /// <b>Words</b>: Underscore separated text to display. <i>(--words, -w)</i><br/>
    /// <b>Words Interpolation</b>: What index of the parsed text list to display. <i>(--wordsInterpolation, -wI)</i><br/>
    /// <b>Delimiter</b>: What character separates words to display. <i>(--delimiter, -d)</i><br/>
    /// </summary>
    internal class TextDisplay : Plugin, IPlugin
    {


        public override string pluginName => "TextDisplay";

        // A list of phrases, made up of a list of sentences split by line breaks
        List<List<string>> phraseFrames = new();
        Vector2[] positions;
        InterpolationGraph positionInterpolation;
        char delimiter;
        int phraseFrameCount;

        public TextDisplay() { }
        public TextDisplay(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
        }

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            // Get current interpolation from beat
            int index = (int)Math.Floor(positionInterpolation.GetTime(beat));

            // Limit phrases between 
            if (index <= 0)
            {
                index = 0;
            }
            if (index > phraseFrameCount)
            {
                while (index > phraseFrameCount)
                {
                    index -= phraseFrameCount;
                }
            }

            // Render all text for now
            List<List<OutputPixel>> wordsToRender = new();
            List<string> phrase = phraseFrames[index];

            // This function gets the longest item in the list and returns it's length
            int longestPhraseLength = phrase.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

            foreach (string word in phrase)
            {
                int paddingAmount = longestPhraseLength - word.Length;

                List<char> wordToCharList = new(word.ToArray());
                List<OutputPixel> wordToOutputPixel = new();
                wordToCharList.AddRange(RepeatNTimesToList(' ', paddingAmount));
                
                wordToCharList.ForEach(c => wordToOutputPixel.Add(new OutputPixel(c)));

                wordsToRender.Add(wordToOutputPixel);
            }

            // Debug, show index of current phrase
            //wordsToRender.Add(new (index.ToString().ToCharArray()));
            //transparentChar = new char();
            transparentChar = new(' ');
            //transparentChar = '*';

            return wordsToRender;
            
            
        }

        public override void Init()
        {
            // "abc,abc\ndef,abc\ndef\nghi"

            delimiter = GetPluginParameter("delimiter").givenUserParameter[0];

            string[] wordsList = StringToStringArray(GetPluginParameter("words").givenUserParameter, false, delimiter);

                
            foreach (string phrase in wordsList)
            {
                //{string.Join(",",phrase.Split('\n'))}
                //Console.WriteLine($"Reading {phrase}");
                phraseFrames.Add(new(phrase.Split('\n')));
            }

            phraseFrameCount = phraseFrames.Count;

            positionInterpolation = new InterpolationGraph(GetPluginParameter("wordsInterpolation").givenUserParameter);

        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("words", new string[] {"--words", "-w"}, ""),
                new PluginParameter("wordsInterpolation", new string[] {"--wordsInterpolation", "-wI"}, ""),
                new PluginParameter("delimiter", new string[] {"--delimiter", "-d"}, ""),
            };
        }

        public override string ShowParameterValues(double time)
        {
            int index = (int)Math.Floor(positionInterpolation.GetTime(time));
            /*
            Console.WriteLine($"{index} {phraseFrames.Count}");
            string textToShow = string.Join("", phraseFrames[index]);
            
            if (textToShow.Length <= 10)
                return $"{textToShow}";
            
            return $"-OVERFLOW-";
            */

            return $"Frame {index}";
        }
    }
}
