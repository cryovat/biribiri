using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Input;
using Bridge;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public class BasicMovement : ComponentBase2D
    {
        private Velocity _velocity;

        [FieldProperty]
        public int Speed { get; set; }

        public override void Setup()
        {
            _velocity = Owner.GetComponent<Velocity>();

            Speed = 64;
        }

        public override void Update(double delta)
        {
            var kb = Game.Keyboard;
            var x = kb.IsDown(Key.ArrowLeft) ? -Speed : kb.IsDown(Key.ArrowRight) ? Speed : 0;
            var y = kb.IsDown(Key.ArrowUp) ? -Speed : kb.IsDown(Key.ArrowDown) ? Speed : 0;

            _velocity.Current.X = x;
            _velocity.Current.Y = y;
        }

        public override ComponentBase Copy()
        {
            return new BasicMovement();
        }
    }
}
