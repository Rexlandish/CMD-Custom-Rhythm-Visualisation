using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8.Plugins
{
    internal class FontParser
    {
        public static void GetData()
        {
            // Get all character data
            string f = File.ReadAllText("./fonts/spleen-6x12.bdf");
            Regex r = new Regex(@"STARTCHAR[\s\S]*?ENDCHAR");
            MatchCollection matches = r.Matches(f);

            Regex searchString;
            Dictionary<string, int[]> finalDataDict = new Dictionary<string, int[]>();
            // Get each data
            foreach (Match match in matches)
            {
                //Console.WriteLine(match.Value);
                //Console.ReadLine();

                // Get name
                searchString = new Regex(@"(?<=STARTCHAR\s)[A-Z ]+(?=\n)"); // Between STARTCHAR and \n
                Match name = searchString.Match(match.Value);
                Console.WriteLine(name.Value);



                // Get data from bitmap
                searchString = new Regex(@"(?<=BITMAP\s)[0-9A-Fa-f\s]+(?=\s*ENDCHAR)"); // Between BITMAP and ENDCHAR
                Match hexValues = searchString.Match(match.Value);
                string[] hexValuesArray = hexValues.Value.Trim().Split('\n');
                int[] decimalValuesArray = new int[hexValuesArray.Length]; // Convert hex values to decimal for easier encoding
                // Remove the emptylines and split by linebreaks
                for (int i = 0; i < hexValuesArray.Length; i++)
                {
                    //Console.WriteLine("0x" + hexValue);
                    var hexValue = hexValuesArray[i];
                    // Convert hex to decimal
                    int decimalValue = Convert.ToInt32("0x" + hexValue, 16); // Hexstring with 0x added

                    /*
                    string binaryString = Convert.ToString(decimalValue, 2).PadLeft(8, '0');
                    foreach (char c in binaryString)
                    {
                        //Console.Write(c == '1' ? "#" : c == '0' ? " " : ".");
                    }
                    //Console.WriteLine();
                    */

                    decimalValuesArray[i] = decimalValue;
                }

                Console.WriteLine(ParseValues(decimalValuesArray));

                finalDataDict[name.Value] = decimalValuesArray;

            }

            string outputData = Newtonsoft.Json.JsonConvert.SerializeObject(finalDataDict);
            Console.WriteLine(outputData);

            File.WriteAllText("./fonts/output.json", outputData);

            Console.ReadLine();

        }

        public static string ParseValues(int[] decimalValues)
        {
            // Prepare output
            string[] outputRows = new string[decimalValues.Length];

            // Iterate through the values and convert them into binary strings
            for (int i = 0; i < decimalValues.Length; i++)
            {

                var value = decimalValues[i];

                string binaryString = Convert.ToString(value, 2).PadLeft(8, '0');

                string currentRow = "";
                for (int j = 0; j < binaryString.Length; j++)
                {
                    char c = binaryString[j];
                    currentRow +=
                        c == '1' ? "#" :
                        c == '0' ? " " :
                        " ";
                }

                outputRows[i] = currentRow;

            }
            return string.Join("\n", outputRows);
        }

        // Get the dictionary value from the parsed bdf file
        public string GetCharacter(string characterName)
        {
            if (fontJSON == null)
            {
                throw new Exception("Null dictionary, please choose font to load from");
            }
            if (fontJSON.ContainsKey(characterName))
                return ParseValues(fontJSON[characterName]);
            else
                return ParseValues(fontJSON["SPACE"]);
        }

        Dictionary<string, int[]> fontJSON;
        public void LoadFontJSON(string path)
        {
            string fileText = File.ReadAllText(path);
            fontJSON = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int[]>>(fileText);
        }

        public string GenerateText(string s)
        {
            List<string[]> characters = new List<string[]>();
            for (int i = 0; i < s.Length; i++)
            {
                // Split the characters by row so it's easier to process
                characters.Add(GetCharacter("LATIN CAPITAL LETTER " + s[i]).Split('\n'));
            }

            // Iterate through rows
            int rowCount = characters[0].Length; // Check how many times to scan across each character
            List<string> combinedRows = new List<string>(); // Combined rows of each character
            for (int i = 0; i < rowCount; i++)
            {
                
                // And iterate through characters
                string currentRow = "";
                foreach (string[] character in characters)
                {
                    currentRow += character[i];
                }

                combinedRows.Add(currentRow);

            }

            return string.Join("\n", combinedRows);

        }
    }
}
