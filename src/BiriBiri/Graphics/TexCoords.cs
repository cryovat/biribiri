using Bridge;

namespace BiriBiri.Graphics
{
    [Immutable]
    public struct TexCoords
    {
        public readonly float MinX;
        public readonly float MaxX;
        public readonly float MinY;
        public readonly float MaxY;

        public TexCoords(double x, double y, double width, double height)
        {
            MinX = (float)x;
            MaxX = (float)(x + width);

            MinY = (float)y;
            MaxY = (float)(y + height);
        }
    }
}
