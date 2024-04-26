using ASCIIMusicVisualiser8.Types.Interpolation.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Repeat;
using static ASCIIMusicVisualiser8.Utility.Conversion;

namespace ASCIIMusicVisualiser8.Plugins
{
    internal class TextDisplay : Plugin, IPlugin
    {


        public override string pluginName => "TextDisplay";

        // A list of phrases, made up of a list of sentences split by line breaks
        List<List<string>> phraseFrames = new();
        Vector2[] positions;
        InterpolationGraph positionInterpolation;

        public override List<List<char>> Generate(double beat, out char transparentChar)
        {
            // Get current interpolation from beat
            int index = (int)Math.Floor(positionInterpolation.GetTime(beat));


            // Render all text for now
            List<List<char>> wordsToRender = new();
            List<string> phrase = phraseFrames[index];

            // This function gets the longest item in the list and returns it's length
            int longestPhraseLength = phrase.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

            foreach (string word in phrase)
            {
                int paddingAmount = longestPhraseLength - word.Length;

                List<char> wordToCharList = new(word.ToArray());
                wordToCharList.AddRange(RepeatNTimesToList(' ', paddingAmount));
                wordsToRender.Add(wordToCharList);
            }

            // Debug, show index of current phrase
            //wordsToRender.Add(new (index.ToString().ToCharArray()));
            transparentChar = new char();
            //transparentChar = '*';

            return wordsToRender;
            
            
        }

        public override void Init()
        {
            // "abc,abc\ndef,abc\ndef\nghi"
            string[] wordsList = StringToStringArray(GetPluginParameter("words").givenUserParameter, false, '_');
            
            foreach (string phrase in wordsList)
            {
                Console.WriteLine($"Initializing with {string.Join(",",phrase.Split('\n'))}");
                phraseFrames.Add(new(phrase.Split('\n')));
            }

            positionInterpolation = new InterpolationGraph(GetPluginParameter("wordsInterpolation").givenUserParameter);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("words", new string[] {"--words", "-w"}, ""),
                new PluginParameter("wordsInterpolation", new string[] {"--wordsInterpolation", "-wI"}, ""),
            };
        }
    }
}
