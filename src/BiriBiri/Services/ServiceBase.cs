using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Services
{
    public abstract class ServiceBase : IUpdatable, IDrawable
    {
        public Game Game;

        public virtual void Update(double delta)
        {
        }

        public virtual void Draw()
        {
        }
    }
}
