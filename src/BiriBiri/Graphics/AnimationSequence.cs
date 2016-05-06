using Bridge;

namespace BiriBiri.Graphics
{
    [FileName("biriBiri.js")]
    public class AnimationSequence
    {
        [FieldProperty]
        public string TextureId { get; private set; }
        [FieldProperty]
        public uint[] FrameIndices { get; private set; }
        [FieldProperty]
        public float[] Intervals { get; private set; }
        [FieldProperty]
        public bool Loop { get; private set; }

        public AnimationSequence(string textureId, uint[] frameIndices, float[] intervals, bool loop)
        {
            TextureId = textureId;
            FrameIndices = frameIndices;
            Intervals = intervals;
            Loop = loop;
        }
    }
}
