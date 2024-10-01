extern "C" __global__ void generateKernel(float* result, int width, int height, double beat, double swirliness, double swirlDensity, double tubeSpacing, double swirlSpeed) {
    double M_PI = 3.14159265358979;
    int j = blockIdx.x * blockDim.x + threadIdx.x;
    int i = blockIdx.y * blockDim.y + threadIdx.y;

    if (i < height && j < width) {
        double _i = (double)i;
        double _j = (double)j;

        // Scrolling
        _j += beat * 8;

        double wibbleAmount = cos(beat * M_PI);

        double asidenessSin = swirliness * sin(wibbleAmount * sin(M_PI * ((swirlSpeed * beat + _i) / swirlDensity)));
        double asidenessCos = swirliness * sin(wibbleAmount * cos(M_PI * ((swirlSpeed * beat + _i) / swirlDensity)));

        double currentDensity = cos((_j / tubeSpacing) - asidenessCos);// *(asidenessSin * 0.2);

        result[i * width + j] = (float)currentDensity;
    }
}


// nvcc -ptx C:\Users\glass\Documents\Repos\ASCIIMusicVisualiserGithub\CMD-Custom-Rhythm-Visualisation\Plugins\kernel.cu -o C:\Users\glass\Documents\Repos\ASCIIMusicVisualiserGithub\CMD-Custom-Rhythm-Visualisation\Plugins\kernel.ptx