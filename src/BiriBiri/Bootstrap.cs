using System;
using BiriBiri.Input;
using BiriBiri.Level2;
using BiriBiri.Utils;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

namespace BiriBiri
{
    [FileName("biriBiri.level2.js")]
    public static class Level2Bootstrap
    {
        public static void UseLevel2(this Game game)
        {
            game.AddFactory(new EntityPresetManager<Entity2D>());
        }

        public static EntityPresetManager<Entity2D> GetPresets2D(this Game game)
        {
            return game.GetFactory<EntityPresetManager<Entity2D>>();
        }
    }

    public static class Bootstrap
    {
        public static void Game(string containerId, int width, int height, Action<Game> onLoaded)
        {
            Game(containerId, width, height, null, onLoaded);
        }

        public static void Game(string containerId, int width, int height, string title, Action<Game> onLoaded)
        {
            var container = Document.GetElementById(containerId);



            var canvas = Document.CreateElement<CanvasElement>("canvas");
            container.AppendChild(canvas);
            canvas.Id = containerId + "-biribiri-canvas";
            canvas.InnerHTML = "Your browser doesn't appear to support the <code>&lt;canvas&gt;</code> element.";
            canvas.Width = width;
            canvas.Height = height;

            var gl = (canvas.GetContext(CanvasTypes.CanvasContextWebGLType.WebGL) ?? canvas.GetContext(CanvasTypes.CanvasContextWebGLType.Experimental_WebGL)).As<WebGLRenderingContext>();
            if (gl == null)
            {
                container.RemoveChild(canvas);
                container.InnerHTML = "Your browser doesn't appear to support WebGL. Please try Chrome, Firefox or Edge.";
                return;
            }

            var keyboard = new Keyboard();
            Document.OnKeyUp = keyboard.HandleKeyUp;
            Document.OnKeyDown = keyboard.HandleKeyDown;

            var mouse = new Mouse();
            mouse.Claim(canvas);

            var lastUpdate = DateTime.Now;
            var ready = true;

            var game = new Game(keyboard, mouse, gl.As<WebGLRenderingContext>(), onLoaded);
            BiriBiri.Game.Current = game;

            if (!string.IsNullOrWhiteSpace(title))
            {
                Document.Title = title + " - " + Document.Title;
            }

            gl.Viewport(0, 0, canvas.Width, canvas.Height);
            gl.Enable(gl.DEPTH_TEST);
            gl.DepthFunc(gl.LEQUAL);

            int intervalId = -1;

            intervalId = Global.SetInterval(() =>
            {
                if (!ready) return;
                ready = false;

                var now = DateTime.Now;

                game.HandleTick((now - lastUpdate).TotalSeconds);

                lastUpdate = now;

                ready = true;
            }, 1000 / 60);

            Window.OnBlur = evt =>
            {
                keyboard.Clear();
            };
        }
    }
}
