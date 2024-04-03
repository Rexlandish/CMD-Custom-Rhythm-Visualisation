using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8
{

    public class PluginParameter
    {
        public string parameterName; // Length
        public string[] parameterFlags; // -l, --length
        public string givenUserParameter; // 2.6

        public PluginParameter(string parameterName, string[] parameterFlags, string givenUserParameter = "")
        {
            this.parameterName = parameterName;
            this.parameterFlags = parameterFlags;
            this.givenUserParameter = givenUserParameter;
        }

        public void SetValue(string value)
        {
            givenUserParameter = value;
        }
    }

    interface IPlugin
    {
        //List<PluginParameter> PluginParameters { get; }
        abstract List<List<char>> Generate(double beat);
        string pluginName { get; } // Name of the plugin
        Plugin pluginAsClass { get; }
        void InitializeParameters();
    }


    public abstract class Plugin : IPlugin
    {

        protected List<PluginParameter> pluginParameters;
        public abstract string pluginName { get; }
        public Plugin pluginAsClass => this;

        public void ProcessParameterString(string parameterString)
        {

            InitializeParameters();
            
            // Check string for flags
            MatchCollection matches = Regex.Matches(parameterString, @"(--?\w+)\s+(?:[A-Za-z0-9_█#]+([\s█~$&,:;=?@#|'<>.^*()%!]*|(-\d)*)*)+(?!\S)");
            List<string> splitParameters = new();
            
            foreach (var param in pluginParameters)
            {
                Console.WriteLine($"{param.parameterName} {param.givenUserParameter}");
            }

            // Extract flags and load into plugin
            for (int i = 0; i < matches.Count; i++)
            {
                string subparameter = matches[i].Value;
                int placeToSplit = subparameter.IndexOf(" ");

                string flag = subparameter.Substring(0, placeToSplit);
                string obj = subparameter.Substring(placeToSplit + 1, subparameter.Length - placeToSplit - 1);


                var specifiedPluginParameter = pluginParameters.Find((PluginParameters) => PluginParameters.parameterFlags.Contains(flag));
                if (specifiedPluginParameter == null)
                {
                    throw new Exception($"No parameter with flag {flag} was found in Plugin {this}!");
                }

                specifiedPluginParameter.SetValue(obj);
                Console.WriteLine($"{specifiedPluginParameter.parameterName} {specifiedPluginParameter.givenUserParameter}");


                // !------------------ THIS ASSIGNS TO THE BASE CLASS??

            }
            Init();
        }

        protected PluginParameter GetPluginParameter(string parameterName)
        {
            return pluginParameters.Find((param) => param.parameterName == parameterName);
        }

        public abstract void Init();
        public abstract List<List<char>> Generate(double beat);
        public abstract void InitializeParameters();
    }

}
