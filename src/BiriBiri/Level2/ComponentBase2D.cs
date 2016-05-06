using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Utils;
using Bridge;

namespace BiriBiri.Level2
{
    [FileName("biriBiri.level2.js")]
    public abstract class ComponentBase2D : ComponentBase
    {
        [FieldProperty]
        public Entity2D Owner { get; set; }

        public Scene2D Scene
        {
            get { return Owner.Scene; }
        }

        public Game Game
        {
            get { return Scene.Game; }
        }
    }

    [FileName("biriBiri.level2.js")]
    public abstract class DrawableComponentBase2D : ComponentBase2D, IDrawable, IQuadTreeItem
    {
        public abstract void Draw();

        public abstract bool IsAt(Rectangle rect);
        public abstract bool IsAt(Vector2 point);
        public abstract bool IsContainedBy(Rectangle rect);
    }
}
