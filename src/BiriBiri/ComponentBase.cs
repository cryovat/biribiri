using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri
{
    public abstract class ComponentBase : IManaged, IUpdatable
    {
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
