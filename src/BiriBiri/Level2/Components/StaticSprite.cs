using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Graphics;
using Bridge;
using Bridge.Html5;

namespace BiriBiri.Level2.Components
{
    [FileName("biriBiri.level2.js")]
    public class StaticSprite : DrawableComponentBase2D
    {
        private static readonly Vector2 _temp = Vector2.Zero;

        [FieldProperty]
        public string TextureId { get; set; }

        public override void Setup()
        {
            Scene.Drawables.Insert(this);
        }

        public override void Draw()
        {
            _temp.X = Math.Floor(Owner.Position.X);
            _temp.Y = Math.Floor(Owner.Position.Y);

            Game.Draw(TextureId, _temp, Owner.Scale);
        }

        public override void Teardown()
        {
            Scene.Drawables.Remove(this);
        }

        public override ComponentBase Copy()
        {
            var copy = new StaticSprite
            {
                TextureId = TextureId
            };

            return copy;
        }

        public override bool IsAt(Rectangle rect)
        {
            return rect.Contains(Owner.Position);
        }

        public override bool IsAt(Vector2 point)
        {
            return false;
        }

        public override bool IsContainedBy(Rectangle rect)
        {
            return rect.Contains(Owner.Position);
        }
    }
}
