using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public abstract class ComponentBase : IManaged, IUpdatable, IDrawable
    {
        public bool Visual;
        public Entity Owner;

        public Scene Scene
        {
            get { return Owner.Scene; }
        }

        public Game Game
        {
            get { return Scene.Game; }
        }

        public virtual void Draw()
        {
        }

        public virtual void Setup()
        {
        }

        public virtual void Teardown()
        {
        }

        public virtual void Update(double delta)
        {
        }

        public abstract ComponentBase Copy();
    }
}
