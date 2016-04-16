using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Utils
{
    public class QuadTree<TItem> where TItem : Positioned
    {
        private List<TItem> _items = new List<TItem>();

        public void Add(TItem item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Remove(TItem item)
        {
            _items.Remove(item);
        }

        public void Apply(Action<TItem> action)
        {
            foreach (var item in _items.OrderBy(i => i.Position.Y).ThenBy(i => i.Position.X))
            {
                action(item);
            }
        }
    }
}
