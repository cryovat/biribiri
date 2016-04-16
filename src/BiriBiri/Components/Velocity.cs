using System;

namespace BiriBiri.Components
{
    public class Velocity : ComponentBase
    {
        public Vector2 Current = Vector2.Zero;

        public override void Update(double delta)
        {
            Owner.Position.Add(Current, delta);
        }

        public override ComponentBase Copy()
        {
            return new Velocity
            {
                Current = Current
            };
        }
    }
}
