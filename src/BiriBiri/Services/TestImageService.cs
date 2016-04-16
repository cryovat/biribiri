using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Services
{
    public class TestImageService : ServiceBase
    {
        public override void Draw()
        {
            Game.Draw(ContentManager.DefaultTextureId, Vector2.Zero);
        }
    }
}
