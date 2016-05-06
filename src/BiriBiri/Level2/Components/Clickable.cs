using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public class Clickable : ComponentBase2D
    {
        public override void Update(double delta)
        {
            base.Update(delta);
        }

        public override ComponentBase Copy()
        {
            return new Clickable();
        }
    }
}
