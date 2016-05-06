using System;
using System.Diagnostics;
using Bridge;
using Bridge.Html5;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public class Velocity : ComponentBase2D
    {
        [FieldProperty]
        public Vector2 Current { get; }

        public Velocity()
        {
            Current = Vector2.Zero;
        }

        public override void Update(double delta)
        {
            Owner.Position.Add(Current, delta);

            Console.Log(Current);
        }

        public override ComponentBase Copy()
        {
            var clone = new Velocity();

            clone.Current.X = Current.X;
            clone.Current.Y = Current.Y;

            return clone;
        }
    }
}
