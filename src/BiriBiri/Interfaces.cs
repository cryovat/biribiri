using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace BiriBiri
{
    public interface IHitTestable<TOther>
    {
        CollisionResult HitTest(TOther other);
    }

    public interface IClonable<TClone>
    {
        TClone Clone();
    }

    public interface IPositioned2D
    {
        [FieldProperty]
        Vector2 Position { get; }
    }

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
