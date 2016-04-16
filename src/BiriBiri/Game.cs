using System;
using System.Linq;
using BiriBiri.Graphics.WebGL;
using BiriBiri.Input;
using BiriBiri.Services;
using BiriBiri.Utils;
using Bridge.WebGL;
using BiriBiri.Graphics;
using Bridge;

namespace BiriBiri
{
    public class Game
    {
        public static Game Current;

        private readonly Camera _overlayCamera = new Camera();
        private readonly WebGLSpriteBatch _spriteBatch;

        private readonly WebGLRenderingContext _glContext;
        private readonly ServiceBase[] _services = new ServiceBase[0];

        [FieldProperty]
        public Keyboard Keyboard { get; private set; }
        [FieldProperty]
        public Mouse Mouse { get; private set; }

        [FieldProperty]
        public Camera Camera { get; private set; } = new Camera();
        [FieldProperty]
        public EntityPresetManager Presets { get; private set; } = new EntityPresetManager();
        [FieldProperty]
        public ContentManager Content { get; private set; }

        public Scene CurrentScene;

        private string _currentTextureId = ContentManager.DefaultTextureId;

        public Game(Keyboard keyboard, Mouse mouse, WebGLRenderingContext glContext)
        {
            Keyboard = keyboard;
            Mouse = mouse;

            Content = new ContentManager(glContext);


            CurrentScene = new Scene(this);

            _glContext = glContext;
            _spriteBatch = new WebGLSpriteBatch(_glContext);
        }

        public void HandleTick(double delta)
        {
            if (Content.LoadedCount != Content.TotalCount) return;

            Update(delta);
            Draw();
        }

        private void Update(double delta)
        {
            Keyboard.Update(delta);
            Mouse.Update(delta);

            for (int i = 0; i < _services.Length; i++)
            {
                _services[i].Update(delta);
            }
        }

        private void Draw()
        {

            _spriteBatch.CurrentCamera = _overlayCamera;
            _spriteBatch.CurrentTexture = Content.GetTexture(ContentManager.DefaultTextureId);
            _spriteBatch.Begin();

            for (int i = 0; i < _services.Length; i++)
            {
                _services[i].Draw();
            }

            _spriteBatch.End();
        }

        public void Draw(string textureId, Vector2 postion, Vector2 size = null, Vector2 scale = null, TexCoords? texCoords = null)
        {
            if (_currentTextureId != textureId)
            {
                _spriteBatch.End();
                _spriteBatch.CurrentTexture = Content.GetTexture(textureId);
                _spriteBatch.Begin();
            }

            _spriteBatch.Draw(postion, size, scale, texCoords);
        }

        public void AddService<TService>() where TService : ServiceBase, new()
        {
            var existing = _services.FirstOrDefault(s => s is TService);
            if (existing != null) return;

            _services.Push(new TService()
            {
                Game = this
            });
        }
    }
}
