using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public interface IManaged
    {
        void Setup();
        void Teardown();
    }

    public interface IUpdatable
    {
        void Update(double delta);
    }

    public interface IDrawable
    {
        void Draw();
    }
}
