using System;
using System.ComponentModel;
using Bridge;

namespace BiriBiri
{
    public class Positioned
    {
        public readonly Vector2 Position = Vector2.Zero;
    }

    public class Entity : Positioned, IManaged
    {
        private object _components = new object();
        private ComponentBase[] _all = new ComponentBase[0];
        private ComponentBase[] _visual = new ComponentBase[0];

        public readonly Vector2 Scale = Vector2.Zero;

        public double Rotation;
        public uint InteractionMask;

        public string EntityPoolKey;

        public Scene Scene;

        public Entity()
        {
        }

        public Entity Copy()
        {
            var copy = new Entity();
            copy.Position.X = Position.X;
            copy.Position.Y = Position.Y;

            for (int i = 0; i < _all.Length; i++)
            {
                var component = _all[i].Copy();
                Script.Set(copy._components, component.GetClassName(), component);

                copy._all.Push(component);

                if (component.Visual)
                {
                    copy._visual.Push(component);
                }
            }

            return copy;
        }

        public TComponent GetComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            var id = typeof (TComponent).GetClassName();
            var component = Script.Get<TComponent>(_components, id);
            if (component == null)
            {
                component = new TComponent();
                component.Owner = this;
                Script.Set(_components, id, component);

                _all.Push(component);

                if (component.Visual)
                {
                    _visual.Push(component);
                }
            }

            return component;
        }

        public void Update(double delta)
        {
            for (int i = 0; i < _all.Length; i++)
            {
                _all[i].Update(delta);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < _visual.Length; i++)
            {
                _visual[i].Draw();
            }
        }

        public void Setup()
        {
            for (int i = 0; i < _all.Length; i++)
            {
                _all[i].Setup();
            }
        }

        public void Teardown()
        {
            for (int i = 0; i < _all.Length; i++)
            {
                _all[i].Teardown();
            }
        }
    }
}
