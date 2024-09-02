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

    public interface IPlugin
    {
        //List<PluginParameter> PluginParameters { get; }
        abstract List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentPixel);
        string pluginName { get; } // Name of the plugin
        Plugin @class { get; } // Reference to the class the interface is on
        void InitializeParameters(); // Converts string input to data
    }


    public abstract class Plugin : ParameterProcessor, IPlugin
    {

        
        public abstract string pluginName { get; }
        public Plugin @class => this;

        public override string ShowParameterValues(double time)
        {
            return "...";
        }

        public void ProcessParameterStringPlugin(string parameterString)
        {
            InitializeParameters();            
            // Parse 
            ProcessParameterString(parameterString);
            Init();
        }


        public abstract List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar);
        

        // Parse string input into parameter types
        public abstract void Init();
    }

}
