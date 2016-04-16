using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiriBiri.Services
{
    public class TestImageService : ServiceBase
    {
        private readonly Vector2 _position = Vector2.Zero;
        private readonly Vector2 _size = new Vector2(128);

        public override void Draw()
        {
            const int Inset = 96;


            _position.X = Inset;
            _position.Y = Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position, _size, Vector2.One);

            _position.X = Game.ViewportWidth - Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position, _size, Vector2.One);

            _position.Y = Game.ViewportHeight - Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position, _size, Vector2.One);

            _position.X = Inset;

            Game.Draw(ContentManager.DefaultTextureId, _position, _size, Vector2.One);
        }
    }
}
