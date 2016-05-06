using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace BiriBiri
{
    public class Camera
    {
        private Vector2 _position;
        private Vector2 _scale;

        public readonly Matrix Matrix = Matrix.CreateIdentity();

        [FieldProperty]
        public uint Width { get; private set; }
        [FieldProperty]
        public uint Height { get; private set; }

        public Camera(uint width, uint height)
        {
            _position = new Vector2(0, 0);
            _scale = new Vector2(1, 1);

            Width = width;
            Height = height;

            Matrix.InitOrthographicOffCenter(0, width, height, 0, 0, -1);
        }

        public void ConvertWorldCoordinate(Vector2 coordiate, Vector2 destination)
        {
            Matrix m;
        }

        public void CopyBoundsTo(Rectangle rectangle)
        {
            var sw = Width*_scale.X;
            var sh = Height*_scale.Y;

            rectangle.X = -sw /2;
            rectangle.Y = -sh /2;
            rectangle.Width = sw;
            rectangle.Height = sh;
        }

        public void CenterAt(double x, double y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public void CenterAt(Vector2 position)
        {
            _position.X = position.X;
            _position.Y = position.Y;
        }

        public void ScaleAt(Vector2 scale)
        {
            _scale.X = scale.X;
            _scale.Y = scale.Y;
        }

        private void UpdateMatrix()
        {
            var shw = (Width / 2) * _scale.X;
            var shh = (Height / 2) * _scale.Y;

            var left = _position.X - shw;
            var top = _position.Y - shh;

            var right = _position.X + shw;
            var bottom = _position.Y + shh;

            Matrix.InitOrthographicOffCenter((float)left, (float)right, (float)bottom, (float)top, 0, -1);
        }
    }
}
