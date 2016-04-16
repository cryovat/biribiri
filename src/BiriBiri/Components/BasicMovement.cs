using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Components
{
    public class BasicMovement : ComponentBase
    {
        private Velocity _velocity;

        public override void Setup()
        {
            _velocity = Owner.GetComponent<Velocity>();
        }

        public override void Update(double delta)
        {
            base.Update(delta);
        }

        public override ComponentBase Copy()
        {
            return new BasicMovement();
        }
    }
}
