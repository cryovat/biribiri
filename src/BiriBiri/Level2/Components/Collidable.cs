using Bridge;
using System;
using BiriBiri.Utils;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public enum CollisionType
    {
        None,
        Circle,
        Square
    }

    [FileName("biriBiri.level2.js")]
    public class Collidable : ComponentBase2D, IHitTestable<Collidable>, IQuadTreeItem
    {
        private readonly Rectangle _boundingRectangle = new Rectangle();

        [FieldProperty]
        public CollisionType CollisionType { get; set; }

        [FieldProperty]
        public float CollisionRadius { get; set; }

        [FieldProperty]
        public uint InteractionMask { get; set; }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Update(double delta)
        {
            var bounds = _boundingRectangle;
            var radius = CollisionRadius;

            bounds.X = Owner.Position.X - radius;
            bounds.Y = Owner.Position.Y - radius;

            bounds.Width = radius + radius;
            bounds.Height = radius + radius;
        }

        public CollisionResult HitTest(Collidable other)
        {
            if (this == other) return CollisionResult.None;
            if ((InteractionMask & other.InteractionMask) == 0) return CollisionResult.None;

            var ownType = CollisionType;
            var otherType = other.CollisionType;

            if (ownType == otherType)
            {
                if (ownType == CollisionType.Circle)
                {
                    return Owner.Position.DistanceTo(other.Owner.Position) - CollisionRadius - other.CollisionRadius <= 0
                        ? CollisionResult.Overlap
                        : CollisionResult.None;
                }
                else
                {
                    throw new NotImplementedException("TODO: Square vs square");
                }
            }
            else
            {
                throw new NotImplementedException("TODO: Circle vs square");
            }
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override ComponentBase Copy()
        {
            return new Collidable
            {
                CollisionType = CollisionType,
                CollisionRadius = CollisionRadius,
                InteractionMask = InteractionMask
            };
        }

        public bool IsAt(Vector2 point)
        {
            return _boundingRectangle.Contains(point);
        }

        public bool IsAt(Rectangle rect)
        {
            return _boundingRectangle.Intersects(rect);
        }

        public bool IsContainedBy(Rectangle rect)
        {
            return rect.Contains(_boundingRectangle);
        }
    }
}
