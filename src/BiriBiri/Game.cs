using System;
using System.Diagnostics;
using System.Linq;
using BiriBiri.Graphics.WebGL;
using BiriBiri.Input;
using BiriBiri.Services;
using BiriBiri.Utils;
using Bridge.WebGL;
using BiriBiri.Graphics;
using Bridge;
using Bridge.Html5;

namespace BiriBiri
{
    public class Game
    {
        public static Game Current;

        private readonly Camera _overlayCamera = new Camera();
        private readonly WebGLSpriteBatch _spriteBatch;

        private readonly CanvasElement _canvas;

        private readonly WebGLRenderingContext _gl;
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

        public WebGLRenderingContext Context { get; private set; }

        public Scene CurrentScene;

        private string _currentTextureId = ContentManager.DefaultTextureId;

        public int ViewportWidth { get { return _canvas.Width; } }
        public int ViewportHeight { get { return _canvas.Height; } }

        public Game(Keyboard keyboard, Mouse mouse, CanvasElement canvas, WebGLRenderingContext gl)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            Context = gl;

            Content = new ContentManager(gl);

            Console.Log(_overlayCamera.Matrix);

            CurrentScene = new Scene(this);

            _canvas = canvas;

            _gl = gl;
            _spriteBatch = new WebGLSpriteBatch(_gl);

            _overlayCamera.Matrix.InitOrthographicOffCenter(0, ViewportWidth, ViewportHeight, 0, 0, -1);
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
            _gl.ClearColor(0, 0, 0.54, 1); // AliceBlue
            _gl.Clear(_gl.COLOR_BUFFER_BIT);
            _gl.ClearDepth(1);

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
