using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiriBiri.Input;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;

namespace BiriBiri
{
    public static class Bootstrap
    {
        public static Game Game(string containerId, int width, int height, string title = null)
        {
            var container = Document.GetElementById(containerId);

            var canvas = Document.CreateElement<CanvasElement>("canvas");
            canvas.Width = width;
            canvas.Height = height;

            var keyboard = new Keyboard();
            Document.OnKeyUp = keyboard.HandleKeyUp;
            Document.OnKeyDown = keyboard.HandleKeyDown;

            var mouse = new Mouse();
            mouse.Claim(canvas);

            container.AppendChild(canvas);
            var gl = canvas.GetContext(CanvasTypes.CanvasContextWebGLType.WebGL)
                ?? canvas.GetContext(CanvasTypes.CanvasContextWebGLType.Experimental_WebGL);

            var lastUpdate = DateTime.Now;
            var ready = true;

            var game = new Game(keyboard, mouse, gl.As<WebGLRenderingContext>());
            BiriBiri.Game.Current = game;

            if (!string.IsNullOrWhiteSpace(title))
            {
                Document.Title = title + " - " + Document.Title;
            }

            Global.SetInterval(() =>
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

            return game;
        }
    }
}
