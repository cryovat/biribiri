using System;
using BiriBiri.Utils;
using Bridge;

namespace BiriBiri.Level2
{
    [FileName("biriBiri.level2.js")]
    public sealed class Entity2D : EntityBase, IManaged, IPositioned2D, IClonable<Entity2D>
    {
        [FieldProperty]
        public Vector2 Position { get; private set; }
        [FieldProperty]
        public Vector2 Scale { get; private set; }
        [FieldProperty]
        public double Rotation { get; set; }

        [FieldProperty]
        internal string EntityPoolKey { get; set; }

        [FieldProperty]
        public Scene2D Scene { get; internal set; }

        public Entity2D()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
        }

        public TComponent GetComponent<TComponent>() where TComponent : ComponentBase2D, new()
        {
            var component = ResolveComponent<TComponent>();
            component.Owner = this;
            return component;
        }

        public Entity2D Clone()
        {
            var clone = new Entity2D();

            clone.Position.X = Position.X;
            clone.Position.Y = Position.Y;

            clone.Scale.X = Scale.X;
            clone.Scale.Y = Scale.Y;

            clone.Rotation = Rotation;

            CopyComponentsTo(clone, c => c.As<ComponentBase2D>().Owner = clone);

            return clone;
        }
    }
}
