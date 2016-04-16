using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Utils
{
    public class EntityPresetManager
    {
        private readonly object _map = new object();

        public Entity CreateInstance(string id)
        {
            var preset = Script.Get<Entity>(id);

            return preset.Copy();
        }

        public void CreateTemplate(string id, Action<Entity> initializer)
        {
            var entity = new Entity();

            initializer(entity);

            Script.Set(_map, id, entity);
        }

        public void ExtendTemplate(string id, string parentId, Action<Entity> initializer)
        {
            var entity = CreateInstance(parentId);

            initializer(entity);

            Script.Set(_map, id, entity);
        }
    }
}
