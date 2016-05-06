using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.WebGL;

namespace BiriBiri.Graphics
{
    [FileName("biriBiri.js")]
    public class Texture
    {
        [FieldProperty]
        public WebGLTexture WebGLTexture { get; private set; }
        [FieldProperty]
        public TexCoords[] Frames { get; private set; }

        public Texture(WebGLTexture texture, uint width, uint height)
        {
            WebGLTexture = texture;

            Frames = new[] {
                new TexCoords(0, 0, 1, 1, width, height)
            };
        }

        public void SliceUniformly(uint width, uint height, uint columns, uint rows)
        {
            var count = columns * rows;
            var atlas = new SpriteAtlas(count);

            var dw = (double)width;
            var dh = (double)height;

            var pw = width / columns;
            var ph = width / height;

            var tw = dw / columns;
            var th = dh / rows;

            for (uint i = 0; i < count; i++)
            {
                var x = Coordinates.IndexToColumn(i, columns);
                var y = Coordinates.IndexToRow(i, columns);

                Frames.Push(new TexCoords(tw * x, th * y, tw, th, pw, ph));
            }
        }
    }
}
