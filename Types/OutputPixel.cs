namespace ASCIIMusicVisualiser8
{
    public class OutputPixel
    {
        float _brightness;

        // If you want this pixel's brightness but the pixel is meant to be a CHARACTER, then just return 1.
        public float brightness
        {
            get
            {
                return (type == OutputPixelType.CHARACTER ? 1 : _brightness);
            }
            private set
            {
                _brightness = value;
            }
        }

        public char character { get; private set; }

        public OutputPixelType? type { get; private set; }

        public enum OutputPixelType
        {
            VALUE,
            CHARACTER
        }

        public OutputPixel(float value)
        {
            this.brightness = value;
            type = OutputPixelType.VALUE;
        }

        public OutputPixel(char character)
        {
            this.character = character;
            type = OutputPixelType.CHARACTER;
        }

        public OutputPixel(OutputPixel op)
        {
            this.character = op.character;
            this.brightness = op.brightness;
            type = op.type;

        }

        public char GetOutput()
        {
            switch (type)
            {
                case OutputPixelType.VALUE:
                    return Utility.Conversion.GetCharFromBrightness(brightness);
                case OutputPixelType.CHARACTER:
                    return character;
                default:
                    throw new System.Exception($"No valid data in OutputPixel? VALUE: {brightness}, CHARACTER: {character}");
            }
        }

        public bool IsTransparent(OutputPixel transparentPixel)
        {
            switch (type)
            {
                case OutputPixelType.VALUE:
                    if (transparentPixel.brightness == this.brightness) return true;
                    break;

                case OutputPixelType.CHARACTER:
                    if (transparentPixel.character ==  this.character) return true;
                    break;

            }
            return false;
        }
    }
}