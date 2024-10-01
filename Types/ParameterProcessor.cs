using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASCIIMusicVisualiser8
{
    public class ParameterProcessor
    {

        // Create the parameters at runtime
        public virtual void InitializeParameters() { }

        public List<PluginParameter> pluginParameters { get; protected set;}

        protected PluginParameter GetPluginParameter(string parameterName)
        {
            var res = pluginParameters.Find((param) => param.parameterName == parameterName);
            return res;
        }

        public virtual string ShowParameterValues(double time)
        {
            return "...";
        }

        // Handle flags that are not included!!!
        protected void ProcessParameterString(string parameterString)
        {

            // Check string for flags
            MatchCollection matches = Regex.Matches(parameterString, @"(--?\w+)\s+(?:((\S+\s*(?!--?\w+))*|(-\d)|()*)*)+(?!-\w)"); // Thank goodness for https://www.debuggex.com
            List<string> splitParameters = new();

            foreach (var param in pluginParameters)
            {
                //Console.WriteLine($"{param.parameterName} {param.givenUserParameter}");
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
                //Console.WriteLine($"{specifiedPluginParameter.parameterName} {specifiedPluginParameter.givenUserParameter}");


                // !------------------ THIS ASSIGNS TO THE BASE CLASS??

            }
        }


    }
}
