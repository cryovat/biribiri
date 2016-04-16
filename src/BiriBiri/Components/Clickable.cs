using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Input
{
    public class Clickable : ComponentBase
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
