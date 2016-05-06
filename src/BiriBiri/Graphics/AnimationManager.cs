using System.Collections.Generic;
using Bridge;

namespace BiriBiri.Graphics
{
    [FileName("biriBiri.js")]
    public class AnimationManager
    {
        private object _map = new object();

        public void Add(string id, string textureId, Vector2 size, float fps, params uint[] frameIndices)
        {
            Add(id, textureId, size, fps, true, frameIndices);
        }

        public void Add(string id, string textureId, Vector2 size, float fps, bool looping, params uint[] frameIndices)
        {
            var timings = new float[frameIndices.Length];

            for (int i = 0; i < frameIndices.Length; i++)
            {
                timings[i] = fps / 60;
            }

            Script.Set(_map, id, new AnimationSequence(textureId, frameIndices, timings, looping));
        }

        public AnimationSequence Get(string id)
        {
            var sequence = Script.Get<AnimationSequence>(id);
            if (sequence == null) throw new KeyNotFoundException("Unknown animation sequence id: " + id);

            return sequence;
        }
    }
}
