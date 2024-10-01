using System;
using System.Collections.Generic;
using System.Numerics;
using static ASCIIMusicVisualiser8.Utility.Creation;
using static ASCIIMusicVisualiser8.Utility.Conversion;
using ManagedCuda;
using System.Windows.Media.Media3D;
using ManagedCuda.VectorTypes;

namespace ASCIIMusicVisualiser8.Plugins
{
    public class SwirlingTubes : Plugin, IPlugin
    {

        public override string pluginName {get => "Swirling Tubes"; }

        Vector2 size;

        /*
        public override List<PluginParameter> PluginParameters
        {

        }
        */

        public SwirlingTubes() { Initialize(); }
        public SwirlingTubes(string parameterString)
        {
            ProcessParameterStringPlugin(parameterString);
            Initialize();
        }

        CudaContext _context;
        CudaKernel _kernel;
        public void Initialize()
        {
            _context = new CudaContext();
            var module = _context.LoadModule(@"C:\Users\glass\Documents\Repos\ASCIIMusicVisualiserGithub\CMD-Custom-Rhythm-Visualisation\Plugins\kernel.ptx");
            _kernel = new("generateKernel", module, _context);
        }

        public override void InitializeParameters()
        {
            pluginParameters =
            new List<PluginParameter>()
            {
                new PluginParameter("size", new string[] {"--size", "-s"}, "")
            };
        }

        public override void Init()
        {

            size = Utility.Conversion.StringToVector2(GetPluginParameter("size").givenUserParameter, ',');

            //! Find a way to get size in from parameters given
            //size = new Vector2(200, 50);
            
        }

        public static double Sin01(double value)
        {
            return (Math.Sin(value) + 1)/2;
        }

        /*
        public static void GenerateKernel(GThread thread, int width, int height, double beat, float[,] finalFloatArray, double swirliness, double swirlDensity, double tubeSpacing, double swirlSpeed)
        {
            int i = thread.blockIdx.y * thread.blockIdx.y + thread.threadIdx.y;
            int j = thread.blockIdx.x * thread.blockIdx.x + thread.threadIdx.x;

            if (i < height && j < width)
            {

                double _i = i;
                double _j = j;

                // Scrolling
                _j += beat * 8;


                double wibbleAmount = Math.Cos(beat * Math.PI * 0.25);

                double asidenessSin = swirliness * Sin01(wibbleAmount * Math.Sin(Math.PI * ((swirlSpeed * beat + _i) / swirlDensity)));
                double asidenessCos = swirliness * Sin01(wibbleAmount * Math.Cos(Math.PI * ((swirlSpeed * beat + _i) / swirlDensity)));

                double currentDensity =
                    Math.Sin((_j / tubeSpacing) - asidenessSin) * (asidenessCos * 0.2);


                finalFloatArray[i, j] = (float)currentDensity;
            }
        }
        */

        public override List<List<OutputPixel>> Generate(double beat, out OutputPixel transparentChar)
        {
            //size = new Vector2(60, 5);
            var finalArray = Create2DArray(new OutputPixel(0), size);


            double opacity;

            double swirliness = 8; // Character span
            double swirlDensity = 20; // Character span
            double tubeSpacing = 5; // i.e. Tube size
            double swirlSpeed = 16; // Horizontal movement

            int width = (int)size.X;
            int height = (int)size.Y;


            CudaDeviceVariable<float> deviceResult = new CudaDeviceVariable<float>(width * height);


            // Set kernel parameters
            _kernel.BlockDimensions = new dim3(16, 16, 1);
            _kernel.GridDimensions = new dim3((width + 15) / 16, (height + 15) / 16, 1);

            _kernel.Run(deviceResult.DevicePointer, width, height, beat, swirliness, swirlDensity, tubeSpacing, swirlSpeed);


            float[] resultArray = new float[width * height];
            deviceResult.CopyToHost(resultArray);


            /*
            for (int i = 0; i < size.Y; i++)
            {
                for (int j = 0; j < size.X; j++)
                {

                    //swirlSpeed = 8 * Sin01(Math.Sin(beat)) + 8;
                    finalArray[i][j] = new(resultArray[i, j]);

                }
            }
            */

            
            for (int i = 0; i < height; i++)
            {
                //var row = new List<float>(width);
                for (int j = 0; j < width; j++)
                {
                    //row.Add(resultArray[i * width + j]);
                    //Console.WriteLine(resultArray[i * width + j]);
                    finalArray[i][j] = new(resultArray[i * width + j]);
                }
            }


            transparentChar = new(' ');
            return finalArray;
        }

        int CombineXY(int x, int y, int width)
        {
            return y * width + x;
        }

        // Splits a single int back into a Vector2
        Vector2 SplitXY(int combined, int width)
        {
            int x = combined % width;
            int y = combined / width;
            return new Vector2(x, y);
        }

        public override string ShowParameterValues(double time)
        {
            return "";
        }

    }
}
