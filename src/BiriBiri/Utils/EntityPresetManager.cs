using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Services;

namespace BiriBiri.Utils
{
    [FileName("biriBiri.js")]
    public class EntityPresetManager<TEntity> where TEntity : EntityBase, IClonable<TEntity>, new()
    {
        private readonly object _map = new object();

        internal EntityPresetManager()
        {
        }

        public TEntity CreateInstance(string id)
        {
            var preset = Script.Get<TEntity>(_map, id);
            if (preset == null) throw new ArgumentException("Unknown entity preset: " + id, "id");

            return preset.Clone();
        }

        public void CreateTemplate(string id, Action<TEntity> initializer)
        {
            var entity = new TEntity();

            initializer(entity);

            Script.Set(_map, id, entity);
        }

        public void ExtendTemplate(string id, string parentId, Action<TEntity> initializer)
        {
            var entity = CreateInstance(parentId);

            initializer(entity);

            Script.Set(_map, id, entity);
        }
    }
}
