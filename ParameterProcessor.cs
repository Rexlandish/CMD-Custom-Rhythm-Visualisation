﻿using System;
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

        protected List<PluginParameter> pluginParameters;

        protected PluginParameter GetPluginParameter(string parameterName)
        {
            return pluginParameters.Find((param) => param.parameterName == parameterName);
        }

        protected void ProcessParameterString(string parameterString)
        {
            // Check string for flags
            MatchCollection matches = Regex.Matches(parameterString, @"(--?\w+)\s+(?:([A-Za-z0-9_█#.\s█~$&,:;=?@#|'<>\[\].^*()%!]*|(-\d)*)*)+(?!\S)");
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
        }
    }
}
