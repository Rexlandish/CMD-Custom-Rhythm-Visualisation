using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class Dotsies
    {
        static Dictionary<char, int> dotsiesDict = new Dictionary<char, int>()
        {
            {'a', 1},
            {'b', 2},
            {'c', 4},
            {'d', 8},
            {'e', 16},
            {'f', 3},
            {'g', 6},
            {'h', 12},
            {'i', 24},
            {'j', 5},
            {'k', 10},
            {'l', 20},
            {'m', 9},
            {'n', 18},
            {'o', 17},
            {'p', 7},   // ###---
            {'q', 11},  // ##-#--
            {'r', 13},  // #-##--
            {'s', 14},  // -###--
            {'t', 22},  // -##-#-
            {'u', 36},  // -#-##-
            {'v', 28},  // --###
            {'w', 19},  // ##--#
            {'x', 21},  // #-#-#
            {'y', 25},  // #--##
            {'z', 27}   // ##-##
        };


        public static string GetDotsies(string word, char blank = ' ', char filled = '█')
        {

            int targetWidth = 5;

            List<string> finalString = new();

            foreach (char c in word)
            {
                char lowerC = Char.ToLower(c);
                string dotsiesLetter;

                if (dotsiesDict.ContainsKey(lowerC))
                {
                    int letterArray = dotsiesDict[lowerC];
                    dotsiesLetter = Convert.ToString(letterArray, 2);
                    dotsiesLetter = Pad(dotsiesLetter, targetWidth);                    
                }
                else
                {
                    dotsiesLetter = Pad("", targetWidth);
                }

                dotsiesLetter = dotsiesLetter.Replace('0', blank);
                dotsiesLetter = dotsiesLetter.Replace('1', filled);

                finalString.Add(dotsiesLetter);

            }


            // Sample the grid 90 degrees anti clockwise
            List<string> finalRotatedString = new();
            for (int i = targetWidth - 1; i >= 0; i--)
            {
                string rowString = "";
                foreach (var row in finalString)
                {
                    rowString += row[i];
                }
                finalRotatedString.Add(rowString);
            }

            return string.Join("\n", finalRotatedString);
        }
        public static string Pad(string s, int paddingAmount)
        {
            return Utility.Repeat.RepeatChar('0', paddingAmount - s.Length) + s;
        }

    }

}
