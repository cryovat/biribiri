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
            container.AppendChild(canvas);
            canvas.Id = containerId + "-biribiri-canvas";
            canvas.InnerHTML = "Your browser doesn't appear to support the <code>&lt;canvas&gt;</code> element.";
            canvas.Width = width;
            canvas.Height = height;

            var keyboard = new Keyboard();
            Document.OnKeyUp = keyboard.HandleKeyUp;
            Document.OnKeyDown = keyboard.HandleKeyDown;

            var mouse = new Mouse();
            mouse.Claim(canvas);

            var gl = (canvas.GetContext(CanvasTypes.CanvasContextWebGLType.Experimental_WebGL) ?? canvas.GetContext(CanvasTypes.CanvasContextWebGLType.WebGL)).As<WebGLRenderingContext>();

            var lastUpdate = DateTime.Now;
            var ready = true;

            var game = new Game(keyboard, mouse, canvas, gl.As<WebGLRenderingContext>());
            BiriBiri.Game.Current = game;

            if (!string.IsNullOrWhiteSpace(title))
            {
                Document.Title = title + " - " + Document.Title;
            }

            gl.Viewport(0, 0, canvas.Width, canvas.Height);
            gl.Enable(gl.DEPTH_TEST);
            gl.DepthFunc(gl.LEQUAL);

            gl.ClearColor(0.94, 0.97, 1, 1); // AliceBlue
            gl.Clear(gl.COLOR_BUFFER_BIT);
            gl.ClearDepth(1);

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
