using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public class Camera : IUpdatable
    {
        private readonly Vector3 _helper = new Vector3();

        public readonly Matrix Matrix = Matrix.CreateIdentity();

        public uint Width;
        public uint Height;

        public double Zoom;

        public void ConvertWorldCoordinate(Vector2 coordiate, Vector2 destination)
        {
            Matrix m;
        }

        public void Update(double delta)
        {
        }

        public void SetPosition(double x, double y)
        {
            _helper.X = x;
            _helper.Y = y;
            _helper.Z = 0;

            Matrix.Translation = _helper;
        }

        public void SetScale(double x, double y)
        {
            _helper.X = x;
            _helper.Y = y;
            _helper.Z = 0;

            Matrix.Scale = _helper;
        }
    }
}
