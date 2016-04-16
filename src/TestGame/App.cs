using BiriBiri;
using BiriBiri.Services;
using Bridge;
using Bridge.Html5;

namespace TestGame
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            var x = new Vector2(2, 3);
            x.Add(2);

            var game = Bootstrap.Game("biri", 640, 480, "Test Game");

            game.AddService<TestImageService>();
        }
    }
}