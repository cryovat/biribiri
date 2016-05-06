using Bridge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BiriBiri.Utils
{
    [FileName("biriBiri.js")]
    public interface IQuadTreeItem
    {
        bool IsAt(Vector2 point);
        bool IsAt(Rectangle rect);
        bool IsContainedBy(Rectangle rect);
    }

    [FileName("biriBiri.js")]
    public class QuadTree<TItem> where TItem : class, IQuadTreeItem
    {
        private readonly QuadTreeBucket _root;

        public QuadTree()
        {
            _root = new QuadTreeBucket(new Rectangle
            {
                X = -500000,
                Y = -500000,
                Width = 1000000,
                Height = 1000000
            });
        }

        public bool Insert(TItem item)
        {
            return _root.Insert(item);
        }

        public void Clear(Action<TItem> cleanupCallback)
        {
            _root.Clear(cleanupCallback);
        }

        public bool Remove(TItem item)
        {
            return _root.Remove(item);
        }

        public void Query(Vector2 point, Action<TItem> matchHandler)
        {
            _root.Query(point, matchHandler);
        }

        public void Query(Rectangle rectangle, Action<TItem> matchHandler)
        {
            _root.Query(rectangle, matchHandler);
        }

        public void Traverse(Action<TItem> valueCallback)
        {
            _root.Traverse(valueCallback);
        }

        private class QuadTreeBucket
        {
            private const int DefaultCapacity = 10;

            QuadTreeBucket topLeft;
            QuadTreeBucket topRight;
            QuadTreeBucket bottomLeft;
            QuadTreeBucket bottomRight;

            private List<TItem> _entries = new List<TItem>(DefaultCapacity);

            [FieldProperty]
            public Rectangle Bounds { get; }

            public QuadTreeBucket(Rectangle bounds)
            {
                Bounds = bounds;
            }

            public bool Insert(TItem value)
            {
                if (!value.IsContainedBy(Bounds)) return false;

                if (_entries.Count < DefaultCapacity)
                {
                    _entries.Add(value);
                }

                if (topLeft == null)
                {
                    Subdivide();
                }

                if (topLeft.Insert(value)) return true;
                if (topRight.Insert(value)) return true;
                if (bottomLeft.Insert(value)) return true;
                if (bottomRight.Insert(value)) return true;

                _entries.Add(value);

                return false;
            }

            public bool Remove(TItem value)
            {
                if (!value.IsContainedBy(Bounds)) return false;

                if (_entries.Contains(value))
                {
                    _entries.Remove(value);
                    return true;
                }

                if (topLeft != null)
                {
                    if (topLeft.Remove(value)) return true;
                    if (topRight.Remove(value)) return true;
                    if (bottomLeft.Remove(value)) return true;
                    if (bottomRight.Remove(value)) return true;
                }

                return false;
            }

            public void Clear(Action<TItem> cleanupCallback)
            {
                Iterate(cleanupCallback);

                _entries.Clear();

                if (topLeft != null)
                {
                    topLeft.Clear(cleanupCallback);
                    topRight.Clear(cleanupCallback);
                    bottomLeft.Clear(cleanupCallback);
                    bottomRight.Clear(cleanupCallback);
                }

                topLeft = null;
                topRight = null;
                bottomLeft = null;
                bottomRight = null;
            }

            public void Query(Vector2 point, Action<TItem> matchHandler)
            {
                if (!Bounds.Contains(point)) return;

                Iterate(n =>
                {
                    if (n.IsAt(point)) matchHandler(n);
                });

                if (topLeft == null) return;

                topLeft.Query(point, matchHandler);
                topRight.Query(point, matchHandler);
                bottomLeft.Query(point, matchHandler);
                bottomRight.Query(point, matchHandler);
            }

            public void Query(Rectangle rect, Action<TItem> matchHandler)
            {
                if (!Bounds.Intersects(rect)) return;

                Iterate(n =>
                {
                    if (n.IsAt(rect)) matchHandler(n);
                });

                if (topLeft == null) return;

                topLeft.Query(rect, matchHandler);
                topRight.Query(rect, matchHandler);
                bottomLeft.Query(rect, matchHandler);
                bottomRight.Query(rect, matchHandler);
            }

            private void Subdivide()
            {
                var x = Bounds.X;
                var y = Bounds.Y;
                var hw = Bounds.Width / 2;
                var hh = Bounds.Height / 2;

                topLeft = new QuadTreeBucket(new Rectangle
                {
                    X = x,
                    Y = x,
                    Width = hw,
                    Height = hh
                });

                topRight = new QuadTreeBucket(new Rectangle
                {
                    X = x + hw,
                    Y = y,
                    Width = hw,
                    Height = hh
                });

                bottomLeft = new QuadTreeBucket(new Rectangle
                {
                    X = x,
                    Y = y + hh,
                    Width = hw,
                    Height = hh
                });

                bottomRight = new QuadTreeBucket(new Rectangle
                {
                    X = x + hw,
                    Y = y + hh,
                    Width = hw,
                    Height = hh
                });
            }

            public void Traverse(Action<TItem> valueCallback)
            {
                Iterate(valueCallback);

                if (topLeft == null) return;

                topLeft.Traverse(valueCallback);
                topRight.Traverse(valueCallback);
                bottomLeft.Traverse(valueCallback);
                bottomRight.Traverse(valueCallback);
            }

            private void Iterate(Action<TItem> valueCallback)
            {
                foreach (var entry in _entries)
                {
                    valueCallback(entry);
                }
            }
        }
    }
}
