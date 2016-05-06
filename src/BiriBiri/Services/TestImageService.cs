using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;

namespace BiriBiri.Services
{
    [FileName("biriBiri.js")]
    public class TestImageService : ServiceBase
    {
        private readonly Vector2 _position = Vector2.Zero;

        public override void Draw()
        {
            const int Inset = 96;


            _position.X = Inset;
            _position.Y = Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position);

            _position.X = Game.ViewportWidth - Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position);

            _position.Y = Game.ViewportHeight - Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position);

            _position.X = Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position);
        }
    }
}
