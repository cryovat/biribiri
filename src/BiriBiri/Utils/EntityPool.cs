using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.Html5;

namespace BiriBiri.Utils
{
    [FileName("biriBiri.js")]
    public class EntityPool<TEntity> where TEntity : EntityBase, IClonable<TEntity>, new()
    {
        private object _dict = new object();

        [FieldProperty]
        public EntityPresetManager<TEntity> PresetManager { get; internal set; }

        public void Preload(string id, int count)
        {
            var arr = Script.Get<TEntity[]>(_dict, id);
            if (arr == null)
            {
                arr = new TEntity[0];
                Script.Set(_dict, id, arr);
            }

            while (arr.Length < count)
            {
                arr.Push(PresetManager.CreateInstance(id));
            }
        }

        public TEntity Get(string id)
        {
            var arr = Script.Get<TEntity[]>(_dict, id);
            if (arr == null)
            {
                arr = new TEntity[0];
                Script.Set(_dict, id, arr);
            }

            var ent = arr.Shift() as TEntity;
            if (ent == null)
            {
                ent = PresetManager.CreateInstance(id);
            }

            return ent;
        }

        public void Return(string id, TEntity entity)
        {
            var arr = Script.Get<TEntity[]>(_dict, id);
            if (arr == null)
            {
                arr = new TEntity[0];
                Script.Set(_dict, id, arr);
            }

            arr.Push(entity);
        }
    }
}
