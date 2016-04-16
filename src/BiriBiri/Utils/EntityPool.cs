using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;

namespace BiriBiri.Utils
{
    public class EntityPool
    {
        public EntityPresetManager PresetManager;

        public void Preload(string name, int count)
        {
        }

        public Entity Get(string name)
        {
            throw new NotImplementedException();
        }

        public void Return(string name, Entity entity)
        {

        }
    }
}
