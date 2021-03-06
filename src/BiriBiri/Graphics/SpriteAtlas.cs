﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace BiriBiri.Graphics
{
    [FileName("biriBiri.js")]
    public class SpriteAtlas
    {
        private readonly TexCoords[] _map;

        public SpriteAtlas(uint count)
        {
            _map = new TexCoords[count];
        }

        public TexCoords Get(uint index)
        {
            if (index >= _map.Length) throw new ArgumentException("Index is out of range", "index");
            return _map[index];
        }

        public static SpriteAtlas Uniform(uint width, uint height, uint columns, uint rows)
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

                atlas._map[i] = new TexCoords(tw * x, th * y, tw, th, pw, ph);
            }

            return atlas;
        }
    }
}
