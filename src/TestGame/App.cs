using System;
using System.ComponentModel;
using BiriBiri;
using BiriBiri.Level2.Components;
using BiriBiri.Services;
using Bridge;
using Bridge.Html5;
using TestGame.Scenes;

namespace TestGame
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var x = new Vector2(2, 3);
            x.Add(2);

            Bootstrap.Game("biri", 640, 480, "Test Game", game =>
            {
                Global.Set("game", game);

                game.UseLevel2();

               // game.AddService<TestImageService>();

                var presets = game.GetPresets2D();
                presets.CreateTemplate("box", b =>
                {
                    var sprite = b.GetComponent<StaticSprite>();
                    sprite.TextureId = ContentManager.DefaultTextureId;

                    var move = b.GetComponent<BasicMovement>();
                });


                game.LoadScene<TitleScene>();

            });
        }
    }
}