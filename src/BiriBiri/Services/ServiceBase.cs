using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Services
{
    [FileName("biriBiri.js")]
    public abstract class ServiceBase : IUpdatable, IDrawable
    {
        [FieldProperty]
        public Game Game { get; internal set; }

        public virtual void Update(double delta)
        {
        }

        public virtual void Draw()
        {
        }
    }
}
