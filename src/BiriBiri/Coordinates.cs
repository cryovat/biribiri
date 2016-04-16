using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public static class Coordinates
    {
        public static uint ToIndex(uint col, uint row, uint columnCount)
        {
            return row*columnCount + col;
        }

        public static uint IndexToColumn(uint index, uint columnCount)
        {
            return index % columnCount;
        }

        public static uint IndexToRow(uint index, uint columnCount)
        {
            return index / columnCount;
        }
    }
}
