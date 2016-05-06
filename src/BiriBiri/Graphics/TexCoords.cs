using Bridge;

namespace BiriBiri.Graphics
{
    [FileName("biriBiri.js"), Immutable]
    public struct TexCoords
    {
        public readonly float MinX;
        public readonly float MaxX;
        public readonly float MinY;
        public readonly float MaxY;

        public readonly uint PixelWidth;
        public readonly uint PixelHeight;

        public TexCoords(double x, double y, double width, double height, uint pixelWidth, uint pixelHeight)
        {
            MinX = (float)x;
            MaxX = (float)(x + width);

            MinY = (float)y;
            MaxY = (float)(y + height);

            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
        }
    }
}
