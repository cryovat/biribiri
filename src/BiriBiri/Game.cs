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
using BiriBiri.Level2;

namespace BiriBiri
{
    public class Game
    {
        public static Game Current;

        private readonly Camera _overlayCamera;
        private readonly WebGLSpriteBatch _spriteBatch;

        private readonly WebGLRenderingContext _gl;
        private readonly Action<Game> _onLoaded;
        private readonly object[] _factories = new object[0];
        private readonly ServiceBase[] _services = new ServiceBase[0];

        private bool _isBootstrapped;

        [FieldProperty]
        public Keyboard Keyboard { get; private set; }
        [FieldProperty]
        public Mouse Mouse { get; private set; }

        [FieldProperty]
        public Camera Camera { get; private set; }
        [FieldProperty]
        public ContentManager Content { get; private set; }
        [FieldProperty]
        public AnimationManager Sequences { get; private set; }

        [FieldProperty]
        public SceneBase CurrentScene { get; private set; }

        private string _currentTextureId = null;
        private SceneBase _loadingScene;

        public uint ViewportWidth { get { return Script.Get<uint>(_gl, "drawingBufferWidth"); } }
        public uint ViewportHeight { get { return Script.Get<uint>(_gl, "drawingBufferHeight"); } }

        public uint ViewportCenterX { get { return ViewportWidth/2; } }
        public uint ViewportCenterY { get { return ViewportHeight/2; } }

        public Game(Keyboard keyboard, Mouse mouse, WebGLRenderingContext gl, Action<Game> onLoaded)
        {
            _gl = gl;
            _onLoaded = onLoaded;
            _spriteBatch = new WebGLSpriteBatch(_gl);

            _overlayCamera = new Camera(ViewportWidth, ViewportHeight);

            Keyboard = keyboard;
            Mouse = mouse;

            Camera = new Camera(ViewportWidth, ViewportHeight);
            Content = new ContentManager(gl, AssetsLoaded);

            LoadScene<InitialScene>();
        }

        public void HandleTick(double delta)
        {
            if (!_isBootstrapped) return;

            Update(delta);
            Draw();
        }

        private void Update(double delta)
        {
            Keyboard.Update(delta);
            Mouse.Update(delta);

            if (CurrentScene != null)
            {
                CurrentScene.Update(delta);
            }

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

            if (CurrentScene != null)
            {
                _spriteBatch.CurrentCamera = Camera;
                CurrentScene.Draw();
                _spriteBatch.CurrentCamera = null;

                CurrentScene.DrawOverlays();
            }

            _spriteBatch.CurrentCamera = null;
            _spriteBatch.CurrentTexture = Content.GetTexture(ContentManager.DefaultTextureId);
            _spriteBatch.Begin();

            for (int i = 0; i < _services.Length; i++)
            {
                _services[i].Draw();
            }

            _spriteBatch.End();
        }


        private static readonly Vector2 _defaultScale = Vector2.One;

        public void Draw(string textureId, Vector2 position)
        {
            Draw(textureId, position, Vector2.One);
        }

        public void Draw(string textureId, Vector2 postion, Vector2 scale)
        {
            if (_currentTextureId != textureId)
            {
                if (_spriteBatch.IsStarted)
                {
                    _spriteBatch.End();
                }


                _spriteBatch.CurrentTexture = Content.GetTexture(textureId);
                _spriteBatch.Begin();
            }

            _spriteBatch.Draw(postion, 0, scale);
        }

        public void DrawFrame(string textureId, Vector2 postion, Vector2 scale, uint frameIndex)
        {
            if (_currentTextureId != textureId)
            {
                if (_spriteBatch.IsStarted)
                {
                    _spriteBatch.End();
                }

                _spriteBatch.CurrentTexture = Content.GetTexture(textureId);
                _spriteBatch.Begin();
            }

            _spriteBatch.Draw(postion, frameIndex, scale);
        }

        public void AddFactory<TFactory>(TFactory instance)
        {
            var existing = _services.FirstOrDefault(s => s is TFactory);
            if (existing != null) throw new InvalidOperationException("Factory already added: " + typeof(TFactory).GetClassName());

            _factories.Push(instance);
        }

        public TFactory GetFactory<TFactory>()
        {
            var existing = _factories.FirstOrDefault(s => s is TFactory);
            if (existing == null) throw new InvalidOperationException("Factory is unavailable: " + typeof(TFactory).GetClassName());

            return (TFactory)existing;
        }

        public void AddService<TService>() where TService : ServiceBase, new()
        {
            var existing = _services.FirstOrDefault(s => s is TService);
            if (existing != null) throw new InvalidOperationException("Service already added: " + typeof(TService).GetClassName());

            _services.Push(new TService()
            {
                Game = this
            });
        }

        public TService GetService<TService>() where TService : ServiceBase
        {
            var existing = _services.FirstOrDefault(s => s is TService);
            if (existing == null) throw new InvalidOperationException("Service is unavailable: " + typeof(TService).GetClassName());

            return (TService)existing;
        }

        public void LoadScene<TScene>() where TScene : SceneBase, new()
        {
            _loadingScene = new TScene();
            _loadingScene.ReceiveGame(this);

            _loadingScene.Load();

            if (CurrentScene != null)
            {
                CurrentScene.Teardown();
                CurrentScene = null;
            }

            if (Content.PendingCount == 0)
            {
                CurrentScene = _loadingScene;
                CurrentScene.Setup();
            }
            else if (!(_loadingScene is InitialScene))
            {
                Global.Alert("Loading screen not implemented yet. :(");
            }
        }

        private void AssetsLoaded(string[] failed)
        {
            if (_loadingScene == null) return;

            if (CurrentScene != null)
            {
                CurrentScene.Teardown();
                CurrentScene = null;
            }

            if (failed.Length == 0)
            {
                CurrentScene = _loadingScene;
                CurrentScene.Setup();
            }
            else
            {
                Global.Alert("Loading failed for assets: " + failed.Join(", "));
            }
        }

        private class DummyEntity : EntityBase, IClonable<DummyEntity>
        {
            public DummyEntity Clone()
            {
                throw new NotImplementedException();
            }
        }

        private class InitialScene : SceneBase<DummyEntity>
        {
            private readonly Vector2 _center = Vector2.Zero;
            private Game _game;

            protected override void Populate()
            {

            }

            internal override void ReceiveGame(Game game)
            {
                _game = game;
            }

            public override void Setup()
            {
                try
                {
                    _game._onLoaded(_game);
                    _game._isBootstrapped = true;
                }
                catch (Exception ex)
                {
                    Global.Alert("Error during game startup: " + ex.Message);
                }
            }

            public override void Clear()
            {
            }

            public override void Teardown()
            {
            }

            public override void Update(double delta)
            {
            }

            public override void Draw()
            {
            }

            public override void DrawOverlays()
            {
                _center.X = _game.ViewportCenterX;
                _center.Y = _game.ViewportCenterY;

                _game.Draw(ContentManager.DefaultTextureId, _center);
            }
        }
    }
}
